using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    [CustomEditor(typeof(UIExtentionTools))]
    public class UIExtensionToolsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UIExtentionTools tools = target as UIExtentionTools;

            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Gather Selectables"))
            {
                Undo.RecordObject(tools, "Gather Selectables");
                tools.GatherSelectables();
            }

            if (GUILayout.Button("Extend Buttons"))
            {
                Undo.RecordObject(tools, "Extend Buttons");
                tools.ExtendButtons();
            }

            if (GUILayout.Button("Extend Toggles"))
            {
                Undo.RecordObject(tools, "Extend Toggles");
                tools.ExtendToggles();
            }

            EditorGUI.EndChangeCheck();


            base.OnInspectorGUI();
        }
    }
}
