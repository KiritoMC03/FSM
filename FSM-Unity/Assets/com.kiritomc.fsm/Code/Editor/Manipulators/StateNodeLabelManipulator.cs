﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class StateNodeLabelManipulator : Manipulator
    {
        private new readonly StateNode target;
        private readonly EditorStateProperty<bool> draggingLocked;
        private readonly Func<ChangeEvent<string>, string> validateNameDelegate;
        private string prevValue;
        private string currentValue;
        private bool isEditing;

        public StateNodeLabelManipulator(StateNode target, EditorStateProperty<bool> draggingLocked, Func<ChangeEvent<string>, string> validateNameDelegate)
        {
            this.target = target;
            this.draggingLocked = draggingLocked;
            this.validateNameDelegate = validateNameDelegate;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.Label.RegisterCallback<PointerDownEvent>(HandleClick);
            target.LabelInputField.RegisterCallback<KeyDownEvent>(HandleKeys);
            target.LabelInputField.RegisterCallback<FocusOutEvent>(UnFocusForSubscription);
            target.LabelInputField.RegisterValueChangedCallback(HandleTextInput);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.Label.UnregisterCallback<PointerDownEvent>(HandleClick);
            target.LabelInputField.UnregisterCallback<KeyDownEvent>(HandleKeys);
            target.LabelInputField.UnregisterCallback<FocusOutEvent>(UnFocusForSubscription);
            target.LabelInputField.UnregisterValueChangedCallback(HandleTextInput);
        }

        private void HandleTextInput(ChangeEvent<string> changeEvent)
        {
            currentValue = validateNameDelegate.Invoke(changeEvent);
        }

        private void HandleKeys(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.keyCode == Keys.ApplyNodeRename)
            {
                target.Label.text = currentValue;
                target.StateName = currentValue;
                UnFocus(false);
            }
            else if (keyDownEvent.keyCode == KeyCode.Escape)
            {
                UnFocus();
            }
        }

        private async void UnFocusForSubscription(EventBase e)
        {
            await Task.Yield();
            UnFocus();
        }

        private void UnFocus(bool resetValue = true)
        {
            if (!isEditing) return;
            isEditing = false;
            if (resetValue)
            {
                target.Label.text = prevValue;
                target.StateName = prevValue;
                target.LabelInputField.value = prevValue;
            }
            target.Label.style.display = DisplayStyle.Flex; 
            target.LabelInputField.style.display = DisplayStyle.None;
            draggingLocked.Value = false;
        }

        private async void HandleClick(PointerDownEvent pointerDownEvent)
        {
            isEditing = true;
            currentValue = prevValue = target.Label.text;
            target.Label.style.display = DisplayStyle.None; 
            target.LabelInputField.style.display = DisplayStyle.Flex;
            draggingLocked.Value = true;
            await Task.Yield();
            target.LabelInputField.Focus();
        }
    }
}