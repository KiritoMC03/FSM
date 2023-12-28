using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LeftPanel : VisualElement
    {
        public LeftPanel()
        {
            style.alignSelf = Align.FlexStart;
            style.width = new StyleLength(new Length(Sizes.LeftPanelWidth, Sizes.LeftPanelWidthUnits));
            style.height = new StyleLength(new Length(Sizes.LeftPanelHeight, Sizes.LeftPanelHeightUnits));
            style.borderRightColor = Colors.LeftPanelBorder;
            style.borderRightWidth = Sizes.LeftPanelBorderWidth;
        }
    }
}