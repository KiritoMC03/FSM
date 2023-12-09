using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LineDrawerRegistration : IDisposable, ICustomRepaintHandler
    {
        private LineDrawer drawer;
        private VisualElement parent;
        private Func<Vector2?> startGetter;
        private Func<Vector2?> endGetter;

        public LineDrawerRegistration(LineDrawer drawer, VisualElement parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
        {
            this.drawer = drawer;
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
                parent.Remove(drawer);
                parent = default;
            }
            drawer = default;
            startGetter = endGetter = default;
        }

        private void Redraw(MeshGenerationContext meshGenerationContext)
        {
            drawer.style.position = Position.Absolute;
            Vector2? start = startGetter.Invoke();
            if (start == null) return;
            drawer.style.top = start.Value.x;
            drawer.style.left = start.Value.y;
            Vector2? end = endGetter.Invoke();
            if (end != null)
            {
                drawer.EndPos = end.Value;
                drawer.StartPos = new Vector2(0f, 0f);
            }
        }

        public void Repaint()
        {
            drawer.MarkDirtyRepaint();
        }
    }
}