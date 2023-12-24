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
        private readonly string linkName;
        private readonly Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter;
        private readonly Action<string, IVisualNodeWithLinkExit> gotHandler;
        private readonly Func<string, IVisualNodeWithLinkExit> currentGetter;
        private readonly NodeLinkFieldView linkFieldView;
        private readonly Subscription connectionFieldViewMouseDownSubscription;
        private readonly VisualNodeLinkDrawerRegistration linkDrawerRegistration;
        private readonly NodeChangingListeningRegistration parentChangedRegistration;
        private NodeChangingListeningRegistration targetChangedRegistration;

        public bool IsDisposed { get; private set; }

        public VisualNodeLinkRegistration(
            VisualNode parent,
            string linkName,
            Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter,
            Action<string, IVisualNodeWithLinkExit> gotHandler,
            Func<string, IVisualNodeWithLinkExit> currentGetter)
        {
            this.parent = parent;
            this.linkName = linkName;
            this.asyncTargetGetter = asyncTargetGetter;
            this.gotHandler = gotHandler;
            this.currentGetter = currentGetter;
            linkFieldView = new NodeLinkFieldView(linkName);
            connectionFieldViewMouseDownSubscription = linkFieldView.SubscribeMouseDown(ConnectionFieldViewMouseDownHandler);
            linkDrawerRegistration = new VisualNodeLinkDrawerRegistration(parent, GetLinkStart, GetLinkEnd);
            parentChangedRegistration = parent.OnChanged(Repaint);

            parent.Add(linkFieldView);
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.Remove(linkFieldView);
            parentChangedRegistration?.Dispose();
            linkDrawerRegistration?.Dispose();
            connectionFieldViewMouseDownSubscription?.Dispose();
            IsDisposed = true;
        }

        private async void ConnectionFieldViewMouseDownHandler(MouseDownEvent _)
        {
            IVisualNodeWithLinkExit result = await asyncTargetGetter();
            targetChangedRegistration?.Dispose();
            if (result != null) targetChangedRegistration = ((VisualNode)result).OnChanged(Repaint);
            linkDrawerRegistration.Repaint();
            gotHandler(linkName, result);
        }

        private Vector2? GetLinkStart() => linkFieldView?.AnchorCenter();
        private Vector2? GetLinkEnd() => currentGetter(linkName)?.GetAbsoluteLinkPointPos();

        private void Repaint()
        {
            linkDrawerRegistration.Repaint();
        }
    }
}