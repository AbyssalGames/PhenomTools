using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ButtonExtended), true)]
    [CanEditMultipleObjects]
    public class ButtonExtendedEditor : ButtonEditor
    {
        private SerializedProperty onHoverProperty;
        private SerializedProperty onDownProperty;
        private SerializedProperty onExitProperty;
        private SerializedProperty onReenterProperty;
        private SerializedProperty onGhostClickProperty;

        private SerializedProperty onHoverSoundProperty;
        private SerializedProperty onDownSoundProperty;
        private SerializedProperty onClickSoundProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            onHoverProperty = serializedObject.FindProperty("onHover");
            onDownProperty = serializedObject.FindProperty("onDown");
            onExitProperty = serializedObject.FindProperty("onExit");
            onReenterProperty = serializedObject.FindProperty("onReenter");
            onGhostClickProperty = serializedObject.FindProperty("onGhostClick");

            onHoverSoundProperty = serializedObject.FindProperty("hoverSound");
            onDownSoundProperty = serializedObject.FindProperty("downSound");
            onClickSoundProperty = serializedObject.FindProperty("clickSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(onHoverProperty);
            EditorGUILayout.PropertyField(onDownProperty);
            EditorGUILayout.PropertyField(onExitProperty);
            EditorGUILayout.PropertyField(onReenterProperty);
            EditorGUILayout.PropertyField(onGhostClickProperty);

            EditorGUILayout.PropertyField(onHoverSoundProperty);
            EditorGUILayout.PropertyField(onDownSoundProperty);
            EditorGUILayout.PropertyField(onClickSoundProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
