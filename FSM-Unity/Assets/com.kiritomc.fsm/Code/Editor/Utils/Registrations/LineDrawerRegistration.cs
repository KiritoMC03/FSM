using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LineDrawerRegistration : IDisposable, ICustomRepaintHandler
    {
        public NodeLinkDrawer Drawer { get; private set; }
        private VisualElement parent;
        private Func<Vector2?> startGetter;
        private Func<Vector2?> endGetter;

        public LineDrawerRegistration(NodeLinkDrawer drawer, VisualElement parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
        {
            this.Drawer = drawer;
            this.parent = parent;
            this.startGetter = startGetter;
            this.endGetter = endGetter;
            parent.generateVisualContent += Redraw;
        }

        public void Dispose()
        {
            if (parent != null)
            {
                parent.generateVisualContent -= Redraw;
                parent.Remove(Drawer);
                parent = default;
            }
            Drawer = default;
            startGetter = endGetter = default;
        }

        private void Redraw(MeshGenerationContext meshGenerationContext)
        {
            Vector2? start = startGetter.Invoke();
            if (start.HasValue) Drawer.LocalStartOffset = start.Value;
            Vector2? end = endGetter.Invoke();
            if (end.HasValue) Drawer.WorldEndPos = end.Value;
        }

        public void Repaint()
        {
            Drawer.MarkDirtyRepaint();
        }
    }
}