using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ButtonExtended), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Button Component.
    ///   Extend this class to write a custom editor for a component derived from Button.
    /// </summary>
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
