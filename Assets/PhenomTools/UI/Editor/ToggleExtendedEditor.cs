using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ToggleExtended), true)]
    [CanEditMultipleObjects]

    public class ToggleExtendedEditor : ToggleEditor
    {
        SerializedProperty onSoundProperty;
        SerializedProperty offSoundProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            onSoundProperty = serializedObject.FindProperty("onSound");
            offSoundProperty = serializedObject.FindProperty("offSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(onSoundProperty);
            EditorGUILayout.PropertyField(offSoundProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
