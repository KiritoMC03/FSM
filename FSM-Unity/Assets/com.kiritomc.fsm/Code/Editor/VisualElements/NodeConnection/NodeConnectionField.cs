using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionField : VisualElement
    {
        private const int HorizontalMargin = 10;
        public const int VerticalMargin = 5;

        public event Action<MouseDownEvent> OnMouseDown;
        private NodeConnectionPoint point;

        public NodeConnectionField(string connectionName)
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.marginTop = VerticalMargin;
            style.marginBottom = VerticalMargin;
            style.marginRight = HorizontalMargin;
            style.marginLeft = HorizontalMargin;
            Add(point = NodeConnectionPoint.Create());
            Add(new Label(connectionName)
            {
                style =
                {
                    marginLeft = HorizontalMargin,
                    marginRight = HorizontalMargin,
                },
            });

            point.OnMouseDown += evt => OnMouseDown?.Invoke(evt);
        }

        public Vector2 AnchorCenter()
        {
            return new Vector2(HorizontalMargin + point.resolvedStyle.width / 2f, VerticalMargin + point.resolvedStyle.height / 2f);
        }
    }
}