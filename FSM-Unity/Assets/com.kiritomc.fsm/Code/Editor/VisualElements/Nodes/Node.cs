using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class Node : VisualElement
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
            const int size = 15;
            header.Add(Connection = new Box()
            {
                style =
                {
                    height = size,
                    minWidth = size,
                    borderTopLeftRadius = size,
                    borderTopRightRadius = size,
                    borderBottomLeftRadius = size,
                    borderBottomRightRadius = size,
                    backgroundColor = Colors.NodeConnectionBackground,
                },
            });
            
            Add(header);

            style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = 8;
            style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = 8;
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.minWidth = 200;
            style.minHeight = 50;
            style.backgroundColor = Colors.NodeBackground;
        }

        public void ApplyStyle()
        {
            // style.minHeight = 50 + NodeConnectionsNumber * 5;
        }
    }

    public abstract class ConditionalNode : Node
    {
        protected ConditionalNode(string nodeName) : base(nodeName)
        {
        }
        protected ConditionalNode(IConditionalLayoutNode node) : base(node.GetType().Name)
        {
        }
    }

    public class ConditionNode : ConditionalNode
    {
        public ConditionNode(ConditionLayoutNode node) : base(node.LogicObject.GetType().Name)
        {
        }
    }

    public class NotNode : ConditionalNode
    {
        public ConditionalNode Input;

        public NotNode(NotLayoutNode node) : base(node)
        {
            Add(new NodeConnection($"{nameof(Input)}"));
        }
    }

    public class OrNode : ConditionalNode
    {
        public ConditionalNode Left;
        public ConditionalNode Right;

        public OrNode(OrLayoutNode node) : base(node)
        {
            Add(new NodeConnection($"{nameof(Left)}"));
            Add(new NodeConnection($"{nameof(Right)}"));
        }
    }

    public class AndNode : ConditionalNode
    {
        public ConditionalNode Left;
        public ConditionalNode Right;

        public AndNode(AndLayoutNode node) : base(node)
        {
            Add(new NodeConnection($"{nameof(Left)}"));
            Add(new NodeConnection($"{nameof(Right)}"));
        }
    }

    public class NodeConnection : VisualElement
    {
        public NodeConnection(string connectionName)
        { 
            const int size = 15;
            const int horizontalMargin = 10;
            const int verticalMargin = 5;
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.marginTop = style.marginBottom = verticalMargin;
            style.marginLeft = style.marginRight = horizontalMargin;
            Add(new Box()
            {
                style =
                {
                    height = size,
                    width = size,
                    borderTopLeftRadius = size,
                    borderTopRightRadius = size,
                    borderBottomLeftRadius = size,
                    borderBottomRightRadius = size,
                    backgroundColor = Colors.NodeConnectionBackground,
                },
            });

            Add(new Label(connectionName)
            {
                style =
                {
                    marginLeft = horizontalMargin,
                    marginRight = horizontalMargin,
                },
            });
        }
    }
}