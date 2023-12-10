using UnityEngine;

namespace FSM.Editor
{
    public static class Colors
    {
        public static Color NodeBackground => new Color(0.15f, 0.15f, 0.15f);
        public static Color NodeConnectionBackground => new Color(0.1f, 0.1f, 0.1f);
        public static Color NodeConnectionColor => new Color(1, 0.84f, 0f);
        public static Color NodeBorderColor => new Color(0.5f, 0.5f, 0.5f);

        public static Gradient NodeConnectionGradient { get; } = new Gradient()
        {
            colorKeys = new[]
            {
                new GradientColorKey(new Color(0.99f, 0.96f, 0.9f), 0),
                new GradientColorKey(new Color(1, 0.84f, 0f), 1),
            },
            alphaKeys = new []
            {
                new GradientAlphaKey(0.65f, 0),
                new GradientAlphaKey(1f, 1),
            },
        };
    }
}