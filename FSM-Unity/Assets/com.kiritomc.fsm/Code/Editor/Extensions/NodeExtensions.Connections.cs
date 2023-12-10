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

        public static async Task<StateTransition> CrateTransitionAsync<T>(this StateNode node, Func<Task<T>> asyncTargetGetter, Predicate<T> checkValid) where T : StateNode
        {
            T target = await asyncTargetGetter.Invoke();
            if (target == null || target == node || !checkValid.Invoke(target)) return default;

            StateTransition transition = new StateTransition(target);
            node.Add(transition);
            node.Disposables.Add(transition);
            node.Disposables.Add(target.ListenChanges(RepaintTransition));
            node.ChildrenRepaintHandler.Add(transition);
            transition.SetLineDrawerRegistrationLink(transition.AddLineDrawerForTransition(StartPosition, TargetPosition));
            return transition;

            Vector2? StartPosition()
            {
                Rect nodeBound = node.worldBound;
                Rect targetBound = target.worldBound;
                float xOffset = nodeBound.x - targetBound.x;
                float yOffset = nodeBound.y - targetBound.y;
                if (Mathf.Abs(xOffset) > Mathf.Abs(yOffset))
                    return xOffset < 0 ? new Vector2(node.resolvedStyle.width / 2f, 0f) : new Vector2(-node.resolvedStyle.width / 2f, 0);
                return yOffset < 0 ? new Vector2(0, node.resolvedStyle.height / 2f) : new Vector2(0, -node.resolvedStyle.height / 2f);
            }

            Vector2? TargetPosition()
            {
                Rect nodeBound = node.worldBound;
                Rect targetBound = target.worldBound;
                float xOffset = nodeBound.x - targetBound.x;
                float yOffset = nodeBound.y - targetBound.y;
                if (Mathf.Abs(xOffset) > Mathf.Abs(yOffset))
                    return xOffset > 0 ? new Vector2(targetBound.xMax, targetBound.center.y) : new Vector2(targetBound.xMin, targetBound.center.y);
                return yOffset > 0 ? new Vector2(targetBound.center.x, targetBound.yMax) : new Vector2(targetBound.center.x, targetBound.yMin);
            }

            T GetFieldValue() => target;
            void RepaintTransition() => transition.Repaint();
        }
    }
}