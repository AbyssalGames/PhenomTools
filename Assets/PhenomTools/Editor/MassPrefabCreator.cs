using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEditor;
using System.IO;

namespace PhenomTools
{
    public static class MassPrefabCreator
    {
        [MenuItem("GameObject/Create Prefab(s)", priority = 0)]
        private static void Initialize(MenuCommand command)
        {
            GameObject[] selected = Selection.GetFiltered<GameObject>(SelectionMode.TopLevel);

            if (selected.Length > 1 && command.context != selected[0])
                return;

            string startingPath = Application.dataPath;
            if (Directory.Exists(Application.dataPath + "/Prefabs"))
                startingPath += "/Prefabs";

            string folderPath = EditorUtility.SaveFolderPanel("Create Prefab(s)", startingPath, "NewPrefab");
            CreatePrefabs(selected, folderPath);
        }

        public static void CreatePrefabs(GameObject[] selected, string folderPath)
        {
            if (selected.Length > 0 && !string.IsNullOrWhiteSpace(folderPath))
            {
                foreach (GameObject go in selected)
                    PrefabUtility.SaveAsPrefabAsset(go, folderPath + "/" + go.name + ".prefab");
            }

            Debug.Log("Saved: " + selected.Length + " GameObjects as Prefabs to: " + folderPath);
        }
    }
}