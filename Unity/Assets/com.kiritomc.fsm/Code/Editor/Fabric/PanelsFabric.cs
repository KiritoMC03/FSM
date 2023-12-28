using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class PanelsFabric
    {
        private readonly VisualElement root;

        public PanelsFabric(VisualElement root)
        {
            this.root = root;
        }

        public LeftPanel LeftPanel()
        {
            LeftPanel result = new LeftPanel();
            root.Add(result);
            return result;
        }

        public NavigationPanel NavigationPanel()
        {
            NavigationPanel result = new NavigationPanel();
            root.Add(result);
            return result;
        }
    }
}