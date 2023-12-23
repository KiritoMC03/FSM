using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NavigationPanel : VisualElement
    {
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();
        private readonly List<Button> currentHierarchy = new List<Button>();
        private Button root;

        public NavigationPanel()
        {
            style.backgroundColor = Colors.NavigationPanelBackground;
            style.width = new StyleLength(new Length(Sizes.NavigationPanelWidth, Sizes.NavigationPanelWidthUnits));
            style.height = new StyleLength(new Length(Sizes.NavigationPanelHeight, Sizes.NavigationPanelHeightUnits));
            Add(root = CreateButton("Root", default));
            EditorState.CurrentContext.ValueChanged += Redraw;
        }

        private void Redraw(VisualNodesContext currentContext)
        {
            while (currentHierarchy.Count > 0)
            {
                Button current = currentHierarchy[0];
                Remove(current);
                currentHierarchy.RemoveAt(0);
            }

            if (currentContext is StatesContext { Name: "Root" }) return;
            Button button;
            currentHierarchy.Add(button = new Button()
            {
                text = currentContext switch
                {
                    StatesContext statesContext => statesContext.Name,
                    TransitionContext transitionContext => transitionContext.Name,
                    _ => "Unknown context",
                },
            });
            Add(button);
        }

        private Button CreateButton(string text, VisualStateNode stateNode)
        {
            return new Button(() => MoveToContext(stateNode))
            {
                text = text,
            };
        }

        private void MoveToContext(VisualStateNode stateNode)
        {
            if (stateNode == null) Fabric.Contexts.CreateRootContext();
            else Fabric.Contexts.CreateStateContext(stateNode);
        }
    }
}