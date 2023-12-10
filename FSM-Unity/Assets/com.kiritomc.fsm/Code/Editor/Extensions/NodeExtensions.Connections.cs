using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FSM.Editor.Extensions
{
    public static partial class NodeExtensions
    {
        public static NodeConnectionField BindConnectionField<T>(this Node node,
            string fieldName,
            FieldWrapper<T> targetField,
            Func<Task<Node>> asyncTargetGetter) where T : NodeWithConnection
        {
            NodeConnectionField connectionField = NodeConnectionField.Create($"{fieldName}");
            LineDrawerRegistration lineDrawerRegistration = node.AddLineDrawerForConnection(GetInputPosition, GetFieldValue);
            NodeChangingListeningRegistration changingRegistration = default;
            connectionField.OnMouseDown += async _ =>
            {
                Node target = await asyncTargetGetter.Invoke();
                if (target is T correct && target != node)
                {
                    targetField.Value = correct;
                    RepaintNode();
                    changingRegistration?.Dispose();
                    changingRegistration = correct.ListenChanges(RepaintNode);
                }
            };

            node.ChildrenRepaintHandler.Add(lineDrawerRegistration);
            node.Disposables.Add(lineDrawerRegistration);
            node.Add(connectionField);
            return connectionField;

            Vector2? GetInputPosition() => connectionField.AnchorCenter();
            T GetFieldValue() => targetField.Value;
            void RepaintNode() => node.Repaint();
        }

        public static async Task CrateTransitionAsync<T>(this StateNode node, Func<Task<T>> asyncTargetGetter) where T: StateNode
        {
            T target = await asyncTargetGetter.Invoke();
            LineDrawerRegistration lineDrawerRegistration = node.AddLineDrawerForTransition(NodePosition, GetFieldValue);
            return;
            Vector2? NodePosition() => Vector2.zero;
            T GetFieldValue() => target;
        }
    }
}