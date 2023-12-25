using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class Colors
    {
        public static Color NodeBackground => new Color(0.15f, 0.15f, 0.15f);
        public static Color NodeConnectionBackground => new Color(0.1f, 0.1f, 0.1f);
        public static Color NodeConnectionColor => new Color(1, 0.84f, 0f);
        public static Color SelectedNodeBorderColor => new Color(0.1f, 0.5f, 0.7f);
        public static Color NodeBorderColor => new Color(0.5f, 0.5f, 0.5f);
        public static Color ContextBackground => new Color(0.11f, 0.11f, 0.11f);

        public static Color CreateNodePopupBackground => new Color(0.3f, 0.3f, 0.3f);
        public static Color CreateNodePopupItemBackground => new Color(0.25f, 0.25f, 0.25f);

        public static Color LeftPanelBorder => new Color(0.5f, 0.5f, 0.5f);
        public static Color NavigationPanelBackground => new Color(0.15f, 0.15f, 0.15f);

        public static Gradient NodeLinkGradient { get; } = new Gradient()
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

        public static Gradient NodeLinkErrorGradient { get; } = new Gradient()
        {
            colorKeys = new[]
            {
                new GradientColorKey(new Color(0.65f, 0.16f, 0.16f), 0),
                new GradientColorKey(new Color(0.88f, 0.18f, 0.07f), 1),
            },
            alphaKeys = new []
            {
                new GradientAlphaKey(0.65f, 0),
                new GradientAlphaKey(1f, 1),
            },
        };
    }
}