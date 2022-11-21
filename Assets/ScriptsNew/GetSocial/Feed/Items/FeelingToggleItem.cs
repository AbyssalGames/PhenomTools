using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBoxVR.GetSocial
{
    public class FeelingToggleItem : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle = null;
        [SerializeField]
        private TextMeshProUGUI text = null;

        [HideInInspector]
        public int index;

        private FeelingView feelingView = null;

        public void Initialize(FeelingView feelingView, bool isOn)
        {
            this.feelingView = feelingView;
            toggle.SetIsOnWithoutNotify(isOn);
        }

        public void OnSelect(bool on)
        {
            if (on)
                feelingView.SetFeeling(index);
            else
                feelingView.CheckIfOff();
        }

#if UNITY_EDITOR
        public void SetText(string value)
        {
            text.SetText(value);
        }
#endif
    }
}
