using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PhenomTools
{
    public class TakeScreenshot : Editor
    {
        //[ContextMenu("Take Screenshot")]
        [MenuItem("PhenomTools/Take Screenshot/Normal Resolution")]
        public static void DoScreenshot()
        {
            string fileName = EditorUtility.SaveFilePanel("Take Screenshot", Application.dataPath, "Screenshot", "png");
            ScreenCapture.CaptureScreenshot(fileName);

            AssetDatabase.Refresh();
        }

        [MenuItem("PhenomTools/Take Screenshot/Super Resolution")]
        public static void DoSuperScreenshot()
        {
            string fileName = EditorUtility.SaveFilePanel("Take Screenshot", Application.dataPath, "Screenshot", "png");
            ScreenCapture.CaptureScreenshot(fileName, 4);

            AssetDatabase.Refresh();
        }
    }
}
