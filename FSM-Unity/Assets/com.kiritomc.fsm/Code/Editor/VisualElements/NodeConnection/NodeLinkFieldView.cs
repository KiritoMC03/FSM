using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeLinkFieldView : VisualElement
    {
        private const int HorizontalMargin = 10;
        public const int VerticalMargin = 5;

        private event Action<MouseDownEvent> OnMouseDown;
        private NodeLinkPoint point;

        public NodeLinkFieldView(string connectionName)
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.marginTop = VerticalMargin;
            style.marginBottom = VerticalMargin;
            style.marginRight = HorizontalMargin;
            style.marginLeft = HorizontalMargin;
            Add(point = new NodeLinkPoint());
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

        public Subscription SubscribeMouseDown(Action<MouseDownEvent> mouseDownHandler)
        {
            OnMouseDown += mouseDownHandler;
            return new Subscription(() => OnMouseDown -= mouseDownHandler);
        }
    }
}