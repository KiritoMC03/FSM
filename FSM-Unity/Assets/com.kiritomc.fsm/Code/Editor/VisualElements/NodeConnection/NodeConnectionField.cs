using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionField : VisualElement
    {
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
            result.Add(NodeConnectionPoint.Create());
            result.Add(new Label(connectionName)
            {
                style =
                {
                    marginLeft = horizontalMargin,
                    marginRight = horizontalMargin,
                },
            });
            return result;
        }
    }
}