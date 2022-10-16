using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class PhenomLogItem : MonoBehaviour
    {
        public static bool altColor;

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

        public string log => data.log;
        public string stackTrace => data.stackTrace;
        public PhenomLogType logType => data.logType;
        public int count => data.count;

        private int logIndex;
        //private int count;

        public void Init(PhenomLog data, ToggleGroup group, int logIndex, bool collapse, bool showTimestamp)
        {
            this.data = data;
            //this.log = log;
            //this.stackTrace = stackTrace;
            //this.logType = logType;

            //gameObject.name = string.Concat("Log_", logIndex);

            logText.SetText(log);
            logText.color = PhenomConsole.GetColorOfLogType(logType, 1f, 1f);

            // bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .1f : .3f, .5f);
            // altColor = !altColor;

            toggle.group = group;

            UpdateInfo(logIndex, collapse, showTimestamp);

            if (collapse)
            {
                bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .1f : .3f, .5f);
                altColor = !altColor;
            }
        }
        
        public void UpdateInfo(int logIndex, bool collapse, bool showTimestamp)
        {
            this.logIndex = logIndex;
            if (showTimestamp)
            {
                // Debug.LogError("Try get index: " + logIndex + ", but count is: " + data.timestamps.Count, gameObject);
                DateTime timestamp = data.timestamps[logIndex];
                logText.SetText( string.Concat("[", timestamp.Hour.ToString("00"), ":",
                    timestamp.Minute.ToString("00"), ":", timestamp.Second.ToString("00"), ":", timestamp.Millisecond.ToString("000"), "] ", log));
            }
            
            badge.SetActive(collapse);

            if (collapse)
            {
                countText.SetText(count.ToBigNumberString());
            }
            else
            {
                bgImage.color = PhenomConsole.GetColorOfLogType(logType, altColor ? .1f : .3f, .5f);
                altColor = !altColor;
            }
        }

        public void OnToggle(bool on)
        {
            PhenomConsole.ToggleStackTrace(stackTrace);
        }
    }
}
