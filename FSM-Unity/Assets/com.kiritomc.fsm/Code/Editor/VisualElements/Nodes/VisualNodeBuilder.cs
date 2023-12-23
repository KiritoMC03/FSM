using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class VisualNodeBuilder
    {
        public static VisualElement NodeHeader<T>(this T node)
            where T : VisualNode
        {
            VisualElement header = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            node.Add(header);
            return header;
        }


        public static Label NodeLabel<T>(this T nodeWithHeader, string text) where T : VisualNode
        {
            return nodeWithHeader.Header.NodeLabel(text);
        }

        public static Label NodeLabel(this VisualElement parent, string text)
        {
            Label label;
            parent.Add(label = new Label(text)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            });
            return label;
        }


        public static TextInputBaseField<string> NodeInputLabel<T>(this T nodeWithHeader, string text) where T : VisualNode
        {
            return nodeWithHeader.Header.NodeInputLabel(text);
        }

        public static TextInputBaseField<string> NodeInputLabel(this VisualElement parent, string text)
        {
            TextField field = new TextField()
            {
                value = text,
                style =
                {
                    alignContent = Align.Center,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            };
            parent.Add(field);
            return field;
        }
    }
}