using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Serialization;
using FSM.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public delegate void SaveFsmEditorDataDelegate(string editorModel, string logicModel);
    
    public class FsmEditorWindow : EditorWindow
    {
        private readonly EditorState editorState = new EditorState();
        private EditorSerializer editorSerializer;
        private FsmContext rootContext;
        private VisualElement root;
        private Fabric fabric;
        private SaveFsmEditorDataDelegate saveFsmEditorDataDelegate;
        private string editorModel;
        private bool isFieldsFilled;

        public static void ShowWindow(SaveFsmEditorDataDelegate saveFsmEditorDataDelegate, string editorModel)
        {
            FsmEditorWindow window = GetWindow<FsmEditorWindow>();
            window.saveFsmEditorDataDelegate = saveFsmEditorDataDelegate;
            window.editorModel = editorModel;
            window.isFieldsFilled = true;
            window.titleContent = new GUIContent("Fsm Editor Window");
            window.Show();
        }

        private void CreateGUI()
        {
            (fabric, root) = Fabric.Build(rootVisualElement);
            editorState.EditorRoot.Value = root;
            ServiceLocator.Instance.Set(fabric);
            ServiceLocator.Instance.Set(editorState);
            LeftPanel leftPanel = fabric.Panels.LeftPanel();
            leftPanel.Add(fabric.Panels.NavigationPanel());
            LoadEditor();

            root.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.S && e.ctrlKey)
                {
                    Save();
                }
            });
        }

        private void Save()
        {
            string json = editorSerializer?.Serialize(rootContext);

            List<StateModel> r = LogicSerializer.Serialize(rootContext);
            string logicJson = JsonConvert.SerializeObject(r);
            saveFsmEditorDataDelegate.Invoke(json, logicJson);
        }

        private void OnDestroy()
        {
            Save();
        }

        private async void LoadEditor()
        {
            while (!isFieldsFilled) await Task.Yield();
            editorSerializer = new EditorSerializer();
            rootContext = new FsmContext
            {
                StatesContext = editorSerializer.Deserialize(editorModel ?? "").StatesContext,
            };
            root.Add(rootContext.StatesContext);
            editorState.CurrentContext.Value = editorState.RootContext.Value = rootContext.StatesContext;
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