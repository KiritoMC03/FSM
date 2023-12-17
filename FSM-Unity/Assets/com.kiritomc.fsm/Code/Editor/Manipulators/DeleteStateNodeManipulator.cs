using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class DeleteStateNodeManipulator<T> : Manipulator
        where T: Node
    {
        private readonly NodesContext<T> context;

        public DeleteStateNodeManipulator(NodesContext<T> context)
        {
            this.context = context;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<KeyDownEvent>(Delete);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<KeyDownEvent>(Delete);
        }

        private void Delete(KeyDownEvent e)
        {
            if (e.keyCode == Keys.DeleteNode)
            {
                foreach (T selectedNode in context.SelectedNodes.ToArray()) 
                    context.Remove(selectedNode);
                context.SelectedNodes.Clear();
            }
        }
    }
}