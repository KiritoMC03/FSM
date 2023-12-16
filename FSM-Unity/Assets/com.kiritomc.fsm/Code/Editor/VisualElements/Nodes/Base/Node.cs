using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class Node : VisualElement, ICustomRepaintHandler, IDisposable
    {
        public string Name;
        internal readonly List<IDisposable> Disposables = new List<IDisposable>(100);
        internal readonly List<ICustomRepaintHandler> ChildrenRepaintHandler = new List<ICustomRepaintHandler>(2);
        internal VisualElement Header;
        internal TextElement Label;

        public Vector2 ResolvedPlacement => new Vector2(resolvedStyle.left, resolvedStyle.top);
        public Vector2 Placement => new Vector2(style.left.value.value, style.top.value.value);
        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        protected Node()
        {
        }

        protected Node(string nodeName)
        {
            Name = nodeName;
            this.DefaultHeader(nodeName);
            ApplyBaseStyle();
        }

        public NodeChangingListeningRegistration OnChanged(Action onChanged)
        {
            if (onChanged == null) return default;
            return new NodeChangingListeningRegistration(this, onChanged);
        }

        public abstract string GetMetadataForSerialization();
        public abstract void HandleDeserializedMetadata(string metadata);

        public virtual void Repaint()
        {
            MarkDirtyRepaint();
            ChildrenRepaintHandler.Repaint();
        }

        protected void ApplyBaseStyle()
        {
            style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = 8;
            style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = Colors.NodeBorderColor;
            style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = Sizes.NodeBorderWidth;
            style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = 8;
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.minWidth = 200;
            style.minHeight = 50;
            style.backgroundColor = Colors.NodeBackground;
        }

        protected virtual Task<Node> RequestConnection()
        {
            TaskCompletionSource<Node> completionSource = new TaskCompletionSource<Node>();
            ConnectionRequestEvent @event = ConnectionRequestEvent.GetPooled();
            @event.target = this;
            @event.SetConnectionCallback = node => completionSource.SetResult(node);
            SendEvent(@event);
            return completionSource.Task;
        }

        public virtual void Dispose()
        {
            foreach (IDisposable disposable in Disposables) disposable.Dispose();
        }
    }
}