using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BlackBoxVR.GetSocial
{
    public class FeelingView : MonoBehaviour
    {
        public UnityEventInt onFeelingSet;

        [SerializeField]
        private TextMeshProUGUI titleText = null;
        [SerializeField]
        private ToggleGroup toggleGroup = null;
        [SerializeField]
        private FeelingToggleItem[] toggles = null;

        private int currentFeeling;

        public void Initialize(int currentFeeling)
        {
            this.currentFeeling = currentFeeling;

            for (int i = 0; i < toggles.Length; i++)
                toggles[i].Initialize(this, currentFeeling == i);

            UpdateTitleText();
        }

        public void SetFeeling(int index)
        {
            currentFeeling = index;
            UpdateTitleText();
        }

        public void CheckIfOff()
        {
            if (!toggleGroup.AnyTogglesOn())
                currentFeeling = -1;
        }

        private void UpdateTitleText()
        {
            string newText = "";

            if (currentFeeling >= 0)
                newText += "Feeling... <size=12>" + GetSocialManager.feelingData[currentFeeling].emoji + "</size> <b>" + GetSocialManager.feelingData[currentFeeling].key + "</b>";

            //TODO add mentions preview here

            titleText.SetText(newText);
        }

        public void OnDone()
        {
            onFeelingSet?.Invoke(currentFeeling);
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        [ContextMenu("Prepare Toggles")]
        public void PrepareToggles()
        {
            if (toggles.Length > 0)
            {
                for (int i = 0; i < toggles.Length; i++)
                {
                    toggles[i].index = i;
                    toggles[i].SetText(GetSocialManager.feelingData[i].emoji + " " + GetSocialManager.feelingData[i].key);
                }
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}
