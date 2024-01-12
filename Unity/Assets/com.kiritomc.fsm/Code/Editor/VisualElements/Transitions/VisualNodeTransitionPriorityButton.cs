using UnityEngine.UIElements;

namespace FSM.Editor
{
    public sealed class VisualNodeTransitionPriorityButton : Button
    {
        public VisualNodeTransitionPriorityButton(bool isUp)
        {
            style.maxHeight = style.maxWidth = style.minHeight = style.minWidth = Sizes.TransitionPriorityButton;
            style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = Sizes.TransitionPriorityButtonRadius;
            style.backgroundColor = Colors.NodeConnectionBackground;
            text = isUp ? "\u2191" : "\u2193";
        }
    }
}