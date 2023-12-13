using System.Collections.Generic;
using System.Linq;
using FSM.Editor.Serialization;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class FsmEditorWindow : UnityEditor.EditorWindow
    {
        private readonly EditorState editorState = new EditorState();
        private Fabric fabric;
        private StatesContext rootContext;

        [MenuItem("FSM/Reset Save")]
        public static void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        
        [UnityEditor.MenuItem("FSM/Editor")]
        private static void ShowWindow()
        {
            FsmEditorWindow window = GetWindow<FsmEditorWindow>();
            window.titleContent = new UnityEngine.GUIContent("Fsm Editor sWindow");
            window.Show();
        }

        private async void CreateGUI()
        {
            string json = PlayerPrefs.GetString("FsmEditorKey", default);
            StatesContextModel statesContextModel = json == null ? new StatesContextModel() : JsonConvert.DeserializeObject<StatesContextModel>(json);
            PlayerPrefs.SetString("FsmEditorKey", json);
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
            root.RegisterCallback<PointerMoveEvent>(e =>
            {
                editorState.PointerPosition.Value = root.LocalToWorld(e.position);
            });
            fabric = new Fabric(editorState, root);
            root.Add(rootContext = CreateRootContext(statesContextModel));
            // root.Add(DrawNode(sn = new StateNode("New state")));
            // root.Add(DrawNode(new StateNode("New state")));

            // while (sn.transitions.Count == 0)
            // {
            //     await Task.Yield();
            // }
            // root.Add(new TransitionContext(sn.transitions.First(), editorState, fabric));
            // root.Add(DrawNode(new NotNode(new NotLayoutNode())));
            // root.Add(DrawNode(new OrNode(new OrLayoutNode())));
            // root.Add(DrawNode(new AndNode(new AndLayoutNode())));
            // root.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new FalseCondition()))));
            // root.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new TrueCondition()))));
        }

        private StatesContext CreateRootContext(StatesContextModel model)
        {
            StatesContext root = new StatesContext(editorState, fabric);
            List<StateNode> states = model?.StateNodeModels.Select(nodeModel =>
            {
                StateNode node = fabric.CreateStateNode(nodeModel.Name, root, nodeModel.Position);
                root.Add(node);
                return node;
            }).ToList();
            if (states != null)
            {
                root.StateNodes = states;
                for (int i = 0; i < states.Count; i++)
                {
                    StateNode state = states[i];
                    state.Transitions = model.StateNodeModels[i].OutgoingTransitions.Select(transitionModel =>
                    {
                        StateNode target = states.Find(targetState => targetState.StateName == transitionModel.TargetName);
                        StateTransition transition = fabric.CreateTransition(state, target);
                        return transition;
                    }).ToList();
                }
            }

            return root;
        }

        private void OnDestroy()
        {
            StatesContextModel rootContextModel = new StatesContextModel
            {
                StateNodeModels = rootContext.StateNodes.Select(node =>
                {
                    return new StateNodeModel
                    (
                        node.StateName,
                        new Vector2Model(node.resolvedStyle.left, node.resolvedStyle.top),
                        node.Transitions.Select(CreateStateTransitionModel).ToArray()
                    );

                    StateTransitionModel CreateStateTransitionModel(StateTransition transition)
                    {
                        return new StateTransitionModel(node.StateName, transition.Target.StateName, default);
                    }
                }).ToArray(),
            };

            string json = JsonConvert.SerializeObject(rootContextModel);
            PlayerPrefs.SetString("FsmEditorKey", json);
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
            // newNode.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            // newNode.AddManipulator(new CreateTransitionManipulator(editorState, root));
            // newNode.AddManipulator(new RouteConnectionManipulator(editorState, root));
            // newNode.ConnectionRequestHandledCallback += async () =>
            // {
            //     editorState.DraggingLocked.Value = true;
            //     TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            //     root.RegisterCallback<MouseUpEvent>(Track);
            //     await completionSource.Task;
            //     root.UnregisterCallback<MouseUpEvent>(Track);
            //     editorState.DraggingLocked.Value = false;
            //     Vector2 pos = Event.current.mousePosition;
            //     List<VisualElement> elements = new List<VisualElement>(10);
            //     root.panel.PickAll(pos, elements);
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