using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class PhenomConsoleLogItem : MonoBehaviour
    {
        public static bool altColor;

        public string log;
        public string stackTrace;

        [SerializeField]
        private TextMeshProUGUI logText = null;
        [SerializeField]
        private TextMeshProUGUI countText = null;
        [SerializeField]
        private Image bgImage = null;
        [SerializeField]
        private Toggle toggle = null;

        private int logIndex;
        private int count;

        public void Init(string log, PhenomLogType logType, int logIndex, string stackTrace, ToggleGroup group)
        {
            this.log = log;
            this.stackTrace = stackTrace;
            this.logIndex = logIndex;

            gameObject.name = string.Concat("Log_", logIndex);

            logText.text = log;
            logText.color = PhenomConsole.GetColorOfLogType(logType, 1f, 1f);

            bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .1f : .3f, .5f);
            altColor = !altColor;

            toggle.group = group;

            IncrementCountBadge();
        }

        public void IncrementCountBadge()
        {
            count++;
            countText.SetText(count.ToString());
        }

        public void OnToggle(bool on)
        {
            PhenomConsole.ToggleStackTrace(stackTrace);
        }
    }
}
