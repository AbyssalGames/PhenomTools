using System;
using UnityEngine;
using UnityEditor;

namespace Abyssal.Utilities
{
  [Serializable]
  public class ObservableProperty<T>
  {
    public event Action<T> onChanged;

    [SerializeField]
    private T value;

    public T Value
    {
      get => value;
      set => Set(value);
    }

    private void Set(T newValue)
    {
      value = newValue;
      onChanged?.Invoke(newValue);
    }

    public void SetWithoutNotify(T newValue) => value = newValue;

    public static implicit operator T(ObservableProperty<T> instance) => instance.Value;
  }

#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(ObservableProperty<>))]
  public class ObservablePropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), label);
    }
  }
#endif
}
