using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ToggleExtended), true)]
    [CanEditMultipleObjects]

    public class ToggleExtendedEditor : ToggleEditor
    {
        private SerializedProperty onHoverProperty;
        private SerializedProperty onDownProperty;
        private SerializedProperty onExitProperty;
        private SerializedProperty onReenterProperty;
        private SerializedProperty onGhostToggleProperty;

        private SerializedProperty onHoverSoundProperty;
        private SerializedProperty onDownSoundProperty;
        private SerializedProperty onClickSoundProperty;
        private SerializedProperty onToggleOnSoundProperty;
        private SerializedProperty onToggleOffSoundProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            onHoverProperty = serializedObject.FindProperty("onHover");
            onDownProperty = serializedObject.FindProperty("onDown");
            onExitProperty = serializedObject.FindProperty("onExit");
            onReenterProperty = serializedObject.FindProperty("onReenter");
            onGhostToggleProperty = serializedObject.FindProperty("onGhostToggle");

            onHoverSoundProperty = serializedObject.FindProperty("hoverSound");
            onDownSoundProperty = serializedObject.FindProperty("downSound");
            onClickSoundProperty = serializedObject.FindProperty("clickSound");
            onToggleOnSoundProperty = serializedObject.FindProperty("toggleOnSound");
            onToggleOffSoundProperty = serializedObject.FindProperty("toggleOffSound");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(onHoverProperty);
            EditorGUILayout.PropertyField(onDownProperty);
            EditorGUILayout.PropertyField(onExitProperty);
            EditorGUILayout.PropertyField(onReenterProperty);
            EditorGUILayout.PropertyField(onGhostToggleProperty);

            EditorGUILayout.PropertyField(onHoverSoundProperty);
            EditorGUILayout.PropertyField(onDownSoundProperty);
            EditorGUILayout.PropertyField(onClickSoundProperty);
            EditorGUILayout.PropertyField(onToggleOnSoundProperty);
            EditorGUILayout.PropertyField(onToggleOffSoundProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
