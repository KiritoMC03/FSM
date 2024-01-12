using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualStateTransition : VisualElement, IDisposable, ICustomRepaintHandler
    {
        public readonly VisualStateNode Source;
        public readonly VisualStateNode Target;
        public readonly TransitionContext Context;
        private readonly TransitionDrawer drawer;
        private CompositeDisposable disposables = new CompositeDisposable();

        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public VisualStateTransition(VisualStateNode source, VisualStateNode target, Vector2 anchorNodePosition = default)
        {
            style.minWidth = new StyleLength(new Length(100, LengthUnit.Percent));
            Source = source;
            Target = target;
            style.position = Position.Absolute;
            style.alignSelf = Align.Center;
            style.flexDirection = FlexDirection.Row;
            style.justifyContent = Justify.Center;
            Context = new TransitionContext(this, $"{source.Name} -> {target.Name}", anchorNodePosition);
            Add(drawer = new TransitionDrawer(source, target, () => Context.Open()));
            target.OnChanged(Repaint).AddTo(disposables);
        }

        public IEnumerable<Vector2> IterateLinkWorldPoints()
        {
            return drawer.Points.Select(point => drawer.LocalToWorld(point));
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