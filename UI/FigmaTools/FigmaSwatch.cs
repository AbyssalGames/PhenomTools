using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Object = UnityEngine.Object;
#endif

namespace PhenomTools.UI
{
  public enum FigmaSwatchType
  {
    None = 0,
    Normal = 1,
    Gradient = 2
  }
  
  [CreateAssetMenu(fileName = "Swatch", menuName = "UI/Figma Tools/Theme Swatch", order = 51)]
  public class FigmaSwatch : ScriptableObject
  {
    public FigmaSwatchType Type => type;
    public Color Color => color;
    public Gradient Gradient => gradient;

    [SerializeField] 
    private FigmaSwatchType type;
    [SerializeField, ShowIf("type", FigmaSwatchType.Normal)] 
    private Color color = Color.white;
    [SerializeField, ShowIf("type", FigmaSwatchType.Gradient)]
    private Gradient gradient;

    public static implicit operator Color(FigmaSwatch swatch) => swatch.Color;
  }
  
#if UNITY_EDITOR
  [CustomEditor(typeof(FigmaSwatch), true)]
  [CanEditMultipleObjects]
  public class FigmaSwatchEditor : OdinEditor
  {
    private FigmaSwatch Instance => (FigmaSwatch)target;

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
      if (Instance.Type != FigmaSwatchType.None)
      {
        Type t = GetType("UnityEditor.SpriteUtility");
        if (t != null)
        {
          MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
          if (method != null)
          {
            Texture2D tex = (Texture2D)EditorGUIUtility.Load("SwatchIcon.png");
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            
            object ret = method.Invoke("RenderStaticPreview", new object[] { sprite, Instance.Type == FigmaSwatchType.Gradient ? Instance.Gradient.Evaluate(.5f) : Instance.Color, width, height });
            
            if (ret is Texture2D texture2D)
              return texture2D;
          }
        }
      }

      return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    private static Type GetType(string typeName)
    {
      Type type = Type.GetType(typeName);
      if (type != null)
        return type;

      if (typeName.Contains("."))
      {
        string assemblyName = typeName.Substring(0, typeName.IndexOf('.'));
        Assembly assembly = Assembly.Load(assemblyName);
        if (assembly == null)
          return null;
        type = assembly.GetType(typeName);
        if (type != null)
          return type;
      }

      Assembly currentAssembly = Assembly.GetExecutingAssembly();
      AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
      foreach (AssemblyName assemblyName in referencedAssemblies)
      {
        Assembly assembly = Assembly.Load(assemblyName);
        if (assembly != null)
        {
          type = assembly.GetType(typeName);
          if (type != null)
            return type;
        }
      }
      return null;
    }
  }

  [CustomPropertyDrawer(typeof(FigmaSwatch))]
  public class FigmaSwatchDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);
      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
      
      if (property.objectReferenceValue != null)
      {
        float half = position.x / 2f;
        Rect colorRect = new(position.x, position.y, half, position.height);
        Rect objRect = new(position.x + half, position.y, position.width - half, position.height);
        
        SerializedObject so = new(property.objectReferenceValue);
        SerializedProperty colorProp = (FigmaSwatchType)so.FindProperty("type").intValue == FigmaSwatchType.Gradient ? so.FindProperty("gradient") : so.FindProperty("color");
        
        EditorGUI.PropertyField(colorRect, colorProp, GUIContent.none);
        EditorGUI.ObjectField(objRect, property, GUIContent.none);

        if (so.hasModifiedProperties)
        {
          so.ApplyModifiedProperties();
          so.Update();
        }
      }
      else
      {
        EditorGUI.ObjectField(position, property, GUIContent.none);
      }
      
      EditorGUI.EndProperty();
    }
  }
#endif
}