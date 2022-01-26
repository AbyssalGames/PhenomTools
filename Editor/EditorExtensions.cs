using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEditor;

public class EditorExtensions
{
    [MenuItem("PhenomTools/Open Persistent Data Folder")]
    public static void OpenPersistentDataFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
