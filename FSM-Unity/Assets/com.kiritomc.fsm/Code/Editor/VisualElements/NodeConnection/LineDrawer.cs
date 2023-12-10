using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class LineDrawer : VisualElement
    {
        public Vector2? LocalStartOffset;
        public Vector2? WorldEndPos;

        public LineDrawer()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            if (!LocalStartOffset.HasValue || !WorldEndPos.HasValue) return;
            Vector2 start = LocalStartOffset.Value;
            Vector2 end = WorldEndPos.Value - new Vector2(worldBound.x, worldBound.y);
            Painter2D paint2D = ctx.painter2D;
            Vector2 startTangent;
            Vector2 endTangent;
            // Vector2 offset = end - StartPos;
        
            // if (Mathf.Abs(offset.x) < Mathf.Abs(offset.y))
            {
                startTangent = new Vector2(start.x + (end.x - start.x) / 2f, start.y);
                endTangent = new Vector2(end.x - (end.x - start.x) / 2f, end.y);
            }
            // else
            // {
            //     startTangent = new Vector2(StartPos.x, (end.y - StartPos.y) / 2f);
            //     endTangent = new Vector2(end.x, (end.y - StartPos.y) / 2f);
            // }
            paint2D.lineWidth = 8.0f;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.strokeGradient = Colors.NodeConnectionGradient;
            paint2D.BeginPath();
            // paint2D.Arc(startTangent, 10, 180, -180);
            // paint2D.Arc(endTangent, 10, 180, -180);
            paint2D.MoveTo(start);
            paint2D.BezierCurveTo(startTangent, endTangent, end);
            paint2D.Stroke();
        }
    }
}