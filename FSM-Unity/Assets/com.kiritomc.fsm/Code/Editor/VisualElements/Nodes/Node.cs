using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class Node : VisualElement, ICustomRepaintHandler
    {
        protected Label Label;
        protected VisualElement Connection;

        protected Node(string nodeName)
        {
            VisualElement header = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            header.Add(Label = new Label(nodeName)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            });
            header.Add(Connection = NodeConnectionPoint.Create());
            
            Add(header);
            ApplyStyle();
        }

        public virtual void Repaint()
        {
        }

        public virtual Vector2 GetAbsoluteConnectionPos()
        {
            return new Vector2(Connection.worldBound.x + Sizes.ConnectionNodePoint / 2f, Connection.worldBound.y + Sizes.ConnectionNodePoint / 2f);
        }

        private void ApplyStyle()
        {
            style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = 8;
            style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = Colors.NodeBorderColor;
            style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = Sizes.NodeBorderWidth;
            style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = 8;
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.minWidth = 200;
            style.minHeight = 50;
            style.backgroundColor = Colors.NodeBackground;
        }
    }
}