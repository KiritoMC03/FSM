using FSM.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace FSM.Editor.CustomEditors
{
    [CustomEditor(typeof(FsmDataAsset))]
    public class FsmDataAssetInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                FsmDataAsset data = (FsmDataAsset)target;
                FsmEditorWindow.ShowWindow((editorModel, logicModel) =>
                {
                    data.Json = logicModel;
                    data.EditorModel = editorModel;
                    EditorUtility.SetDirty(data);
                    AssetDatabase.SaveAssetIfDirty(data);
                }, data.EditorModel);
            }
            DrawDefaultInspector();
        }
    }
}