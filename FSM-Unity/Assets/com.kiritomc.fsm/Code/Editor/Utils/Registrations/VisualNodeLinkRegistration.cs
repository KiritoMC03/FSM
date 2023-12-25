using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualNodeLinkRegistration : ICancelable
    {
        public readonly NodeLinkFieldView LinkFieldView;
        private readonly VisualNode parent;
        private readonly string linkName;
        private readonly Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter;
        private readonly Action<string, IVisualNodeWithLinkExit> gotHandler;
        private readonly Func<string, IVisualNodeWithLinkExit> currentGetter;
        private readonly Func<IVisualNodeWithLinkExit, bool> checker;
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
            Func<string, IVisualNodeWithLinkExit> currentGetter,
            Func<IVisualNodeWithLinkExit, bool> checker,
            int insertFieldAt = -1)
        {
            this.parent = parent;
            this.linkName = linkName;
            this.asyncTargetGetter = asyncTargetGetter;
            this.gotHandler = gotHandler;
            this.currentGetter = currentGetter;
            this.checker = checker;
            LinkFieldView = new NodeLinkFieldView(linkName);
            connectionFieldViewMouseDownSubscription = LinkFieldView.SubscribeMouseDown(ConnectionFieldViewMouseDownHandler);
            linkDrawerRegistration = new VisualNodeLinkDrawerRegistration(parent, GetLinkStart, GetLinkEnd);
            parentChangedRegistration = parent.OnChanged(Repaint);

            if (insertFieldAt != -1) parent.Insert(insertFieldAt, LinkFieldView);
            else parent.Add(LinkFieldView);
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.Remove(LinkFieldView);
            parentChangedRegistration?.Dispose();
            linkDrawerRegistration?.Dispose();
            connectionFieldViewMouseDownSubscription?.Dispose();
            IsDisposed = true;
        }

        private async void ConnectionFieldViewMouseDownHandler(MouseDownEvent _)
        {
            IVisualNodeWithLinkExit result = await asyncTargetGetter();
            SetTarget(result);
        }

        public void SetTarget(IVisualNodeWithLinkExit target)
        {
            if (target != null && !checker(target)) return;
            targetChangedRegistration?.Dispose();
            if (target != null) targetChangedRegistration = ((VisualNode)target).OnChanged(Repaint);
            gotHandler(linkName, target);
            linkDrawerRegistration.Clear();
            linkDrawerRegistration.Repaint();
        }

        private Vector2? GetLinkStart() => LinkFieldView?.AnchorCenter();
        private Vector2? GetLinkEnd() => currentGetter(linkName)?.GetAbsoluteLinkPointPos();

        private void Repaint()
        {
            linkDrawerRegistration.Repaint();
        }
    }
}