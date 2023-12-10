using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodesFabric
    {
        private readonly EditorState editorState;

        public NodesFabric(EditorState editorState)
        {
            this.editorState = editorState;
        }
        
        public OrNode TestConditional(VisualElement pointerTrackingElement)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(editorState, pointerTrackingElement));
            return node;
        }
    }
}