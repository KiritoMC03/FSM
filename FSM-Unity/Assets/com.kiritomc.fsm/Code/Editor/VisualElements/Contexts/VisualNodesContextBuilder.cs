using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class VisualNodesContextBuilder
    {
        public static VisualNodesContext<T> DefaultLayout<T>(this VisualNodesContext<T> ctx)
            where T: VisualNode
        {
            ctx.style.alignSelf = Align.FlexEnd;
            ctx.style.width = new StyleLength(new Length(Sizes.ContextWidth, Sizes.ContextWidthUnits));
            ctx.style.height = new StyleLength(new Length(Sizes.ContextHeight, Sizes.ContextHeightUnits));
            return ctx;
        }

        public static VisualNodesContext<T> DefaultColors<T>(this VisualNodesContext<T> ctx)
            where T: VisualNode
        {
            ctx.style.backgroundColor = Colors.ContextBackground;
            return ctx;
        }

        public static VisualNodesContext<T> DefaultInteractions<T>(this VisualNodesContext<T> ctx)
            where T: VisualNode
        {
            ctx.focusable = true;
            return ctx;
        }
    }
}