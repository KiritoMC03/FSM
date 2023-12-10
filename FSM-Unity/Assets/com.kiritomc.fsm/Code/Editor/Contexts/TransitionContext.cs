using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : VisualElement
    {
        private readonly StateTransition target;
        private readonly EditorState editorState;
        private readonly NodesFabric fabric;
        private readonly List<ConditionalNode> nodes = new List<ConditionalNode>();

        public TransitionContext(StateTransition target, EditorState editorState, NodesFabric fabric)
        {
            this.target = target;
            this.editorState = editorState;
            this.fabric = fabric;
            focusable = true;
            style.alignSelf = Align.FlexEnd;
            style.width = new StyleLength(new Length(80f, LengthUnit.Percent));
            style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
            style.backgroundColor = Colors.ContextBackground;
            this.AddManipulator(new CreateNodeManipulator(fabric));
        }
    }
}