using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class FsmEditorWindow : UnityEditor.EditorWindow
    {
        private EditorState editorState = new EditorState();
        
        [UnityEditor.MenuItem("FSM/Editor")]
        private static void ShowWindow()
        {
            FsmEditorWindow window = GetWindow<FsmEditorWindow>();
            window.titleContent = new UnityEngine.GUIContent("Fsm Editor sWindow");
            window.Show();
        }

        private void CreateGUI()
        {
            // rootVisualElement.Add(DrawNode(new NotNode(new NotLayoutNode())));
            // rootVisualElement.Add(DrawNode(new OrNode(new OrLayoutNode())));
            // rootVisualElement.Add(DrawNode(new AndNode(new AndLayoutNode())));
            // rootVisualElement.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new FalseCondition()))));
            // rootVisualElement.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new TrueCondition()))));
            rootVisualElement.Add(DrawNode(new StateNode("New state")));
            rootVisualElement.Add(DrawNode(new StateNode("New state")));
            rootVisualElement.Add(DrawNode(new StateNode("New state")));
            rootVisualElement.Add(DrawNode(new StateNode("New state")));
        }

        private void Empty()
        {
            new VisualElement
            {
                focusable = false,
                tabIndex = 0,
                delegatesFocus = false,
                viewDataKey = null,
                userData = null,
                usageHints = UsageHints.None,
                pickingMode = PickingMode.Position,
                name = null,
                languageDirection = LanguageDirection.Inherit,
                visible = false,
                generateVisualContent = null,
                tooltip = null,
                style =
                {
                    alignContent = default,
                    alignItems = default,
                    alignSelf = default,
                    backgroundColor = default,
                    backgroundImage = default,
                    backgroundPositionX = default,
                    backgroundPositionY = default,
                    backgroundRepeat = default,
                    backgroundSize = default,
                    borderBottomColor = default,
                    borderBottomLeftRadius = default,
                    borderBottomRightRadius = default,
                    borderBottomWidth = default,
                    borderLeftColor = default,
                    borderLeftWidth = default,
                    borderRightColor = default,
                    borderRightWidth = default,
                    borderTopColor = default,
                    borderTopLeftRadius = default,
                    borderTopRightRadius = default,
                    borderTopWidth = default,
                    bottom = default,
                    color = default,
                    cursor = default,
                    display = default,
                    flexBasis = default,
                    flexDirection = default,
                    flexGrow = default,
                    flexShrink = default,
                    flexWrap = default,
                    fontSize = default,
                    height = default,
                    justifyContent = default,
                    left = default,
                    letterSpacing = default,
                    marginBottom = default,
                    marginLeft = default,
                    marginRight = default,
                    marginTop = default,
                    maxHeight = default,
                    maxWidth = default,
                    minHeight = default,
                    minWidth = default,
                    opacity = default,
                    overflow = default,
                    paddingBottom = default,
                    paddingLeft = default,
                    paddingRight = default,
                    paddingTop = default,
                    position = default,
                    right = default,
                    rotate = default,
                    scale = default,
                    textOverflow = default,
                    textShadow = default,
                    top = default,
                    transformOrigin = default,
                    transitionDelay = default,
                    transitionDuration = default,
                    transitionProperty = default,
                    transitionTimingFunction = default,
                    translate = default,
                    unityBackgroundImageTintColor = default,
                    unityFont = default,
                    unityFontDefinition = default,
                    unityFontStyleAndWeight = default,
                    unityOverflowClipBox = default,
                    unityParagraphSpacing = default,
                    unitySliceBottom = default,
                    unitySliceLeft = default,
                    unitySliceRight = default,
                    unitySliceScale = default,
                    unitySliceTop = default,
                    unityTextAlign = default,
                    unityTextOutlineColor = default,
                    unityTextOutlineWidth = default,
                    unityTextOverflowPosition = default,
                    visibility = default,
                    whiteSpace = default,
                    width = default,
                    wordSpacing = default,
                },
            };
        }

        public Node DrawNode(Node newNode)
        {
            newNode.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            newNode.AddManipulator(new RouteConnectionManipulator(editorState, rootVisualElement));
            // newNode.ConnectionRequestHandledCallback += async () =>
            // {
            //     editorState.DraggingLocked.Value = true;
            //     TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            //     rootVisualElement.RegisterCallback<MouseUpEvent>(Track);
            //     await completionSource.Task;
            //     rootVisualElement.UnregisterCallback<MouseUpEvent>(Track);
            //     editorState.DraggingLocked.Value = false;
            //     Vector2 pos = Event.current.mousePosition;
            //     List<VisualElement> elements = new List<VisualElement>(10);
            //     rootVisualElement.panel.PickAll(pos, elements);
            //     foreach (VisualElement element in elements)
            //         if (element is Node node)
            //             return node;
            //     return default;
            //
            //     void Track(MouseUpEvent _) => completionSource.SetResult(true);
            // };
            return newNode;
        }
    }
}