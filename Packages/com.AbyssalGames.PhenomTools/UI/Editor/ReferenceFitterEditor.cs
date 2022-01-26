using System;
using System.Collections;
using System.Collections.Generic;
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
        private SerializedProperty additionalVerticalSizeProperty;
        private SerializedProperty additionalHorizontalSizeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            refProperty = serializedObject.FindProperty("reference");
            layoutProperty = serializedObject.FindProperty("layout");
            additionalVerticalSizeProperty = serializedObject.FindProperty("additionalVerticalSize");
            additionalHorizontalSizeProperty = serializedObject.FindProperty("additionalHorizontalSize");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(refProperty);
            EditorGUILayout.PropertyField(layoutProperty);

            ReferenceFitter fitter = (target as ReferenceFitter);
            if (fitter.verticalFit != UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                EditorGUILayout.PropertyField(additionalVerticalSizeProperty);
            if (fitter.horizontalFit != UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                EditorGUILayout.PropertyField(additionalHorizontalSizeProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
