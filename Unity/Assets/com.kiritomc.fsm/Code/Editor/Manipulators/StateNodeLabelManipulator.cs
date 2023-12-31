﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class StateNodeLabelManipulator : Manipulator
    {
        private new readonly VisualStateNode target;
        private readonly Func<ChangeEvent<string>, string> validateNameDelegate;
        private string prevValue;
        private string currentValue;
        private bool isEditing;
        private Vector3 touchPosition;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public StateNodeLabelManipulator(VisualStateNode target, Func<ChangeEvent<string>, string> validateNameDelegate)
        {
            this.target = target;
            this.validateNameDelegate = validateNameDelegate;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.Label.RegisterCallback<PointerDownEvent>(HandlePress);
            target.Label.RegisterCallback<PointerUpEvent>(HandleClick);
            target.LabelInputField.RegisterCallback<KeyUpEvent>(HandleKeys);
            target.LabelInputField.RegisterCallback<FocusOutEvent>(UnFocusForSubscription);
            target.LabelInputField.RegisterValueChangedCallback(HandleTextInput);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.Label.UnregisterCallback<PointerDownEvent>(HandlePress);
            target.Label.UnregisterCallback<PointerUpEvent>(HandleClick);
            target.LabelInputField.UnregisterCallback<KeyUpEvent>(HandleKeys);
            target.LabelInputField.UnregisterCallback<FocusOutEvent>(UnFocusForSubscription);
            target.LabelInputField.UnregisterValueChangedCallback(HandleTextInput);
        }

        private void HandleTextInput(ChangeEvent<string> changeEvent)
        {
            currentValue = validateNameDelegate.Invoke(changeEvent);
        }

        private void HandleKeys(KeyUpEvent keyUpEvent)
        {
            if (Keys.ApplyNodeRename.Contains(keyUpEvent.keyCode))
            {
                target.Label.text = currentValue;
                target.Name = currentValue;
                UnFocus(false);
            }
            else if (keyUpEvent.keyCode == KeyCode.Escape)
            {
                UnFocus();
            }
        }

        private async void UnFocusForSubscription(EventBase e)
        {
            await Task.Delay(1000);
            UnFocus();
        }

        private void UnFocus(bool resetValue = true)
        {
            if (!isEditing) return;
            isEditing = false;
            if (resetValue)
            {
                target.Label.text = prevValue;
                target.Name = prevValue;
                target.LabelInputField.value = prevValue;
            }
            target.Label.style.display = DisplayStyle.Flex; 
            target.LabelInputField.style.display = DisplayStyle.None;
            EditorState.DraggingLocked.Value = false;
        }

        private void HandlePress(PointerDownEvent pointerDownEvent)
        {
            touchPosition = pointerDownEvent.position;
        }

        private async void HandleClick(PointerUpEvent pointerUpEvent)
        {
            if (!pointerUpEvent.position.Approximately(touchPosition)) return;
            isEditing = true;
            currentValue = prevValue = target.Label.text;
            target.Label.style.display = DisplayStyle.None; 
            target.LabelInputField.style.display = DisplayStyle.Flex;
            EditorState.DraggingLocked.Value = true;
            await Task.Yield();
            target.LabelInputField.Focus();
        }
    }
}