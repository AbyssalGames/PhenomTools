// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace PhenomTools
// {
//     public class FindMissingScriptsRecursively : EditorWindow
//     {
//         static int go_count = 0, components_count = 0, missing_count = 0;
//
//         [MenuItem("PhenomTools/Find Missing Scripts Recursively")]
//         public static void ShowWindow()
//         {
//             GetWindow(typeof(FindMissingScriptsRecursively));
//         }
//
//         public void OnGUI()
//         {
//             if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
//             {
//                 FindInSelected();
//             }
//         }
//
//         private static void FindInSelected()
//         {
//             GameObject[] go = Selection.gameObjects;
//             go_count = 0;
//             components_count = 0;
//             missing_count = 0;
//             foreach (GameObject g in go)
//             {
//                 FindInGO(g);
//             }
//
//             Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
//         }
//
//         private static void FindInGO(GameObject g)
//         {
//             go_count++;
//             Component[] components = g.GetComponents<Component>();
//             for (int i = 0; i < components.Length; i++)
//             {
//                 components_count++;
//                 if (components[i] == null)
//                 {
//                     missing_count++;
//                     string s = g.name;
//                     Transform t = g.transform;
//                     while (t.parent != null)
//                     {
//                         s = t.parent.name + "/" + s;
//                         t = t.parent;
//                     }
//
//                     Debug.Log(s + " has an empty script attached in position: " + i, g);
//                 }
//             }
//
//             // Now recurse through each child GO (if there are any):
//             foreach (Transform childT in g.transform)
//             {
//                 //Debug.Log("Searching " + childT.name  + " " );
//                 FindInGO(childT.gameObject);
//             }
//         }
//
//         [MenuItem("PhenomTools/Remove Missing Scripts Recursively")]
//         private static void FindAndRemoveMissingInSelected()
//         {
//             // EditorUtility.CollectDeepHierarchy does not include inactive children
//             var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
//                 .Select(t => t.gameObject);
//             var prefabs = new HashSet<Object>();
//             int compCount = 0;
//             int goCount = 0;
//             foreach (var go in deeperSelection)
//             {
//                 int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
//                 if (count > 0)
//                 {
//                     if (PrefabUtility.IsPartOfAnyPrefab(go))
//                     {
//                         RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
//                         count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
//                         // if count == 0 the missing scripts has been removed from prefabs
//                         if (count == 0)
//                             continue;
//                         // if not the missing scripts must be prefab overrides on this instance
//                     }
//
//                     Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
//                     GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
//                     compCount += count;
//                     goCount++;
//                 }
//             }
//
//             Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
//         }
//
//         // Prefabs can both be nested or variants, so best way to clean all is to go through them all
//         // rather than jumping straight to the original prefab source.
//         private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
//             ref int goCount)
//         {
//             var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
//             // Only visit if source is valid, and hasn't been visited before
//             if (source == null || !prefabs.Add(source))
//                 return;
//
//             // go deep before removing, to differantiate local overrides from missing in source
//             RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);
//
//             int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
//             if (count > 0)
//             {
//                 Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
//                 GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
//                 compCount += count;
//                 goCount++;
//             }
//         }
//     }
// }