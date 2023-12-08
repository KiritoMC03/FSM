using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NotNode : ConditionalNode
    {
        public ConditionalNode Input;
        private readonly LineDrawer drawer;

        public NotNode(NotLayoutNode node) : base(node)
        {
            NodeConnectionField con;
            Add(con = NodeConnectionField.Create($"{nameof(Input)}"));
            Add(drawer = new LineDrawer());
            generateVisualContent += _ =>
            {
                drawer.style.position = Position.Absolute;
                drawer.style.top = con.resolvedStyle.top;
                drawer.style.left = con.resolvedStyle.left;
                if (Input != null)
                {
                    drawer.EndPos = Input.GetAbsoluteConnectionPos();
                    drawer.StartPos = new Vector2(15 / 2f, 15 / 2f);
                }
            };
        }

        public override void Repaint()
        {
            drawer.MarkDirtyRepaint();
        }
    }
}