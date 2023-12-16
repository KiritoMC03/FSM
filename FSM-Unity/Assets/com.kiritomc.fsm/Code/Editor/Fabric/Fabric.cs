using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class Fabric
    {
        private readonly VisualElement root;

        public ContextsFabric Contexts { get; }
        public NodesFabric Nodes { get; } = new NodesFabric();
        public TransitionsFabric Transitions { get; } = new TransitionsFabric();
        public PopupsFabric Popups { get; }
        public PanelsFabric Panels { get; }

        public Fabric(VisualElement root)
        {
            this.root = root;
            Contexts = new ContextsFabric(root);
            Popups = new PopupsFabric(root);
            Panels = new PanelsFabric(root);
        }

        public static (Fabric Fabric, VisualElement Root) Build(VisualElement rootVisualElement)
        {
            VisualElement root = new VisualElement()
            {
                focusable = true,
                style =
                {
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                },
            };
            rootVisualElement.Add(root);
            root.focusable = true;
            root.style.flexDirection = FlexDirection.Row;
            root.AddManipulator(new SaveWorldPointerPositionManipulator());

            return (new Fabric(root), root);
        }
    }
}