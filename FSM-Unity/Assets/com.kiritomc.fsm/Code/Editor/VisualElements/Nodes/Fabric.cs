using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class Fabric
    {
        private readonly EditorState editorState;
        private VisualElement root;

        public Fabric(EditorState editorState, VisualElement root)
        {
            this.editorState = editorState;
            this.root = root;
        }
        
        public OrNode TestConditional(VisualElement pointerTrackingElement)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(editorState, pointerTrackingElement));
            return node;
        }

        public SelectNodePopup CreateSelectNodePopup(IEnumerable<string> availableNodes, Action<string> selectedHandler)
        {
            SelectNodePopup result = new SelectNodePopup(availableNodes, selectedHandler);
            root.Add(result);
            Vector2 position = result.WorldToLocal(editorState.PointerPosition.Value);
            result.style.position = Position.Absolute;
            result.style.left = position.x;
            result.style.top = position.y;
            return result;
        }

        public StateNode CreateStateNode(string name, VisualElement pointerTrackingElement)
        {
            StateNode node = new StateNode(name);
            node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            node.AddManipulator(new CreateTransitionManipulator(editorState, pointerTrackingElement));
            return node;
        }
    }
}