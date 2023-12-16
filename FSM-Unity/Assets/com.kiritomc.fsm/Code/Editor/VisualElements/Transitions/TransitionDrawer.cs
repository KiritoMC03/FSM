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

        private readonly StateNode source;
        private readonly StateNode target;
        private readonly Button editButton;
        private readonly Vector2[] points = new Vector2[BezierCurveMaxPoints];
        private int currentPointsNumber;

        public TransitionDrawer(StateNode source, StateNode target, Action onEditClicked)
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
            if (source == null || target == null) return;
            Vector2 start = GetStartPoint();
            Vector2 end = GetTargetPint();
            float isTooCloseMult = target.worldBound.height / 2f > Mathf.Abs(TargetCenterLocal.y) ? 1 : -1;
            float startAddTangent = 0;//(MinimalTangentDistance + source.resolvedStyle.width / 2f) * Mathf.Sign(start.x);
            float endAddTangent = 0;//(MinimalTangentDistance + target.resolvedStyle.height / 2f) * Mathf.Sign(end.y) * isTooCloseMult;
            Painter2D paint2D = ctx.painter2D;
            Vector2 startTangent = new Vector2(start.x + (end.x - start.x) / 2f + startAddTangent, start.y);
            Vector2 endTangent = new Vector2(end.x, end.y + (end.y - start.y) * isTooCloseMult / 2f + endAddTangent);
            paint2D.lineWidth = 8.0f;
            paint2D.lineCap = LineCap.Round;
            paint2D.lineJoin = LineJoin.Round;
            paint2D.strokeGradient = Colors.NodeConnectionGradient;
            paint2D.BeginPath();
            paint2D.MoveTo(start);
            paint2D.BezierCurveTo(startTangent, endTangent, end);
            paint2D.Stroke();

            currentPointsNumber = Mathf.Clamp(Mathf.RoundToInt(Vector2.Distance(end, start) / LengthForMaxPoints * BezierCurveMaxPoints), 3, BezierCurveMaxPoints);
            DrawingUtils.CalculateBezierCurve(start, startTangent, endTangent, end, points, currentPointsNumber);
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
        
        private Vector2 GetTargetPint()
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