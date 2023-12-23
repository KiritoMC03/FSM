using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualNodeLinkRegistration : ICancelable
    {
        private readonly VisualNode parent;
        private readonly Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter;
        private readonly Action<IVisualNodeWithLinkExit> gotHandler;
        private readonly Func<IVisualNodeWithLinkExit> currentGetter;
        private readonly NodeLinkFieldView linkFieldView;
        private readonly Subscription connectionFieldViewMouseDownSubscription;
        private readonly VisualNodeLinkDrawerRegistration linkDrawerRegistration;

        public bool IsDisposed { get; private set; }

        public VisualNodeLinkRegistration(
            VisualNode parent,
            string linkName,
            Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter,
            Action<IVisualNodeWithLinkExit> gotHandler,
            Func<IVisualNodeWithLinkExit> currentGetter)
        {
            this.parent = parent;
            this.asyncTargetGetter = asyncTargetGetter;
            this.gotHandler = gotHandler;
            this.currentGetter = currentGetter;
            linkFieldView = new NodeLinkFieldView(linkName);
            connectionFieldViewMouseDownSubscription = linkFieldView.SubscribeMouseDown(ConnectionFieldViewMouseDownHandler);
            linkDrawerRegistration = new VisualNodeLinkDrawerRegistration(parent, GetLinkStart, GetLinkEnd);

            parent.Add(linkFieldView);
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.Remove(linkFieldView);
            connectionFieldViewMouseDownSubscription?.Dispose();
            linkDrawerRegistration?.Dispose();
            IsDisposed = true;
        }

        private async void ConnectionFieldViewMouseDownHandler(MouseDownEvent _)
        {
            IVisualNodeWithLinkExit result = await asyncTargetGetter();
            gotHandler(result);
        }

        private Vector2? GetLinkStart() => linkFieldView?.AnchorCenter();
        private Vector2? GetLinkEnd() => currentGetter()?.GetAbsoluteLinkPointPos();
    }
}