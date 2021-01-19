using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ReferenceFitter), true)]
    [CanEditMultipleObjects]
    public class ReferenceFitterEditor : ContentSizeFitterEditor
    {
        private SerializedProperty refProperty;
        private SerializedProperty layoutProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            refProperty = serializedObject.FindProperty("reference");
            layoutProperty = serializedObject.FindProperty("layout");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(refProperty);
            EditorGUILayout.PropertyField(layoutProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
