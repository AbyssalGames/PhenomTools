using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Touchable))]
public class TouchableEditor : Editor
{
    private SerializedProperty blockRaycastsProperty;

    private void OnEnable()
    {
        blockRaycastsProperty = serializedObject.FindProperty("m_RaycastTarget");
    }

    public override void OnInspectorGUI() 
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(blockRaycastsProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
