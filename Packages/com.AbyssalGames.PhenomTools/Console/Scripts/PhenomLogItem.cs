using System;
using PhenomTools.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class PhenomLogItem : MonoBehaviour
    {
        private static bool altColor;

        [SerializeField]
        private TextMeshProUGUI logText = null;
        [SerializeField]
        private TextMeshProUGUI countText = null;
        [SerializeField]
        private GameObject badge = null;
        [SerializeField]
        private Image bgImage = null;
        [SerializeField]
        private Toggle toggle = null;

        [NonSerialized]
        public PhenomLog data;

        private string log => data.log;
        private string stackTrace => data.stackTrace;
        private PhenomLogType logType => data.logType;
        private int count => data.count;

        public void Init(PhenomLog data, ToggleGroup group, int logIndex, bool collapse, bool showTimestamp)
        {
            this.data = data;
            
            logText.SetText(log);
            logText.color = PhenomConsole.GetColorOfLogType(logType, 1f, 1f);
            toggle.group = group;

            UpdateInfo(logIndex, collapse, showTimestamp);

            if (collapse)
            {
                bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .2f : .25f, .5f);
                altColor = !altColor;
            }
        }
        
        public void UpdateInfo(int logIndex, bool collapse, bool showTimestamp)
        {
            if (showTimestamp)
            {
                DateTime timestamp = data.timestamps[logIndex];
                logText.SetText( string.Concat("[", timestamp.Hour.ToString("00"), ":",
                    timestamp.Minute.ToString("00"), ":", timestamp.Second.ToString("00"), ":", timestamp.Millisecond.ToString("000"), "] ", log));
            }
            
            badge.SetActive(collapse);

            if (collapse)
            {
                countText.SetText(count.TryFormatAsVeryLargeNumber());
            }
            else
            {
                bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .2f : .25f, .5f);
                altColor = !altColor;
            }
        }

        public void OnToggle(bool on)
        {
            PhenomConsole.ToggleStackTrace(data);
        }
    }
}
