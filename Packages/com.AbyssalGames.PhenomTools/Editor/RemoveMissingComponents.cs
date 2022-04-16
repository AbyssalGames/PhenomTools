using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace PhenomTools
{
    public class RemoveMissingComponents : Editor
    {
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

        [MenuItem("PhenomTools/Remove MissingComponents in Prefabs at Path")]
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
#endif
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//namespace PhenomTools
//{
//    public class FindMissingScriptsRecursivelyAndRemove : EditorWindow
//    {
//        private static int _goCount;
//        private static int _componentsCount;
//        private static int _missingCount;

//        private static bool _bHaveRun;

//        [MenuItem("PhenomTools/FindMissingScriptsRecursivelyAndRemove")]
//        public static void ShowWindow()
//        {
//            GetWindow(typeof(FindMissingScriptsRecursivelyAndRemove));
//        }

//        public void OnGUI()
//        {
//            if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
//            {
//                FindInSelected();
//            }

//            if (!_bHaveRun) return;

//            EditorGUILayout.TextField($"{_goCount} GameObjects Selected");
//            if (_goCount > 0) EditorGUILayout.TextField($"{_componentsCount} Components");
//            if (_goCount > 0) EditorGUILayout.TextField($"{_missingCount} Deleted");
//        }

//        private static void FindInSelected()
//        {
//            var go = Selection.gameObjects;
//            _goCount = 0;
//            _componentsCount = 0;
//            _missingCount = 0;
//            foreach (var g in go)
//            {
//                FindInGo(g);
//            }

//            _bHaveRun = true;
//            Debug.Log($"Searched {_goCount} GameObjects, {_componentsCount} components, found {_missingCount} missing");

//            AssetDatabase.SaveAssets();
//        }

//        private static void FindInGo(GameObject g)
//        {
//            _goCount++;
//            var components = g.GetComponents<Component>();

//            var r = 0;

//            for (var i = 0; i < components.Length; i++)
//            {
//                _componentsCount++;
//                if (components[i] != null) continue;
//                _missingCount++;
//                var s = g.name;
//                var t = g.transform;
//                while (t.parent != null)
//                {
//                    s = t.parent.name + "/" + s;
//                    t = t.parent;
//                }

//                Debug.Log($"{s} has a missing script at {i}", g);

//                var serializedObject = new SerializedObject(g);

//                var prop = serializedObject.FindProperty("m_Component");

//                prop.DeleteArrayElementAtIndex(i - r);
//                r++;

//                serializedObject.ApplyModifiedProperties();
//            }

//            foreach (Transform childT in g.transform)
//            {
//                FindInGo(childT.gameObject);
//            }
//        }
//    }
//}