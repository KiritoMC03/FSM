using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class SelectNodesManipulator<T> : Manipulator
        where T: Node
    {
        private readonly NodesContext<T> context;
        private Vector3 downPoint;
        private bool isPressed;
        private VisualElement selectionMarker = new VisualElement()
        {
            pickingMode = PickingMode.Ignore,
            style =
            {
                position = Position.Absolute,
                width = new StyleLength(new Length(100, LengthUnit.Percent)),
                height = new StyleLength(new Length(100, LengthUnit.Percent)),
                
                borderTopColor = Colors.SelectedNodeBorderColor,
                borderBottomColor = Colors.SelectedNodeBorderColor,
                borderRightColor = Colors.SelectedNodeBorderColor,
                borderLeftColor = Colors.SelectedNodeBorderColor,
                
                borderTopWidth = Sizes.SelectedNodeBorderWidth,
                borderBottomWidth = Sizes.SelectedNodeBorderWidth,
                borderLeftWidth = Sizes.SelectedNodeBorderWidth,
                borderRightWidth = Sizes.SelectedNodeBorderWidth,
                
                borderTopRightRadius = Sizes.NodeBorderRadius,
                borderTopLeftRadius = Sizes.NodeBorderRadius,
                borderBottomRightRadius = Sizes.NodeBorderRadius,
                borderBottomLeftRadius = Sizes.NodeBorderRadius,
                
                marginTop = -Sizes.NodePadding,
                marginBottom = -Sizes.NodePadding,
                marginLeft = -Sizes.NodePadding,
                marginRight = -Sizes.NodePadding,
            },
        };

        public SelectNodesManipulator(NodesContext<T> context)
        {
            this.context = context;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(HandleDown);
            target.RegisterCallback<PointerUpEvent>(HandleUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(HandleDown);
            target.UnregisterCallback<PointerUpEvent>(HandleUp);
        }

        private void HandleDown(PointerDownEvent e)
        {
            T node = target.panel.Pick<T>(e.position);
            if (node == null) return;
            downPoint = e.position;
            isPressed = true;
        }

        private void HandleUp(PointerUpEvent e)
        {
            if (isPressed && downPoint == e.position)
            {
                T node = target.panel.Pick<T>(downPoint);
                if (node != null)
                {
                    if (!context.SelectedNodes.Contains(node))
                    {
                        context.SelectedNodes.Add(node);
                        node.Add(selectionMarker);
                    }
                    isPressed = false;
                    return;
                }
            }
            isPressed = false;
            ClearSelected();
        }

        private void ClearSelected()
        {
            foreach (T selectedNode in context.SelectedNodes) selectedNode.Remove(selectionMarker);
            context.SelectedNodes.Clear();
        }
    }
}