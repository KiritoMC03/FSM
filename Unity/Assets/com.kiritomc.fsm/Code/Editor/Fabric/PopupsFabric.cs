using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class PopupsFabric
    {
        private readonly VisualElement root;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public PopupsFabric(VisualElement root)
        {
            this.root = root;
        }

        public SelectNodePopup CreateSelectNodePopup(IEnumerable<string> availableNodes, Action<string> selectedHandler)
        {
            SelectNodePopup result = new SelectNodePopup(availableNodes, selectedHandler);
            root.Add(result);
            Vector2 position = result.WorldToLocal(EditorState.PointerPosition.Value);
            result.style.position = Position.Absolute;
            result.style.left = position.x;
            result.style.top = position.y;
            return result;
        }
    }
}