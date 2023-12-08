using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionPoint : VisualElement
    {
        private NodeConnectionPoint()
        {
        }
        
        public static NodeConnectionPoint Create()
        {
            return new NodeConnectionPoint()
            {
                style =
                {
                    minHeight = Sizes.ConnectionNodePoint,
                    minWidth = Sizes.ConnectionNodePoint,
                    borderTopLeftRadius = Sizes.ConnectionNodePoint,
                    borderTopRightRadius = Sizes.ConnectionNodePoint,
                    borderBottomLeftRadius = Sizes.ConnectionNodePoint,
                    borderBottomRightRadius = Sizes.ConnectionNodePoint,
                    backgroundColor = Colors.NodeConnectionBackground,
                },
            };
        }
    }
}