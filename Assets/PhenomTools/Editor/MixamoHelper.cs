using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    public class MixamoHelper : Editor
    {
        private static List<string> allFiles = new List<string>();

        [MenuItem("PhenomTools/Mixamo Rename Utility")]
        public static void Rename()
        {
            DirSearch();

            if (allFiles.Count > 0)
            {
                for (int i = 0; i < allFiles.Count; i++)
                {
                    int idx = allFiles[i].IndexOf("Assets");
                    string filename = Path.GetFileName(allFiles[i]);
                    string asset = allFiles[i].Substring(idx);
                    AnimationClip orgClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(
                        asset, typeof(AnimationClip));

                    var fileName = Path.GetFileNameWithoutExtension(allFiles[i]);
                    var importer = (ModelImporter)AssetImporter.GetAtPath(asset);

                    RenameAndImport(importer, fileName);
                }
            }
        }

        private static void RenameAndImport(ModelImporter asset, string name)
        {
            ModelImporter modelImporter = asset as ModelImporter;
            ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

            for (int i = 0; i < clipAnimations.Length; i++)
            {
                clipAnimations[i].name = name;
            }

            modelImporter.clipAnimations = clipAnimations;
            modelImporter.SaveAndReimport();
        }

        public static void DirSearch()
        {
            string info = EditorUtility.OpenFolderPanel("Mixamo Directory", Application.dataPath, "Mixamo");
            string[] fileInfo = Directory.GetFiles(info, "*.fbx", SearchOption.AllDirectories);

            allFiles = new List<string>();

            foreach (string file in fileInfo)
            {
                if (file.EndsWith(".fbx"))
                    allFiles.Add(file);
            }
        }
    }
}