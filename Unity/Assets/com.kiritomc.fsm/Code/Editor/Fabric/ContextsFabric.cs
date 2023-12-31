using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class ContextsFabric
    {
        private readonly VisualElement root;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public ContextsFabric(VisualElement root)
        {
            this.root = root;
        }

        public StatesContext OpenRootContext()
        {
            VisualNodesContext current = EditorState.CurrentContext.Value;
            current?.parent.Remove(current);
            EditorState.RootContext.Value ??= new StatesContext("Root");
            EditorState.RootContext.Value.style.display = DisplayStyle.Flex;
            EditorState.CurrentContext.Value = EditorState.RootContext.Value;
            root.Add(EditorState.RootContext.Value);
            return EditorState.RootContext.Value;
        }

        public void OpenTransitionContext(TransitionContext transitionContext)
        {
        }
    }
}