using UnityEditor;
using UnityEditor.UI;

namespace PhenomTools.UI.Editor
{
  [CustomEditor(typeof(ScrollRectExtended), true)]
  [CanEditMultipleObjects]
  public class ScrollRectExtendedEditor : ScrollRectEditor
  {
    private SerializedProperty onScrollProperty;
    private SerializedProperty onBeginDragProperty;
    private SerializedProperty onEndDragProperty;

    protected override void OnEnable()
    {
      base.OnEnable();
      onScrollProperty = serializedObject.FindProperty("onScroll");
      onBeginDragProperty = serializedObject.FindProperty("onBeginDrag");
      onEndDragProperty = serializedObject.FindProperty("onEndDrag");
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      serializedObject.Update();

      EditorGUILayout.PropertyField(onScrollProperty);
      EditorGUILayout.PropertyField(onBeginDragProperty);
      EditorGUILayout.PropertyField(onEndDragProperty);

      serializedObject.ApplyModifiedProperties();
    }
  }
}
