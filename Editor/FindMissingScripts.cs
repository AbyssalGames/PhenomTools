using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PhenomTools
{
    public class FindMissingScripts : EditorWindow
    {
        static int go_count = 0, components_count = 0, missing_count = 0;
        
        [MenuItem("PhenomTools/Find Missing Scripts")]
        private static void FindInSelected()
        {
            GameObject[] go = Selection.gameObjects;
            int go_count = 0, components_count = 0, missing_count = 0;
            foreach (GameObject g in go)
            {
                go_count++;
                Component[] components = g.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    components_count++;
                    if (components[i] == null)
                    {
                        missing_count++;
                        string s = g.name;
                        Transform t = g.transform;
                        while (t.parent != null)
                        {
                            s = t.parent.name + "/" + s;
                            t = t.parent;
                        }

                        Debug.Log(s + " has an empty script attached in position: " + i, g);
                    }
                }
            }

            Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
        }
        
        [MenuItem("PhenomTools/Find Missing Scripts Recursively")]
        private static void FindInSelectedRecursive()
        {
            GameObject[] go = Selection.gameObjects;
            go_count = 0;
            components_count = 0;
            missing_count = 0;
            foreach (GameObject g in go)
            {
                FindInGO(g);
            }

            Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
        }

        private static void FindInGO(GameObject g)
        {
            go_count++;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                components_count++;
                if (components[i] == null)
                {
                    missing_count++;
                    string s = g.name;
                    Transform t = g.transform;
                    while (t.parent != null)
                    {
                        s = t.parent.name + "/" + s;
                        t = t.parent;
                    }

                    Debug.Log(s + " has an empty script attached in position: " + i, g);
                }
            }

            // Now recurse through each child GO (if there are any):
            foreach (Transform childT in g.transform)
            {
                //Debug.Log("Searching " + childT.name  + " " );
                FindInGO(childT.gameObject);
            }
        }

        [MenuItem("PhenomTools/Remove Missing Scripts Recursively")]
        private static void FindAndRemoveMissingInSelected()
        {
            // EditorUtility.CollectDeepHierarchy does not include inactive children
            var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
                .Select(t => t.gameObject);
            var prefabs = new HashSet<Object>();
            int compCount = 0;
            int goCount = 0;
            foreach (var go in deeperSelection)
            {
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                if (count > 0)
                {
                    if (PrefabUtility.IsPartOfAnyPrefab(go))
                    {
                        RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
                        count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                        // if count == 0 the missing scripts has been removed from prefabs
                        if (count == 0)
                            continue;
                        // if not the missing scripts must be prefab overrides on this instance
                    }

                    Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                    compCount += count;
                    goCount++;
                }
            }

            Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
        }

        // Prefabs can both be nested or variants, so best way to clean all is to go through them all
        // rather than jumping straight to the original prefab source.
        private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
            ref int goCount)
        {
            var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
            // Only visit if source is valid, and hasn't been visited before
            if (source == null || !prefabs.Add(source))
                return;

            // go deep before removing, to differantiate local overrides from missing in source
            RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
                compCount += count;
                goCount++;
            }
        }
        
        /// <summary>
        /// DOES :
        /// Remove missing scripts in prefabs found at PATH, then save prefab.
        /// Saved prefab will have no missing scripts left.
        /// Will not mod prefabs that dont have missing scripts.
        ///
        /// NOTE :
        /// If prefab has another prefab#2 that is not in PATH, that prefab#2 will still have missing scripts.
        /// The instance of the prefab#2 in prefab will not have missing scripts (thus counted has override of prefab#2)
        ///
        /// HOW TO USE :
        /// Copy code in script anywhere in project.
        /// Set the PATH var in method <see cref="RemoveMissingScripstsInPrefabsAtPath"/>.
        /// Clik the button.
        /// </summary>

        [MenuItem("PhenomTools/Remove Missing Components in Prefabs at Path")]
        public static void RemoveMissingScripstsInPrefabsAtPath()
        {
            //string PATH = "Assets/";

            EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);
            string[] ids = AssetDatabase.FindAssets("t:Prefab");//, new string[] { PATH });
            for (int i = 0; i < ids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(ids[i]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                int delCount = 0;
                RecursivelyModifyPrefabChilds(instance, ref delCount);

                if (delCount > 0)
                {
                    Debug.Log($"Removed({delCount}) on {path}", prefab);
                    PrefabUtility.SaveAsPrefabAssetAndConnect(instance, path, InteractionMode.AutomatedAction);
                }

                UnityEngine.Object.DestroyImmediate(instance);
                EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", i / (float)ids.Length);
            }
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        private static void RecursivelyModifyPrefabChilds(GameObject obj, ref int delCount)
        {
            if (obj.transform.childCount > 0)
            {
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    var _childObj = obj.transform.GetChild(i).gameObject;
                    RecursivelyModifyPrefabChilds(_childObj, ref delCount);
                }
            }

            int innerDelCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            delCount += innerDelCount;
        }
    }
}