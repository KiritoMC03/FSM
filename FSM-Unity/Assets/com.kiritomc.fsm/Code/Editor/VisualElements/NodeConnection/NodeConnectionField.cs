using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionField : VisualElement
    {
        public event Action<MouseDownEvent> OnMouseDown;
        public NodeConnectionPoint point;

        private NodeConnectionField()
        {
        }
        
        public static NodeConnectionField Create(string connectionName)
        {
            const int horizontalMargin = 10;
            const int verticalMargin = 5;
            NodeConnectionField result = new NodeConnectionField()
            {
                style =
                {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    marginTop = verticalMargin,
                    marginBottom = verticalMargin,
                    marginRight = horizontalMargin,
                    marginLeft = horizontalMargin,
                }
            };
            result.Add(result.point = NodeConnectionPoint.Create());
            result.Add(new Label(connectionName)
            {
                style =
                {
                    marginLeft = horizontalMargin,
                    marginRight = horizontalMargin,
                },
            });

            result.point.OnMouseDown += evt => result.OnMouseDown?.Invoke(evt);
            return result;
        }

        public Vector2 AnchorCenter()
        {
            return new Vector2(
                resolvedStyle.top + point.resolvedStyle.height / 2f,
                resolvedStyle.left + point.resolvedStyle.width / 2f);
        }
    }
}