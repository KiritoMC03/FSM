using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionField : VisualElement
    {
        public event Action<MouseDownEvent> OnMouseDown;
        private NodeConnectionPoint point;

        public NodeConnectionField(string connectionName)
        {
            const int horizontalMargin = 10;
            const int verticalMargin = 5;
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.marginTop = verticalMargin;
            style.marginBottom = verticalMargin;
            style.marginRight = horizontalMargin;
            style.marginLeft = horizontalMargin;
            Add(point = NodeConnectionPoint.Create());
            Add(new Label(connectionName)
            {
                style =
                {
                    marginLeft = horizontalMargin,
                    marginRight = horizontalMargin,
                },
            });

            point.OnMouseDown += evt => OnMouseDown?.Invoke(evt);
        }

        public Vector2 AnchorCenter()
        {
            return new Vector2(
                resolvedStyle.top + point.resolvedStyle.height / 2f,
                resolvedStyle.left + point.resolvedStyle.width / 2f);
        }
    }
}