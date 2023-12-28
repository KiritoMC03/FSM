using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeLinkPoint : VisualElement
    {
        public event Action<MouseDownEvent> OnMouseDown;

        public NodeLinkPoint()
        {
            style.minHeight = Sizes.ConnectionNodePoint;
            style.minWidth = Sizes.ConnectionNodePoint;
            style.borderTopLeftRadius = Sizes.ConnectionNodePoint;
            style.borderTopRightRadius = Sizes.ConnectionNodePoint;
            style.borderBottomLeftRadius = Sizes.ConnectionNodePoint;
            style.borderBottomRightRadius = Sizes.ConnectionNodePoint;
            style.backgroundColor = Colors.NodeConnectionBackground;
            RegisterCallback<MouseDownEvent>(evt => OnMouseDown?.Invoke(evt));
        }
    }
}