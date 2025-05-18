using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
  public class ScriptTemplateHandler : ScriptableObject
  {
    public Object classTemplate, monoBehaviourTemplate, structTemplate, scriptableObjectTemplate, editorTemplate;
    
    [MenuItem("Assets/Create C# Script/Class", false, 1)]
    public static void CreateClass()
    {
      ScriptTemplateHandler dummy = CreateInstance<ScriptTemplateHandler>();
      Object file = dummy.classTemplate;
      DestroyImmediate(dummy);
      ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(file), "NewClass.cs");
    }

    [MenuItem("Assets/Create C# Script/Monobehavior", false, 1)]
    public static void CreateMonoBehavior()
    {
      ScriptTemplateHandler dummy = CreateInstance<ScriptTemplateHandler>();
      Object file = dummy.monoBehaviourTemplate;
      DestroyImmediate(dummy);
      ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(file), "NewMonobehavior.cs");
    }

    [MenuItem("Assets/Create C# Script/Struct", false, 1)]
    public static void CreateStruct()
    {
      ScriptTemplateHandler dummy = CreateInstance<ScriptTemplateHandler>();
      Object file = dummy.structTemplate;
      DestroyImmediate(dummy);
      ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(file), "NewStruct.cs");
    }

    [MenuItem("Assets/Create C# Script/Scriptable Object", false, 1)]
    public static void CreateScriptableObject()
    {
      ScriptTemplateHandler dummy = CreateInstance<ScriptTemplateHandler>();
      Object file = dummy.scriptableObjectTemplate;
      DestroyImmediate(dummy);
      ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(file), "NewScriptableObject.cs");
    }

    [MenuItem("Assets/Create C# Script/Editor", false, 1)]
    public static void CreateEditor()
    {
      ScriptTemplateHandler dummy = CreateInstance<ScriptTemplateHandler>();
      Object file = dummy.editorTemplate;
      DestroyImmediate(dummy);
      ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(file), "NewEditor.cs");
    }
  }
}
