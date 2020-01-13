using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ButtonExtended), true)]
    [CanEditMultipleObjects]
    public class ButtonExtendedEditor : ButtonEditor
    {
        SerializedProperty m_SoundProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SoundProperty = serializedObject.FindProperty("clickSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_SoundProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
