using System.Collections.Generic;
using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class SelectVisualNodesManipulator<T> : Manipulator
        where T: VisualNode
    {
        private readonly VisualNodesContext<T> context;
        private readonly List<VisualNodeStyleRegistration> currentSelectedRegistrations = new List<VisualNodeStyleRegistration>(10);
        private Vector3 downPoint;
        private bool isPressed;

        public SelectVisualNodesManipulator(VisualNodesContext<T> context)
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
            ClearSelected();
            if (isPressed && downPoint == e.position)
            {
                T node = target.panel.Pick<T>(downPoint);
                if (node != null)
                {
                    if (!context.SelectedNodes.Contains(node))
                    {
                        ApplySelectedStyle(node);
                        context.SelectedNodes.Add(node);
                    }
                    isPressed = false;
                    return;
                }
            }
            isPressed = false;
        }

        private void ClearSelected()
        {
            foreach (VisualNodeStyleRegistration registration in currentSelectedRegistrations)
            {
                registration?.Dispose();
            }
            currentSelectedRegistrations.Clear();
            context.SelectedNodes.Clear();
        }

        private void ApplySelectedStyle(VisualNode node)
        {
            currentSelectedRegistrations.Add(new VisualNodeStyleRegistration(node, Style));
            void Style()
            {
                node.style.borderTopColor = node.style.borderBottomColor = node.style.borderRightColor = node.style.borderLeftColor = Colors.SelectedNodeBorderColor;
            }
        }
    }
}