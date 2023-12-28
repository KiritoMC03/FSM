using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class SaveWorldPointerPositionManipulator : Manipulator
    {
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerMoveEvent>(SavePosition);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerMoveEvent>(SavePosition);
        }

        private void SavePosition(PointerMoveEvent e)
        {
            EditorState.PointerPosition.Value = target.LocalToWorld(e.position);
        }
    }
}