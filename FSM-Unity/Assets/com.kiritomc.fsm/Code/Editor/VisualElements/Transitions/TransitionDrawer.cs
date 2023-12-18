using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionDrawer : VisualElement, IDisposable, ICustomRepaintHandler
    {
        private const int BezierCurveMaxPoints = 100;
        private const float LengthForMaxPoints = 1000;
        private const float MinimalTangentDistance = 6;

        private readonly VisualStateNode source;
        private readonly VisualStateNode target;
        private readonly Button editButton;
        private readonly Vector2[] points = new Vector2[BezierCurveMaxPoints];
        private int currentPointsNumber;

        public TransitionDrawer(VisualStateNode source, VisualStateNode target, Action onEditClicked)
        {
            this.source = source;
            this.target = target;
            generateVisualContent += OnGenerateVisualContent;

            Add(editButton = new Button(onEditClicked)
            {
                text = "Edit",
                style =
                {
                    position = Position.Absolute,
                },
            });
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
            Vector2 initialEnd = GetTargetPoint();
            Vector2 bezierEnd = new Vector2(initialEnd.x, initialEnd.y + (TargetCenterLocal.y < 0 ? 10 : -10));
            float isTooCloseMult = target.worldBound.height / 2f > Mathf.Abs(TargetCenterLocal.y) ? 1 : -1;
            float startAddTangent = (MinimalTangentDistance + source.resolvedStyle.width / 2f) * Mathf.Sign(start.x);
            float endAddTangent = (MinimalTangentDistance + target.resolvedStyle.height / 2f) * Mathf.Sign(initialEnd.y) * isTooCloseMult;
            Painter2D paint2D = ctx.painter2D;
            Vector2 startTangent = new Vector2(start.x + (initialEnd.x - start.x) / 2f + startAddTangent, start.y);
            Vector2 endTangent = new Vector2(initialEnd.x, initialEnd.y + (initialEnd.y - start.y) * isTooCloseMult / 2f + endAddTangent);
            paint2D.lineWidth = 8.0f;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.strokeGradient = Colors.NodeConnectionGradient;
            paint2D.BeginPath();
            paint2D.MoveTo(start);
            paint2D.BezierCurveTo(startTangent, endTangent, bezierEnd);
            paint2D.Stroke();

            int sign = TargetCenterLocal.y < 0 ? 1 : -1;
            paint2D.BeginPath();
            paint2D.MoveTo(initialEnd);
            paint2D.LineTo(initialEnd + new Vector2(sideLength / 2f, sideLength) * sign);
            paint2D.LineTo(initialEnd + new Vector2(-sideLength / 2f, sideLength) * sign);
            paint2D.LineTo(initialEnd);
            paint2D.fillColor = Color.yellow;
            paint2D.Fill();
            
            

            currentPointsNumber = Mathf.Clamp(Mathf.RoundToInt(Vector2.Distance(bezierEnd, start) / LengthForMaxPoints * BezierCurveMaxPoints), 3, BezierCurveMaxPoints);
            DrawingUtils.CalculateBezierCurve(start, startTangent, endTangent, bezierEnd, points, currentPointsNumber);
            UpdateEditButtonPosition();
            // for (int i = 1; i < currentPointsNumber; i++)
            // {
            //     paint2D.BeginPath();
            //     paint2D.MoveTo(points[i-1]);
            //     paint2D.LineTo(points[i]);
            //     paint2D.strokeColor = i % 2 ==0 ? Color.black : Color.white;
            //     paint2D.Stroke();
            // }
        }

        private void UpdateEditButtonPosition()
        {
            Vector2 centerPoint = points[currentPointsNumber / 2];
            editButton.style.top = centerPoint.y - editButton.resolvedStyle.height / 2f;
            editButton.style.left = centerPoint.x - editButton.resolvedStyle.width / 2f;
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