using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    public static class ScriptTemplateHandler
    {
        [MenuItem("Assets/Create C# Script/Class", false, 1)]
        public static void CreateClass()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewClass.cs.txt"), "NewClass.cs");
        }

        [MenuItem("Assets/Create C# Script/Monobehavior", false, 1)]
        public static void CreateMonoBehavior()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewMonoBehavior.cs.txt"), "NewMonobehavior.cs");
        }

        [MenuItem("Assets/Create C# Script/Struct", false, 1)]
        public static void CreateStruct()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewStruct.cs.txt"), "NewStruct.cs");
        }

        [MenuItem("Assets/Create C# Script/Scriptable Object", false, 1)]
        public static void CreateScriptableObject()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewScriptableObject.cs.txt"), "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create C# Script/Editor", false, 1)]
        public static void CreateEditor()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewEditor.cs.txt"), "NewEditor.cs");
        }
    }
}
