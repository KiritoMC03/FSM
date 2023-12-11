using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class ContextBuilder
    {
        public static Context DefaultLayout(this Context ctx)
        {
            ctx.style.alignSelf = Align.FlexEnd;
            ctx.style.width = new StyleLength(new Length(80f, LengthUnit.Percent));
            ctx.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
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