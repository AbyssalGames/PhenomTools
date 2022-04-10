using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(AspectRatioLayoutElement))]
    [CanEditMultipleObjects]
    public class AspectRatioLayoutElementEditor : LayoutElementEditor
    {
        private SerializedProperty aspectMode;
        private SerializedProperty aspectRatio;

        protected override void OnEnable()
        {
            aspectMode = serializedObject.FindProperty("m_aspectMode");
            aspectRatio = serializedObject.FindProperty("m_aspectRatio");
        }
 
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(aspectMode);
            EditorGUILayout.PropertyField(aspectRatio);
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}
