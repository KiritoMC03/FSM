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

        protected virtual Task<Node> RequestConnection()
        {
            TaskCompletionSource<Node> completionSource = new TaskCompletionSource<Node>();
            ConnectionRequestEvent @event = ConnectionRequestEvent.GetPooled();
            @event.target = this;
            @event.SetConnectionCallback = node => completionSource.SetResult(node);
            SendEvent(@event);
            return completionSource.Task;
        }

        protected NodeLinkFieldView BuildConnectionField<T>(
            string fieldName,
            FieldWrapper<T> targetField,
            Func<Task<Node>> asyncTargetGetter) where T : NodeWithConnection
        {
            NodeLinkFieldView linkFieldView = new NodeLinkFieldView($"{fieldName}");
            LineDrawerRegistration lineDrawerRegistration = LineDrawerForConnection(GetInputPosition, GetFieldValue);
            NodeChangingListeningRegistration changingRegistration = default;

            ChildrenRepaintHandler.Add(lineDrawerRegistration);
            Disposables.Add(lineDrawerRegistration);
            Disposables.Add(linkFieldView.SubscribeMouseDown(MouseDownHandler));
            Disposables.Add(targetField.Subscribe(newValue =>
            {
                if (newValue == null) return;
                RepaintNode();
                changingRegistration?.Dispose();
                changingRegistration = newValue.OnChanged(RepaintNode);
            }));

            Add(linkFieldView);
            return linkFieldView;

            Vector2? GetInputPosition() => linkFieldView.AnchorCenter();
            T GetFieldValue() => targetField.Value;
            async void MouseDownHandler(MouseDownEvent _)
            {
                Node target = await asyncTargetGetter.Invoke();
                if (target is T correct && target != this) targetField.Value = correct;
            }
            void RepaintNode()
            {
                Repaint();
                BringToFront();
            }
        }

        private LineDrawerRegistration LineDrawerForConnection<T>(Func<Vector2?> startGetter, Func<T> targetFieldGetter) 
            where T: NodeWithConnection
        {
            NodeConnectionDrawer drawer;
            Add(drawer = new NodeConnectionDrawer());
            return new LineDrawerRegistration(drawer, this, startGetter, GetEndPosition);
            Vector2? GetEndPosition() => targetFieldGetter.Invoke()?.GetAbsoluteConnectionPos();
        }

        // protected NodeConnectionField BuildCognnectionField<T>(
        //     string fieldName,
        //     FieldWrapper<T> targetField,
        //     Func<Task<Node>> asyncTargetGetter) where T : NodeWithConnection
        // {
        //     NodeConnectionField connectionField = new NodeConnectionField($"{fieldName}");
        //     LineDrawerRegistration lineDrawerRegistration = LineDrawerForConnection(GetInputPosition, GetFieldValue);
        //     NodeChangingListeningRegistration changingRegistration = default;
        //
        //     ChildrenRepaintHandler.Add(lineDrawerRegistration);
        //     Disposables.Add(lineDrawerRegistration);
        //     Disposables.Add(connectionField.SubscribeMouseDown(MouseDownHandler));
        //     Disposables.Add(targetField.Subscribe(newValue =>
        //     {
        //         if (newValue == null) return;
        //         RepaintNode();
        //         changingRegistration?.Dispose();
        //         changingRegistration = newValue.OnChanged(RepaintNode);
        //     }));
        //
        //     Add(connectionField);
        //     return connectionField;
        //
        //     Vector2? GetInputPosition() => connectionField.AnchorCenter();
        //     T GetFieldValue() => targetField.Value;
        //     async void MouseDownHandler(MouseDownEvent _)
        //     {
        //         Node target = await asyncTargetGetter.Invoke();
        //         if (target is T correct && target != this) targetField.Value = correct;
        //     }
        //     void RepaintNode()
        //     {
        //         Repaint();
        //         BringToFront();
        //     }
        // }

        public virtual void Dispose()
        {
            foreach (IDisposable disposable in Disposables) disposable.Dispose();
        }
    }
}