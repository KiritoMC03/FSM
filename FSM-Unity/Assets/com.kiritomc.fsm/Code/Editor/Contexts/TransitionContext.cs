using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : Context
    {
        private readonly StateTransition target;
        private readonly EditorState editorState;
        private readonly Fabric fabric;
        private readonly List<ConditionalNode> nodes = new List<ConditionalNode>();

        public TransitionContext(StateTransition target, EditorState editorState, Fabric fabric)
        {
            this.target = target;
            this.editorState = editorState;
            this.fabric = fabric;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            // this.AddManipulator(new CreateNodeManipulator(fabric));
        }
    }
}