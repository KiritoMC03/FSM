using System.Collections.Generic;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NavigationPanel : VisualElement
    {
        private readonly List<VisualElement> currentHierarchy = new List<VisualElement>();
        private CompositeDisposable disposables = new CompositeDisposable();
        private VisualElement root;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public NavigationPanel()
        {
            style.backgroundColor = Colors.NavigationPanelBackground;
            style.width = new StyleLength(new Length(Sizes.NavigationPanelWidth, Sizes.NavigationPanelWidthUnits));
            style.height = new StyleLength(new Length(Sizes.NavigationPanelHeight, Sizes.NavigationPanelHeightUnits));
            style.paddingTop = style.paddingBottom = style.paddingRight = style.paddingLeft = Sizes.NavigationPanelPaddings;
            EditorState.CurrentContext.ValueChanged += Redraw;
        }

        private void Redraw(VisualNodesContext currentContext)
        {
            while (currentHierarchy.Count > 0)
            {
                VisualElement current = currentHierarchy[0];
                Remove(current);
                currentHierarchy.RemoveAt(0);
            }

            disposables?.Dispose();
            disposables = new CompositeDisposable();
            Add(root = CreateButton("Root", default, currentContext.Name == "Root"));
            currentHierarchy.Add(root);
            foreach (VisualStateNode stateNode in EditorState.RootContext.Value.Nodes)
            {
                var button = CreateButton($"\t{stateNode.Context.Name}", stateNode, currentContext.Name == stateNode.Context.Name);
                currentHierarchy.Add(button);
                Add(button);
            }
        }

        private VisualElement CreateButton(string text, VisualStateNode stateNode, bool isSelected)
        {
            TextElement element = new TextElement
            {
                text = text,
                style =
                {
                    fontSize = Sizes.NavigationPanelTextSize,
                    unityFontStyleAndWeight = isSelected ? new StyleEnum<FontStyle>(FontStyle.Bold) : default,
                },
            };
            new OnVisualElementClickedRegistration(element, () => MoveToContext(stateNode)).AddTo(disposables);
            return element;
        }

        private void MoveToContext(VisualStateNode stateNode)
        {
            if (stateNode == null) Fabric.Contexts.OpenRootContext();
            else stateNode.Context.Open();
        }
    }
}