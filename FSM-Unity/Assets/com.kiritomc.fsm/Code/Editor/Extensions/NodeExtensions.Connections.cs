using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Extensions
{
    public static partial class NodeExtensions
    {
        public static NodeConnectionField BindConnectionField<T>(this Node node,
            string fieldName,
            FieldWrapper<T> targetField,
            Func<Task<Node>> asyncTargetGetter) where T : NodeWithConnection
        {
            NodeConnectionField connectionField = new NodeConnectionField($"{fieldName}");
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
            void RepaintNode()
            {
                node.Repaint();
                node.BringToFront();
            }
        }

        public static async Task<StateTransition> CrateTransitionAsync<T>(this StateNode node, Func<Task<T>> asyncTargetGetter, Predicate<T> checkValid)
            where T : StateNode
        {
            T target = await asyncTargetGetter.Invoke();
            if (target == null || target == node || !checkValid.Invoke(target)) return default;

            StateTransition transition = new StateTransition(node, target);
            node.Add(transition);
            node.Disposables.Add(transition);
            node.Disposables.Add(target.ListenChanges(RepaintTransition));
            node.ChildrenRepaintHandler.Add(transition);
            return transition;
            void RepaintTransition() => transition.Repaint();
        }
    }
}