using System;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualNodeLinkDrawerRegistration : ICancelable, ICustomRepaintHandler
    {
        private readonly VisualElement parent;
        private readonly Func<Vector2?> localStartGetter;
        private readonly Func<Vector2?> worldEndGetter;

        public NodeLinkDrawer Drawer { get; private set; }
        public bool IsDisposed { get; private set; }

        public VisualNodeLinkDrawerRegistration(VisualElement parent, Func<Vector2?> localStartGetter, Func<Vector2?> worldEndGetter)
        {
            this.parent = parent;
            this.localStartGetter = localStartGetter;
            this.worldEndGetter = worldEndGetter;
            this.parent.Add(Drawer = new NodeLinkDrawer());
            parent.generateVisualContent += Redraw;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.generateVisualContent -= Redraw;
            parent.Remove(Drawer);
            IsDisposed = true;
        }

        public void Clear()
        {
            Drawer.WorldEndPos = Drawer.LocalStartOffset = default;
            Repaint();
        }
        private void Redraw() => Redraw(default);
        private void Redraw(MeshGenerationContext meshGenerationContext)
        {
            Vector2? start = localStartGetter.Invoke();
            Vector2? end = worldEndGetter.Invoke();
            if (start.HasValue) Drawer.LocalStartOffset = start.Value;
            if (end.HasValue) Drawer.WorldEndPos = end.Value;
        }

        public void Repaint()
        {
            Redraw();
            Drawer.MarkDirtyRepaint();
        }
    }
}