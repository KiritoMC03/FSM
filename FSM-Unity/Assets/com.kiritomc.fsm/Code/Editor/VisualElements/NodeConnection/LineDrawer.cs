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
            Vector2 startTangent;
            Vector2 endTangent;
            // Vector2 offset = end - StartPos;
        
            // if (Mathf.Abs(offset.x) < Mathf.Abs(offset.y))
            {
                startTangent = new Vector2(StartPos.x + (end.x - StartPos.x) / 2f, StartPos.y);
                endTangent = new Vector2(end.x - (end.x - StartPos.x) / 2f, end.y);
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
            paint2D.Arc(startTangent, 10, 180, -180);
            paint2D.Arc(endTangent, 10, 180, -180);
            paint2D.MoveTo(StartPos);
            paint2D.BezierCurveTo(startTangent, endTangent, end);
            paint2D.Stroke();
        }
    }
}