using UnityEngine;

namespace FSM.Editor
{
    public static class DrawingUtils
    {
        public static void CalculateBezierCurve(Vector2 startPoint, Vector2 startTangent, Vector2 endTangent, Vector2 endPoint, Vector2[] buffer, int pointsNumber)
        {
            for (int i = 0; i <= pointsNumber-1; i++)
            {
                double t = i / (double)pointsNumber;
                double u = 1 - t;
                double tt = t * t;
                double uu = u * u;
                double uuu = uu * u;
                double ttt = tt * t;

                // Calculate blending functions
                double p0 = uuu;
                double p1 = 3 * uu * t;
                double p2 = 3 * u * tt;
                double p3 = ttt;

                // Calculate x and y coordinates of the Bezier curve point
                int x = (int)(startPoint.x * p0 + startTangent.x * p1 + endTangent.x * p2 + endPoint.x * p3);
                int y = (int)(startPoint.y * p0 + startTangent.y * p1 + endTangent.y * p2 + endPoint.y * p3);

                buffer[i] = new Vector2(x, y);
            }
        }
    }
}