using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LineDrawerRegistration : IDisposable, ICustomRepaintHandler
    {
        public LineDrawer Drawer { get; private set; }
        private VisualElement parent;
        private Func<Vector2?> startGetter;
        private Func<Vector2?> endGetter;

        public LineDrawerRegistration(LineDrawer drawer, VisualElement parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
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
            Drawer.style.position = Position.Absolute;
            Vector2? start = startGetter.Invoke();
            if (start == null) return;
            Drawer.style.left = start.Value.x;
            Drawer.style.top = start.Value.y;
            Vector2? end = endGetter.Invoke();
            if (end != null)
            {
                Drawer.EndPos = end.Value;
                Drawer.StartPos = new Vector2(0f, 0f);
            }
        }

        public void Repaint()
        {
            Drawer.MarkDirtyRepaint();
        }
    }
}