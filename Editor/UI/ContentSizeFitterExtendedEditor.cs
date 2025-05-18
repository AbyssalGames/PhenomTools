using UnityEngine.UI;

namespace UnityEditor.UI
{
  [CustomEditor(typeof(ContentSizeFitterExtended))]
  [CanEditMultipleObjects]
  public class ContentSizeFitterExtendedEditor : ContentSizeFitterEditor
  {
    private SerializedProperty maximumConstraints;

    protected override void OnEnable()
    {
      base.OnEnable();
      maximumConstraints = serializedObject.FindProperty("maximumConstraints");
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      EditorGUILayout.PropertyField(maximumConstraints, true);

      serializedObject.ApplyModifiedProperties();
    }
  }
}
