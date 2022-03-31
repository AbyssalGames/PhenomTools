using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PhenomTools;

namespace PhenomTools
{
    [CustomEditor(typeof(Tester))]
    //[CanEditMultipleObjects]
    public class TesterEditor : Editor
    {
        //SerializedProperty value;
 
        public void OnEnable()
        {
            //value = serializedObject.FindProperty("Value");
        }
 
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
 
            // EditorGUI content
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}
