using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class OpenStateContextManipulator : PointerManipulator
    {
        private readonly VisualStateNode stateNode;

        public OpenStateContextManipulator(VisualStateNode stateNode)
        {
            this.stateNode = stateNode;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            stateNode.RegisterCallback<ClickEvent>(HandleClicked);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            stateNode.UnregisterCallback<ClickEvent>(HandleClicked);
        }

        private void HandleClicked(ClickEvent e)
        {
            if (e.clickCount == 2 && e.button == 0)
            {
                stateNode.Context.Open();
            }
        }
    }
}