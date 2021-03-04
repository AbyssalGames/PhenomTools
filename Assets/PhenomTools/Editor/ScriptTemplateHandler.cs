using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    public static class ScriptTemplateHandler
    {
        [MenuItem("Assets/Create C# Script/Empty Class", false, 1)]
        public static void CreateBlankClass()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewClass.cs.txt"), "NewClass.cs");
        }

        [MenuItem("Assets/Create C# Script/Empty Monobehavior", false, 1)]
        public static void CreateBlankMonoBehavior()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewMonoBehavior.cs.txt"), "NewMonobehavior.cs");
        }

        [MenuItem("Assets/Create C# Script/Empty Struct", false, 1)]
        public static void CreateBlankStruct()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewStruct.cs.txt"), "NewStruct.cs");
        }
    }
}
