using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class ContextVisualBuilder
    {
        public static Context DefaultLayout(this Context ctx)
        {
            ctx.style.alignSelf = Align.FlexEnd;
            ctx.style.width = new StyleLength(new Length(Sizes.ContextWidth, Sizes.ContextWidthUnits));
            ctx.style.height = new StyleLength(new Length(Sizes.ContextHeight, Sizes.ContextHeightUnits));
            return ctx;
        }

        public static Context DefaultColors(this Context ctx)
        {
            ctx.style.backgroundColor = Colors.ContextBackground;
            return ctx;
        }

        public static Context DefaultInteractions(this Context ctx)
        {
            ctx.focusable = true;
            return ctx;
        }
    }
}