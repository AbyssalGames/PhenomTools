using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ButtonExtended), true)]
    [CanEditMultipleObjects]
    public class ButtonExtendedEditor : SelectableEditor
    {
        private SerializedProperty m_OnClickProperty;
        private SerializedProperty longPressDurationProperty;
        private SerializedProperty onHoverProperty;
        private SerializedProperty onDownProperty;
        private SerializedProperty onUpProperty;
        private SerializedProperty onExitProperty;
        private SerializedProperty onReenterProperty;
        private SerializedProperty onLongPressProperty;
        private SerializedProperty onGhostClickProperty;

#if PhenomAudio
        private SerializedProperty onHoverSoundProperty;
        private SerializedProperty onDownSoundProperty;
        private SerializedProperty onClickSoundProperty;
#endif

        private bool eventsFoldout;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
            longPressDurationProperty = serializedObject.FindProperty("longPressDuration");
            onHoverProperty = serializedObject.FindProperty("onHover");
            onDownProperty = serializedObject.FindProperty("onDown");
            onUpProperty = serializedObject.FindProperty("onUp");
            onExitProperty = serializedObject.FindProperty("onExit");
            onReenterProperty = serializedObject.FindProperty("onReenter");
            onLongPressProperty = serializedObject.FindProperty("onLongPress");
            onGhostClickProperty = serializedObject.FindProperty("onGhostClick");

#if PhenomAudio
            onHoverSoundProperty = serializedObject.FindProperty("hoverSound");
            onDownSoundProperty = serializedObject.FindProperty("downSound");
            onClickSoundProperty = serializedObject.FindProperty("clickSound");
#endif
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(longPressDurationProperty);
            eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events");

            if (eventsFoldout)
            {
                EditorGUILayout.PropertyField(m_OnClickProperty);
                EditorGUILayout.PropertyField(onHoverProperty);
                EditorGUILayout.PropertyField(onDownProperty);
                EditorGUILayout.PropertyField(onUpProperty);
                EditorGUILayout.PropertyField(onExitProperty);
                EditorGUILayout.PropertyField(onReenterProperty);
                EditorGUILayout.PropertyField(onLongPressProperty);
                EditorGUILayout.PropertyField(onGhostClickProperty);

#if PhenomAudio
                EditorGUILayout.PropertyField(onHoverSoundProperty);
                EditorGUILayout.PropertyField(onDownSoundProperty);
                EditorGUILayout.PropertyField(onClickSoundProperty);
#endif
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
