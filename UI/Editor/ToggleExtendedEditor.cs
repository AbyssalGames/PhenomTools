using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    [CustomEditor(typeof(ToggleExtended), true)]
    [CanEditMultipleObjects]

    public class ToggleExtendedEditor : SelectableEditor
    {
        private SerializedProperty m_OnValueChangedProperty;
        private SerializedProperty m_TransitionProperty;
        private SerializedProperty m_GraphicsProperty;
        private SerializedProperty m_GroupProperty;
        private SerializedProperty m_IsOnProperty;
        private SerializedProperty longPressDurationProperty;
        private SerializedProperty vibrateOnLongPressProperty;

        private SerializedProperty onHoverProperty;
        private SerializedProperty onDownProperty;
        private SerializedProperty onUpProperty;
        private SerializedProperty onExitProperty;
        private SerializedProperty onReenterProperty;
        private SerializedProperty onLongPressProperty;
        private SerializedProperty onGhostToggleProperty;

#if PhenomAudio
        private SerializedProperty onHoverSoundProperty;
        private SerializedProperty onDownSoundProperty;
        private SerializedProperty onClickSoundProperty;
        private SerializedProperty onToggleOnSoundProperty;
        private SerializedProperty onToggleOffSoundProperty;
#endif

        private bool eventsFoldout;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
            m_GraphicsProperty = serializedObject.FindProperty("graphics");
            m_GroupProperty = serializedObject.FindProperty("m_Group");
            m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
            m_OnValueChangedProperty = serializedObject.FindProperty("onValueChanged");
            longPressDurationProperty = serializedObject.FindProperty("longPressDuration");
            vibrateOnLongPressProperty = serializedObject.FindProperty("vibrateOnLongPress");

            onHoverProperty = serializedObject.FindProperty("onHover");
            onDownProperty = serializedObject.FindProperty("onDown");
            onUpProperty = serializedObject.FindProperty("onUp");
            onExitProperty = serializedObject.FindProperty("onExit");
            onReenterProperty = serializedObject.FindProperty("onReenter");
            onLongPressProperty = serializedObject.FindProperty("onLongPress");
            onGhostToggleProperty = serializedObject.FindProperty("onGhostToggle");

#if PhenomAudio
            onHoverSoundProperty = serializedObject.FindProperty("hoverSound");
            onDownSoundProperty = serializedObject.FindProperty("downSound");
            onClickSoundProperty = serializedObject.FindProperty("clickSound");
            onToggleOnSoundProperty = serializedObject.FindProperty("toggleOnSound");
            onToggleOffSoundProperty = serializedObject.FindProperty("toggleOffSound");
#endif
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            Toggle toggle = serializedObject.targetObject as Toggle;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_IsOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);

                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;

                toggle.isOn = m_IsOnProperty.boolValue;

                if (group != null && group.isActiveAndEnabled && toggle.IsActive())
                {
                    if (toggle.isOn || (!group.AnyTogglesOn() && !group.allowSwitchOff))
                    {
                        toggle.isOn = true;
                        group.NotifyToggleOn(toggle);
                    }
                }
            }
            EditorGUILayout.PropertyField(m_TransitionProperty);
            EditorGUILayout.PropertyField(m_GraphicsProperty);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_GroupProperty);
            
            if ((target as ToggleExtended).onLongPress.GetPersistentEventCount() > 0)
            {
                EditorGUILayout.PropertyField(longPressDurationProperty);
                EditorGUILayout.PropertyField(vibrateOnLongPressProperty);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);

                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;
                toggle.group = group;
            }

            EditorGUILayout.Space();

            eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events");

            if (eventsFoldout)
            {
                EditorGUILayout.PropertyField(m_OnValueChangedProperty);

                EditorGUILayout.PropertyField(onHoverProperty);
                EditorGUILayout.PropertyField(onDownProperty);
                EditorGUILayout.PropertyField(onUpProperty);
                EditorGUILayout.PropertyField(onExitProperty);
                EditorGUILayout.PropertyField(onReenterProperty);
                EditorGUILayout.PropertyField(onLongPressProperty);
                EditorGUILayout.PropertyField(onGhostToggleProperty);

#if PhenomAudio
                EditorGUILayout.PropertyField(onHoverSoundProperty);
                EditorGUILayout.PropertyField(onDownSoundProperty);
                EditorGUILayout.PropertyField(onClickSoundProperty);
                EditorGUILayout.PropertyField(onToggleOnSoundProperty);
                EditorGUILayout.PropertyField(onToggleOffSoundProperty);
#endif
            }

            serializedObject.ApplyModifiedProperties();
        }

        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //}

        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();

        //    serializedObject.Update();

        //    serializedObject.ApplyModifiedProperties();
        //}

        //[MenuItem("GameObject/UI/Toggle Extended", priority = 2032)]
        //public static void Create(MenuCommand menuCommand)
        //{
        //    GameObject newGo = new GameObject("New Toggle");
        //    newGo.transform.SetParent((menuCommand.context as GameObject).transform);
        //    newGo.transform.localPosition = Vector3.zero;
        //    newGo.transform.localRotation = Quaternion.identity;

        //    Image graphic = newGo.AddComponent<Image>();
        //    ToggleExtended toggle = newGo.AddComponent<ToggleExtended>();
        //    toggle.targetGraphic = graphic;
        //}
    }
}
