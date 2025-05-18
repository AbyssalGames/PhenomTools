using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PhenomTools.Utility;
using Object = UnityEngine.Object;

namespace PhenomTools
{
    public enum PhenomLogType
    {
        Error, Assertion, Warning, Normal, Boon, Count
    }

    [Serializable]
    public class PhenomLog 
    {
        public string log;
        public PhenomLogType logType;
        public string stackTrace;
        public int count = 1;

        public List<DateTime> timestamps = new List<DateTime>();
        public List<PhenomLogItem> logItems = new List<PhenomLogItem>();

        public PhenomLog(string log, PhenomLogType logType, string stackTrace)
        {
            this.log = log;
            this.logType = logType;
            this.stackTrace = stackTrace;
            
            timestamps.Add(DateTime.Now);
        }
        
        public override bool Equals(object obj)
        {
            PhenomLog other = obj as PhenomLog;
            return log == other.log && logType == other.logType;
        }

        public override int GetHashCode()
        {
            return log.GetHashCode() ^ logType.GetHashCode();
        }
    }

    /// <summary>
    /// Custom Run-time logger
    /// </summary>
    public class PhenomConsole : MonoBehaviour
    {
        public static bool isDebugMode { get; private set; }

        private static PhenomConsole instance;
        private static bool isEnabled;
        private static string fileName;

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private RectTransform consoleTransform;
        [SerializeField]
        private CanvasGroup consoleCanvasGroup;
        [SerializeField]
        private CanvasGroup showButtonCanvasGroup;
        [SerializeField]
        private Toggle collapseToggle;
        [SerializeField]
        private Transform logRoot;
        [SerializeField]
        private RectTransform logsRect;
        [SerializeField]
        private GameObject stackTraceObject;
        [SerializeField]
        private RectTransform stackTraceRect;
        [SerializeField]
        private TextMeshProUGUI stackTraceText;
        [SerializeField]
        private ToggleGroup toggleGroup;
        [SerializeField]
        private PhenomLogItem logItemPrefab;
        [SerializeField]
        private GameObject openLogFileButton;
        [SerializeField]
        private GameObject copyButton;
        [SerializeField]
        private int maxLogItems = 100;
        [SerializeField]
        private TextMeshProUGUI[] logTypeCountTexts;
        [SerializeField]
        private Toggle[] logTypeToggles;

        [Space]
        [SerializeField]
        private bool enableOnAwake;
        [SerializeField]
        private bool collapse = true;
        [SerializeField]
        private bool listenForSystemLogs;
        [SerializeField]
        private bool useTimestamp = true;
        [SerializeField]
        private bool writeLogFile = true;

        private List<PhenomLog> logs = new();
        private List<PhenomLogItem> logItems = new();
        
        private int currentLogItemCount;
        private int currentLogCount;
        private Vector3 consoleOriginalPos;
        private PhenomLog selectedLog;
        private int[] logTypeCounts = new int[(int)PhenomLogType.Count];
        private bool[] logTypeDisplayEnabled = { true, true, true, true, true };

        private void Awake()
        {
            if (!Application.isBatchMode && instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            if (writeLogFile)
            {
                fileName = Application.persistentDataPath + "/PhenomLog_" + DateTime.UtcNow.ToString("MM-dd_HH-mm-ss") + ".txt";
                File.Create(fileName);

#if UNITY_EDITOR || UNITY_STANDALONE
                openLogFileButton.SetActive(true);
#endif
            }
            
#if UNITY_ASSERTIONS
            logTypeToggles[(int)PhenomLogType.Assertion].gameObject.SetActive(true);
#else
            logTypeToggles[(int)PhenomLogType.Assertion].gameObject.SetActive(false);
#endif

            collapse = PlayerPrefs.GetInt("PC_Collapse", 0).ToBoolBinary();
            collapseToggle.SetIsOnWithoutNotify(collapse);

            for (int i = 0; i < logTypeDisplayEnabled.Length; i++)
            {
                logTypeDisplayEnabled[i] = PlayerPrefs.GetInt("PC_Type_" + i, 1).ToBoolBinary();
                logTypeToggles[i].SetIsOnWithoutNotify(logTypeDisplayEnabled[i]);
            }

            if (enableOnAwake)
                ToggleDebugMode(true);
            else
                instance.canvas.enabled = false;
        }

        public static void ToggleDebugMode(bool on)
        {
            isDebugMode = on;
            instance.canvas.enabled = on;

            if (instance.listenForSystemLogs)
            { 
                if (on)
                    Application.logMessageReceived += instance.SystemLogReceived;
                else
                    Application.logMessageReceived -= instance.SystemLogReceived;
            }
        }

        private void SystemLogReceived(string log, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception)
                logType = LogType.Error;

            if (instance != null)
                instance.NewLine(log, (PhenomLogType)logType, stackTrace);
        }

