using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class CreateNodeManipulator : Manipulator
    {
        private readonly NodesFabric fabric;

        public CreateNodeManipulator(NodesFabric fabric)
        {
            this.fabric = fabric;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<KeyUpEvent>(HandleKeyUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<KeyUpEvent>(HandleKeyUp);
        }

        private void HandleKeyUp(KeyUpEvent e)
        {
            if (e.keyCode == Keys.CreateNode)
            {
                target.Add(fabric.TestConditional(target));
            }
        }
    }
}