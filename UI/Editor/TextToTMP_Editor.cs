using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    [CustomEditor(typeof(TextToTMP)), CanEditMultipleObjects]
    public class TextToTMP_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            DrawDefaultInspector();

            if (GUILayout.Button("Convert Text to TextMeshPro"))
            {
                foreach (Object t in targets)
                    ((TextToTMP)t).ConvertTextToTMP();
            }
        }
    }
}
