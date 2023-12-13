using FSM.Editor.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class FsmEditorWindow : EditorWindow
    {
        private readonly EditorState editorState = new EditorState();
        private Fabric fabric;
        private FsmContext rootContext;
        private EditorSerializer editorSerializer;

        [MenuItem("FSM/Reset Save")]
        public static void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        
        [MenuItem("FSM/Editor")]
        private static void ShowWindow()
        {
            FsmEditorWindow window = GetWindow<FsmEditorWindow>();
            window.titleContent = new GUIContent("Fsm Editor sWindow");
            window.Show();
        }

        private void CreateGUI()
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
            root.RegisterCallback<PointerMoveEvent>(e =>
            {
                editorState.PointerPosition.Value = root.LocalToWorld(e.position);
            });
            fabric = new Fabric(editorState, root);
            editorSerializer = new EditorSerializer(editorState, fabric);
            string json = PlayerPrefs.GetString("FsmEditorKey", default);
            rootContext = new FsmContext
            {
                StatesContext = editorSerializer.Deserialize(json).StatesContext,
            };
            root.Add(rootContext.StatesContext);
        }

        private void OnDestroy()
        {
            string json = editorSerializer?.Serialize(rootContext);
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
    }
}