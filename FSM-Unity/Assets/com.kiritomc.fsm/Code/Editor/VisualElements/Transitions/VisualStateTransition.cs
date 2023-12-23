using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualStateTransition : VisualElement, IDisposable, ICustomRepaintHandler
    {
        public readonly VisualStateNode Source;
        public readonly VisualStateNode Target;
        public readonly TransitionContext Context;
        private TransitionDrawer drawer;

        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public VisualStateTransition(VisualStateNode source, VisualStateNode target)
        {
            Source = source;
            Target = target;
            style.position = Position.Absolute;
            style.top = 0;
            style.bottom = 0;
            style.alignSelf = Align.Center;
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.Center;
            Context = new TransitionContext(this, $"{source.Name} -> {target.Name}");
            Add(drawer = new TransitionDrawer(source, target, () => Fabric.Contexts.OpenTransitionContext(Context)));
            target.OnChanged(Repaint);
            
            
            
            source.Add(transition);
            source.Disposables.Add(transition);
            source.Disposables.Add(target.OnChanged(RepaintTransition));
            source.ChildrenRepaintHandler.Add(transition);
            return transition;
            void RepaintTransition() => transition.Repaint();
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