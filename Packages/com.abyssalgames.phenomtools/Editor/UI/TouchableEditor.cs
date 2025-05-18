using UnityEditor;

namespace PhenomTools.UI.Editor
{
  [CustomEditor(typeof(Touchable))]
  [CanEditMultipleObjects]
  public class TouchableEditor : UnityEditor.Editor
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
}