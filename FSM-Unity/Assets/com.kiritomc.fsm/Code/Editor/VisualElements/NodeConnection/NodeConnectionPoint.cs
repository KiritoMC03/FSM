using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionPoint : VisualElement
    {
        public event Action<MouseDownEvent> OnMouseDown;

        private NodeConnectionPoint()
        {
        }
        
        public static NodeConnectionPoint Create()
        {
            NodeConnectionPoint result = new NodeConnectionPoint()
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
            result.RegisterCallback<MouseDownEvent>(evt => result.OnMouseDown?.Invoke(evt));
            return result;
        }
    }
}