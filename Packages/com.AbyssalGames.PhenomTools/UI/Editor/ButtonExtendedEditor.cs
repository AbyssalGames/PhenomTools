﻿using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ButtonExtended), true)]
    [CanEditMultipleObjects]
    public class ButtonExtendedEditor : SelectableEditor
    {
        SerializedProperty m_OnClickProperty;
        private SerializedProperty onHoverProperty;
        private SerializedProperty onDownProperty;
        private SerializedProperty onUpProperty;
        private SerializedProperty onExitProperty;
        private SerializedProperty onReenterProperty;
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
            onHoverProperty = serializedObject.FindProperty("onHover");
            onDownProperty = serializedObject.FindProperty("onDown");
            onUpProperty = serializedObject.FindProperty("onUp");
            onExitProperty = serializedObject.FindProperty("onExit");
            onReenterProperty = serializedObject.FindProperty("onReenter");
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

            eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events");

            if (eventsFoldout)
            {
                EditorGUILayout.PropertyField(m_OnClickProperty);
                EditorGUILayout.PropertyField(onHoverProperty);
                EditorGUILayout.PropertyField(onDownProperty);
                EditorGUILayout.PropertyField(onUpProperty);
                EditorGUILayout.PropertyField(onExitProperty);
                EditorGUILayout.PropertyField(onReenterProperty);
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
