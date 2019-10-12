using UnityEditor;
using UnityEngine;

public static class ScriptTemplateHandler
{
    [MenuItem("Assets/Create C# Script/Empty Monobehavior", false, 1)]
    public static void CreateBlankScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 6), "/Templates/PhenomTemplates-NewScript.cs.txt"), "NewMonobehavior.cs");
    }

    [MenuItem("Assets/Create C# Script/Generic Weapon", false, 2)]
    public static void CreateWeaponScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 6), "/Templates/PhenomTemplates-NewWeapon.cs.txt"), "Weapon_.cs");
    }

    [MenuItem("Assets/Create C# Script/Weapons/Melee", false, 3)]
    public static void CreateMeleeWeaponScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Concat(Application.dataPath.Remove(Application.dataPath.Length - 6), "/Templates/PhenomTemplates-NewMeleeWeapon.cs.txt"), "WeaponMelee_.cs");
    }
}