        public static void Log(object log, PhenomLogType logType = PhenomLogType.Normal, Object context = null, string stackTrace = null)
        {
            if (logType == PhenomLogType.Normal)
                Debug.Log(log, context);
            else if (logType == PhenomLogType.Warning)
                Debug.LogWarning(log, context);
            else if (logType == PhenomLogType.Error)
                Debug.LogError(log, context);
            else if (logType == PhenomLogType.Assertion)
                Debug.LogAssertion(log, context);
            else if (logType == PhenomLogType.Boon) 
                Debug.Log(log, context);

            if (instance != null && !instance.listenForSystemLogs)
                instance.NewLine(log.ToString(), logType, stackTrace);
        }
        public static void LogError(object log, Object context = null, string stackTrace = null)
        {
            Log(log, PhenomLogType.Error, context, stackTrace);
        }
        public static void LogWarning(object log, Object context = null, string stackTrace = null)
        {
            Log(log, PhenomLogType.Warning, context, stackTrace);
        }
        public static void LogAssertion(object log, Object context = null, string stackTrace = null)
        {
            Log(log, PhenomLogType.Assertion, context, stackTrace);
        }
        public static void LogBoon(object log, Object context = null, string stackTrace = null)
        {
            Log(log, PhenomLogType.Boon, context, stackTrace);
        }

        private void NewLine(string logText, PhenomLogType logType, string stackTrace)
        {
            currentLogCount++;
            logTypeCounts[(int)logType]++;
            logTypeCountTexts[(int)logType].SetText(logTypeCounts[(int)logType].TryFormatAsVeryLargeNumber());
            
            if (logs.AnyOut(l => l.log == logText && l.logType == logType, out PhenomLog log))
            {
                log.count++;
                log.timestamps.Add(DateTime.Now);
                
                if (!logTypeDisplayEnabled[(int)logType])
                    return;
                
                if (collapse)
                {
                    if (log.logItems.Count == 1 && log.logItems[0] != null)
                        log.logItems[0].UpdateInfo(log.count - 1, collapse, useTimestamp);
                    else
                        CreateNewLogItem(log);
                }
                else
                {
                    CreateNewLogItem(log);
                }
            }
            else
            {
                log = new PhenomLog(logText, logType, stackTrace);
                logs.Add(log);
                
                if (logTypeDisplayEnabled[(int)logType])
                    CreateNewLogItem(log);
            }
            
            if(writeLogFile && logTypeDisplayEnabled[(int)logType])
                instance.WriteToFile(log);
        }

        private void CreateNewLogItem(PhenomLog log)
        {
            if (currentLogItemCount == maxLogItems)
            {
                logItems[0].data.logItems.RemoveAt(0);
                Destroy(logItems[0].gameObject);
                logItems.RemoveAt(0);
                currentLogItemCount--;
            }
            
            PhenomLogItem newItem = Instantiate(logItemPrefab, logRoot);
            newItem.Init(log, toggleGroup,  Math.Max(0, log.count - 1), collapse, useTimestamp);
            log.logItems.Add(newItem);
            logItems.Add(newItem);
            currentLogItemCount++;
        }

        public static void ToggleStackTrace(PhenomLog log)
        {
            if (instance == null || !instance.listenForSystemLogs || string.IsNullOrWhiteSpace(log.stackTrace))
                return;

            instance.stackTraceText.SetText(log.log + "\r\n" + log.stackTrace);

            if (instance.toggleGroup.AnyTogglesOn())
            {
                instance.selectedLog = log;
                instance.logsRect.anchorMin = new Vector2(0f, instance.stackTraceRect.anchorMax.y);
                instance.stackTraceObject.SetActive(true);
                instance.copyButton.SetActive(true);
            }
            else
            {
                instance.selectedLog = null;
                instance.logsRect.anchorMin = new Vector2(0f, 0f);
                instance.stackTraceObject.SetActive(false);
                instance.copyButton.SetActive(false);
            }
        }

        public void WriteToFile(PhenomLog log)
        {
            using (StreamWriter writer = new(fileName, true))
                writer.WriteLine(GetFormattedLogString(log));
        }
        
        public void OpenLogFile()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(fileName);
#elif UNITY_STANDALONE
            Application.OpenURL(fileName);
#endif
        }

        public void CopyToClipboard()
        {
            GUIUtility.systemCopyBuffer = GetFormattedLogString(selectedLog);
        }

        public static string GetFormattedLogString(PhenomLog log)
        {
            DateTime timestamp = log.timestamps[log.timestamps.Count - 1];
            string logType = log.logType == PhenomLogType.Normal ? "" : "[" + log.logType + "] ";
            
            return string.Concat("[", timestamp.Hour.ToString("00"), ":",
                timestamp.Minute.ToString("00"), ":", timestamp.Second.ToString("00"), ":", timestamp.Millisecond.ToString("000"), "] ", 
                logType,
                log.log,
                "\n",
                log.stackTrace);//.Replace("\n", "\n    "));
        }

