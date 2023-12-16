using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StateTransition : VisualElement, IDisposable, ICustomRepaintHandler
    {
        public readonly StateNode Source;
        public readonly StateNode Target;
        private TransitionDrawer drawer;
        private TransitionContext context;

        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public StateTransition(StateNode source, StateNode target)
        {
            Source = source;
            Target = target;
            style.position = Position.Absolute;
            style.top = 0;
            style.bottom = 0;
            style.alignSelf = Align.Center;
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.Center;
            Add(drawer = new TransitionDrawer(source, target, () => Fabric.CreateTransitionContext(this)));
        }

        public void Dispose()
        {
            drawer?.Dispose();
        }

        public void Repaint()
        {
            MarkDirtyRepaint();
            drawer.Repaint();
        }
    }
}