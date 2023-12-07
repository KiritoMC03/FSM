using FSM.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace FSM.Editor
{
    public class FsmEditorWindow : UnityEditor.EditorWindow
    {
        [UnityEditor.MenuItem("FSM/Editor")]
        private static void ShowWindow()
        {
            FsmEditorWindow window = GetWindow<FsmEditorWindow>();
            window.titleContent = new UnityEngine.GUIContent("Fsm Editor sWindow");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(DrawNode(new NotNode(new NotLayoutNode())));
            rootVisualElement.Add(DrawNode(new OrNode(new OrLayoutNode())));
            rootVisualElement.Add(DrawNode(new AndNode(new AndLayoutNode())));
            rootVisualElement.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new FalseCondition()))));
            rootVisualElement.Add(DrawNode(new ConditionNode(new ConditionLayoutNode(new TrueCondition()))));
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
            newNode.ApplyStyle();
            newNode.AsDraggable();
            return newNode;
        }
    }

    public static class Colors
    {
        public static Color NodeBackground => new Color(0.15f, 0.15f, 0.15f);
        public static StyleColor NodeConnectionBackground => new Color(0.1f, 0.1f, 0.1f);
    }

    public static class Modifiers
    {
        public static void AsDraggable(this VisualElement element)
        {
            bool isPressed = false;
            Vector2 offset = default;
            element.RegisterCallback<MouseLeaveEvent>(StopDrag);
            element.RegisterCallback<MouseUpEvent>(StopDrag);
            element.RegisterCallback<MouseDownEvent>(HandleMouseDown);
            element.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
            return;

            void StartDrag() => isPressed = true;
            void StopDrag<T>(T _) => isPressed = false;
            void HandleMouseDown(IMouseEvent evt)
            {
                StartDrag();
                element.BringToFront();
                offset = new Vector2(
                    element.resolvedStyle.left - evt.mousePosition.x,
                    element.resolvedStyle.top - evt.mousePosition.y);
            }
            void HandleMouseMove(IMouseEvent evt)
            {
                if (!isPressed) return;
                element.style.left = evt.mousePosition.x + offset.x;
                element.style.top = evt.mousePosition.y + offset.y;
            }
        }
    }
}