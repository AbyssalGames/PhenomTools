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



        //[MenuItem("Assets/Create C# Script/Generic Weapon", false, 2)]
        //public static void CreateWeaponScript()
        //{
        //    ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewWeapon.cs.txt"), "Weapon_.cs");
        //}

        //[MenuItem("Assets/Create C# Script/Weapons/Melee", false, 3)]
        //public static void CreateMeleeWeaponScript()
        //{
        //    ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 7), "/Templates/PhenomTemplates-NewMeleeWeapon.cs.txt"), "WeaponMelee_.cs");
        //}
    }
}
