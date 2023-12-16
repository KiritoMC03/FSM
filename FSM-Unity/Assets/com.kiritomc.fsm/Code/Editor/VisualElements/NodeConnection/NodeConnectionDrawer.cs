using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeConnectionDrawer : VisualElement
    {
        private const int MaxPoints = 100;
        private const float LengthForMaxPoints = 1000;
        private const float PointClickTrackRadius = 6;
        public Vector2? LocalStartOffset;
        public Vector2? WorldEndPos;
        private Vector2[] points = new Vector2[MaxPoints];
        private int currentPointsNumber;

        public Vector2? LocalEndPos => WorldEndPos.HasValue ? WorldEndPos.Value - new Vector2(worldBound.x, worldBound.y) : null;

        public NodeConnectionDrawer()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            if (base.ContainsPoint(localPoint)) return true;
            for (int i = 0; i < currentPointsNumber - 1; i++)
                if (Vector2.Distance(points[i], localPoint) < PointClickTrackRadius) 
                    return true;
            return false;
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

            currentPointsNumber = Mathf.Clamp(Mathf.RoundToInt(Vector2.Distance(end, start) / LengthForMaxPoints * MaxPoints), 0, MaxPoints);
            DrawingUtils.CalculateBezierCurve(start, startTangent, endTangent, end, points, currentPointsNumber);
            // for (int i = 1; i < currentPointsNumber; i++)
            // {
            //     paint2D.BeginPath();
            //     paint2D.MoveTo(points[i-1]);
            //     paint2D.LineTo(points[i]);
            //     paint2D.strokeColor = i % 2 ==0 ? Color.black : Color.white;
            //     paint2D.Stroke();
            // }
        }
    }
}