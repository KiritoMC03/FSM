using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LineDrawer : VisualElement
    {
        public Vector2 StartPos;
        public Vector2 EndPos;

        public LineDrawer()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            Vector2 end = EndPos - new Vector2(worldBound.x, worldBound.y);
            Painter2D paint2D = ctx.painter2D;
            Vector2 startTangent = new Vector2((end.x - StartPos.x) / 2f, StartPos.y);
            Vector2 endTangent = new Vector2((end.x - StartPos.x) / 2f, end.y);
            paint2D.lineWidth = 8.0f;
            paint2D.strokeColor = Colors.NodeConnectionColor;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.BeginPath();
            paint2D.MoveTo(StartPos);
            paint2D.BezierCurveTo(startTangent, endTangent, end);
            paint2D.Stroke();
        }
    }
}