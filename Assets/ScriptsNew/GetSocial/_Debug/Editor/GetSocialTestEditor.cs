using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PhenomTools;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    [CustomEditor(typeof(GetSocialTest))]
//[CanEditMultipleObjects]
    public class GetSocialTestEditor : Editor
    {
        private GetSocialTest master;
        private FeedView feedView;
        //SerializedProperty value;

        public void OnEnable()
        {
            master = target as GetSocialTest;
            //value = serializedObject.FindProperty("Value");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //base.OnInspectorGUI();

            if (Application.isPlaying && GetSocialSdk.Core.GetSocial.IsInitialized)
            {
                master.dummiesToCreate = EditorGUILayout.IntField("Dummies To Create", master.dummiesToCreate);
                if (GUILayout.Button("Create Dummies")) master.GenerateRandomDummies();

                EditorGUILayout.Space();

                if (GUILayout.Button("Create Dummy Custom Post")) master.PostDummyCustom();
                //if (GUILayout.Button("Create Dummy Streak Post")) master.PostDummyStreak();
                //if (GUILayout.Button("Create Dummy Battle Log Post")) master.PostDummyBattleLog();
                //if (GUILayout.Button("Create Dummy Hero Level Up Post")) master.PostDummyHeroLevelUp();
                //if (GUILayout.Button("Create Dummy Advance League Post")) master.PostDummyAdvanceLeague();
                //if (GUILayout.Button("Create Dummy Arena Level Up Post")) master.PostDummyArenaLevelUp();
                //if (GUILayout.Button("Create Dummy Boss Defeated Post")) master.PostDummyBossDefeated();
                //if (GUILayout.Button("Create Dummy Duration Quest Post")) master.PostDummyDurationQuest();
                //if (GUILayout.Button("Create Dummy Achievement Post")) master.PostDummyAchievement();
                if (GUILayout.Button("Create Dummy Profile Pic Post")) master.PostDummyNewProfilePic();

                EditorGUILayout.Space();

                if (GUILayout.Button("Open Global Feed")) master.OpenGlobalFeed();

                if (feedView == null)
                    feedView = FindObjectOfType<FeedView>();
                else if (GUILayout.Button("Select FeedView in Hierarchy"))
                    Selection.activeGameObject = feedView.transform.FindDeepChild("Content").gameObject;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}