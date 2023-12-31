using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionDrawer : VisualElement, IDisposable, ICustomRepaintHandler
    {
        private const int BezierCurveMaxPoints = 160;
        private const float LengthForMaxPoints = 1000;
        private const float MinimalTangentDistance = 6;
        private const float TangentMultiplier = 0.4f;

        public readonly Vector2[] Points = new Vector2[BezierCurveMaxPoints];
        private readonly VisualStateNode source;
        private readonly VisualStateNode target;
        private int currentPointsNumber;

        public TransitionDrawer(VisualStateNode source, VisualStateNode target, Action onEditClicked)
        {
            this.source = source;
            this.target = target;
            generateVisualContent += OnGenerateVisualContent;
        }

        public void Dispose()
        {
            generateVisualContent -= OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            const float sideLength = 20f;
            if (source == null || target == null) return;
            Vector2 start = GetStartPoint();
            Vector2 end = GetTargetPoint();
            Vector2 bezierEnd = new Vector2(end.x, end.y + (TargetCenterLocal.y < 0 ? 10 : -10));
            float isTooCloseMult = target.worldBound.height / 2f > Mathf.Abs(TargetCenterLocal.y) ? 1 : -1;
            float startAddTangent = (MinimalTangentDistance + source.resolvedStyle.width / 2f) * Mathf.Sign(start.x) * TangentMultiplier;
            float endAddTangent = (MinimalTangentDistance + target.resolvedStyle.height / 2f) * Mathf.Sign(end.y) * isTooCloseMult * TangentMultiplier;

            Vector2 startOffset = new Vector2(start.x + Sizes.NodeTransitionOffset * Mathf.Sign(startAddTangent), start.y);
            Vector2 endOffset = new Vector2(bezierEnd.x, bezierEnd.y + Sizes.NodeTransitionOffset * Mathf.Sign(endAddTangent));
            Vector2 startTangent = new Vector2(startOffset.x + (endOffset.x - startOffset.x) / 2f + startAddTangent, startOffset.y);
            Vector2 endTangent = new Vector2(endOffset.x, bezierEnd.y + (bezierEnd.y - startOffset.y) * isTooCloseMult / 2f + endAddTangent);

            Painter2D paint2D = ctx.painter2D;
            paint2D.lineWidth = 8.0f;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.strokeGradient = Colors.NodeLinkGradient;
            paint2D.BeginPath();
            paint2D.MoveTo(start);
            paint2D.LineTo(startOffset);
            paint2D.BezierCurveTo(startTangent, endTangent, endOffset);
            paint2D.LineTo(bezierEnd);
            paint2D.Stroke();

            int sign = TargetCenterLocal.y < 0 ? 1 : -1;
            paint2D.BeginPath();
            paint2D.MoveTo(end);
            paint2D.LineTo(end + new Vector2(sideLength / 2f, sideLength) * sign);
            paint2D.LineTo(end + new Vector2(-sideLength / 2f, sideLength) * sign);
            paint2D.LineTo(end);
            paint2D.fillColor = Color.yellow;
            paint2D.Fill();
            
            

            currentPointsNumber = Mathf.Clamp(Mathf.RoundToInt(Vector2.Distance(bezierEnd, start) / LengthForMaxPoints * BezierCurveMaxPoints), 3, BezierCurveMaxPoints);
            DrawingUtils.CalculateBezierCurve(start, startTangent, endTangent, bezierEnd, Points, currentPointsNumber);
            // for (int i = 1; i < currentPointsNumber; i++)
            // {
            //     paint2D.BeginPath();
            //     paint2D.MoveTo(points[i-1]);
            //     paint2D.LineTo(points[i]);
            //     paint2D.strokeColor = i % 2 ==0 ? Color.black : Color.white;
            //     paint2D.Stroke();
            // }
        }

        private Vector2 TargetCenterLocal => source.WorldToLocal(target.worldBound).min;

        private Vector2 GetStartPoint()
        {
            Vector2 result = new Vector2(source.resolvedStyle.width / 2f, 0f);
            if (TargetCenterLocal.x < 0) result.x *= -1;
            return result;
        }
        
        private Vector2 GetTargetPoint()
        {
            Vector2 targetCenter = TargetCenterLocal;
            float halfHeight = target.worldBound.height / 2f;
            if (TargetCenterLocal.y > 0) halfHeight *= -1;
            return new Vector2(targetCenter.x, targetCenter.y + halfHeight);
        }

        public void Repaint()
        {
            MarkDirtyRepaint();
        }
    }
}