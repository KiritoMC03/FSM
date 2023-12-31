using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeLinkDrawer : VisualElement
    {
        private const int MaxPoints = 100;
        private const float LengthForMaxPoints = 1000;
        public Gradient OverrideGradient;
        public Vector2? LocalStartOffset;
        public Vector2? WorldEndPos;
        private Vector2[] points = new Vector2[MaxPoints];
        private int currentPointsNumber;

        public Vector2? LocalEndPos => WorldEndPos.HasValue ? WorldEndPos.Value - new Vector2(worldBound.x, worldBound.y) : null;

        public NodeLinkDrawer()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            if (!LocalStartOffset.HasValue || !WorldEndPos.HasValue) return;
            Vector2 start = LocalStartOffset.Value;
            Vector2 end = (WorldEndPos.Value - new Vector2(worldTransform.GetPosition().x, worldTransform.GetPosition().y)) / worldTransform.lossyScale.x;
            Painter2D paint2D = ctx.painter2D;

            Vector2 startOffset = new Vector2(start.x - Sizes.NodeLinkLinkOffset, start.y);
            Vector2 endOffset = new Vector2(end.x + Sizes.NodeLinkLinkOffset, end.y);
            Vector2 startTangent = new Vector2(startOffset.x - Sizes.NodeLinkLinkOffset, startOffset.y);
            Vector2 endTangent = new Vector2(endOffset.x + Sizes.NodeLinkLinkOffset, endOffset.y);

            paint2D.lineWidth = 8.0f;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.strokeGradient = OverrideGradient ?? Colors.NodeLinkGradient;
            paint2D.BeginPath();
            paint2D.MoveTo(start);
            paint2D.LineTo(startOffset);
            paint2D.BezierCurveTo(startTangent, endTangent, endOffset);
            paint2D.LineTo(end);
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