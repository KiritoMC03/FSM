using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class ContextVisualBuilder
    {
        public static NodesContext<T> DefaultLayout<T>(this NodesContext<T> ctx)
            where T: Node
        {
            ctx.style.alignSelf = Align.FlexEnd;
            ctx.style.width = new StyleLength(new Length(Sizes.ContextWidth, Sizes.ContextWidthUnits));
            ctx.style.height = new StyleLength(new Length(Sizes.ContextHeight, Sizes.ContextHeightUnits));
            return ctx;
        }

        public static NodesContext<T> DefaultColors<T>(this NodesContext<T> ctx)
            where T: Node
        {
            ctx.style.backgroundColor = Colors.ContextBackground;
            return ctx;
        }

        public static NodesContext<T> DefaultInteractions<T>(this NodesContext<T> ctx)
            where T: Node
        {
            ctx.focusable = true;
            return ctx;
        }
    }
}