using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ScrollRectExtended), true)]
    [CanEditMultipleObjects]
    public class ScrollRectExtendedEditor : ScrollRectEditor
    {
        private SerializedProperty onBeginDragProperty;
        private SerializedProperty onEndDragProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            onBeginDragProperty = serializedObject.FindProperty("onBeginDrag");
            onEndDragProperty = serializedObject.FindProperty("onEndDrag");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(onBeginDragProperty);
            EditorGUILayout.PropertyField(onEndDragProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