        public void ToggleConsoleDisplay(bool on)
        {
            DOTween.Kill(consoleCanvasGroup);
            DOTween.Kill(consoleTransform);
            DOTween.Kill(showButtonCanvasGroup);

            if (on)
            {
                DOTween.To(() => consoleCanvasGroup.alpha, a => consoleCanvasGroup.alpha = a, 1f, .25f);
                consoleCanvasGroup.blocksRaycasts = true;
                consoleCanvasGroup.interactable = true;

                DOTween.To(() => showButtonCanvasGroup.alpha, a => showButtonCanvasGroup.alpha = a, 0f, .25f);
                showButtonCanvasGroup.blocksRaycasts = false;
                showButtonCanvasGroup.interactable = false;

                consoleOriginalPos = consoleTransform.localPosition;
                consoleTransform.DOLocalMove(Vector3.zero, .25f);
            }
            else
            {
                DOTween.To(() => consoleCanvasGroup.alpha, a => consoleCanvasGroup.alpha = a, 0f, .25f);
                consoleCanvasGroup.blocksRaycasts = false;
                consoleCanvasGroup.interactable = false;

                DOTween.To(() => showButtonCanvasGroup.alpha, a => showButtonCanvasGroup.alpha = a, 1f, .25f);
                showButtonCanvasGroup.blocksRaycasts = true;
                showButtonCanvasGroup.interactable = true;

                consoleTransform.DOLocalMove(consoleOriginalPos, .25f);
            }
        }

        public void Clear()
        {
            currentLogItemCount = 0;
            currentLogCount = 0;

            for (int i = 0; i < (int)PhenomLogType.Count; i++)
            {
                logTypeCounts[i] = 0;
                logTypeCountTexts[i].SetText("0");
            }

            for (int i = 0; i < logItems.Count; i++)
                Destroy(logItems[i].gameObject);
            
            instance.selectedLog = null;
            instance.logsRect.anchorMin = new Vector2(0f, 0f);
            instance.stackTraceObject.SetActive(false);
            instance.copyButton.SetActive(false);
            
            logItems.Clear();
            logs.Clear();
        }

        public void ToggleCollapse(bool on)
        {
            collapse = on;
            PlayerPrefs.SetInt("PC_Collapse", collapse.ToIntBinary());
            Rebuild();
        }

        public void ToggleLogTypeDisplay(int i)
        {
            logTypeDisplayEnabled[i] = !logTypeDisplayEnabled[i];
            PlayerPrefs.SetInt("PC_Type_" + i, logTypeDisplayEnabled[i].ToIntBinary());
            Rebuild();
        }

        private void Rebuild()
        {
            currentLogItemCount = 0;
            logItems.Clear();
            
            if (collapse)
            {
                foreach(PhenomLog log in logs)
                {
                    for (int j = log.logItems.Count - 1; j >= 0; j--) 
                    {
                        Destroy(log.logItems[j].gameObject);
                        log.logItems.RemoveAt(j);
                    }

                    if (logTypeDisplayEnabled[(int)log.logType])
                        CreateNewLogItem(log);
                }
            }
            else
            {
                List<KeyValuePair<DateTime, PhenomLog>> allLogs = new();

                foreach (PhenomLog log in logs)
                {
                    foreach (DateTime timestamp in log.timestamps)
                        allLogs.Add(new KeyValuePair<DateTime, PhenomLog>(timestamp, log));

                    for (int i = 0; i < log.logItems.Count; i++)
                        Destroy(log.logItems[i].gameObject);

                    log.logItems.Clear();
                }

                allLogs = allLogs.OrderBy(l => l.Key).ToList();

                foreach (KeyValuePair<DateTime, PhenomLog> kvp in allLogs)
                {
                    PhenomLog log = kvp.Value;

                    if (logTypeDisplayEnabled[(int)log.logType])
                        CreateNewLogItem(log);
                }
            }
        }
        
        public static Color GetColorOfLogType(PhenomLogType type, float value, float alpha)
        {
            switch (type)
            {
                default:
                case PhenomLogType.Normal:
                    return new Color(1 * value, 1 * value, 1 * value, alpha);
                case PhenomLogType.Assertion:
                    return new Color(1 * value, .5f * value, 0 * value, alpha);
                case PhenomLogType.Warning:
                    return new Color(1 * value, .92f * value, .016f * value, alpha);
                case PhenomLogType.Error:
                    return new Color(1 * value, 0 * value, 0 * value, alpha);
                case PhenomLogType.Boon:
                    return new Color(0 * value, 1 * value, 0 * value, alpha);
            }
        }

        public static string GetTypePrefix(PhenomLogType type)
        {
            switch (type)
            {
                default:
                    return "";
                case PhenomLogType.Assertion:
                    return "Assertion: ";
                case PhenomLogType.Boon:
                    return "Boon: ";
                case PhenomLogType.Error:
                    return "Error: ";
                case PhenomLogType.Warning:
                    return "Warning: ";
            }
        }
    }
}