using System;
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
            target.Label.RegisterCallback<PointerUpEvent>(HandleClick);
            target.LabelInputField.RegisterCallback<KeyUpEvent>(HandleKeys);
            target.LabelInputField.RegisterCallback<FocusOutEvent>(UnFocusForSubscription);
            target.LabelInputField.RegisterValueChangedCallback(HandleTextInput);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
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
            if (keyUpEvent.keyCode == Keys.ApplyNodeRename)
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
                target.Name = prevValue;
                target.LabelInputField.value = prevValue;
            }
            target.Label.style.display = DisplayStyle.Flex; 
            target.LabelInputField.style.display = DisplayStyle.None;
            draggingLocked.Value = false;
        }

        private async void HandleClick(PointerUpEvent pointerUpEvent)
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