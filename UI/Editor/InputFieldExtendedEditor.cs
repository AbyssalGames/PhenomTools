using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;

namespace PhenomTools
{
    [CustomEditor(typeof(InputFieldExtended))]
    [CanEditMultipleObjects]
    public class InputFieldExtendedEditor : TMP_InputFieldEditor
    {
        private SerializedProperty onBeginDrag;
        private SerializedProperty onEndDrag;

        protected override void OnEnable()
        {
            base.OnEnable();
            onBeginDrag = serializedObject.FindProperty("onBeginDrag");
            onEndDrag = serializedObject.FindProperty("onEndDrag");
        }
 
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(onBeginDrag);
            EditorGUILayout.PropertyField(onEndDrag);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
