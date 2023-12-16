using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FSM.Editor
{
    public abstract class NodeWithConnection: Node //ToDo: name?
    {
        protected NodeConnectionPoint Connection;

        protected NodeWithConnection(string nodeName) : base(nodeName)
        {
            Connection = Header.ConnectionPointRelativeTo(Label, ConnectionPosition.Right);
        }

        public virtual Vector2 GetAbsoluteConnectionPos()
        {
            return new Vector2(Connection.worldBound.x + Sizes.ConnectionNodePoint / 2f, Connection.worldBound.y + Sizes.ConnectionNodePoint / 2f);
        }

        protected NodeConnectionField BuildConnectionField<T>(
            string fieldName,
            FieldWrapper<T> targetField,
            Func<Task<Node>> asyncTargetGetter) where T : NodeWithConnection
        {
            NodeConnectionField connectionField = new NodeConnectionField($"{fieldName}");
            LineDrawerRegistration lineDrawerRegistration = LineDrawerForConnection(GetInputPosition, GetFieldValue);
            NodeChangingListeningRegistration changingRegistration = default;
            connectionField.OnMouseDown += async _ =>
            {
                Node target = await asyncTargetGetter.Invoke();
                if (target is T correct && target != this)
                {
                    targetField.Value = correct;
                    RepaintNode();
                    changingRegistration?.Dispose();
                    changingRegistration = correct.OnChanged(RepaintNode);
                }
            };

            ChildrenRepaintHandler.Add(lineDrawerRegistration);
            Disposables.Add(lineDrawerRegistration);
            Add(connectionField);
            return connectionField;

            Vector2? GetInputPosition() => connectionField.AnchorCenter();
            T GetFieldValue() => targetField.Value;
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
    }
}