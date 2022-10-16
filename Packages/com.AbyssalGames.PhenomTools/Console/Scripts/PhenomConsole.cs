using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Object = UnityEngine.Object;
using System.Collections.Specialized;
using System.Collections;

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
        private Canvas canvas = null;
        [SerializeField]
        private RectTransform consoleTransform = null;
        [SerializeField]
        private CanvasGroup consoleCanvasGroup = null;
        [SerializeField]
        private CanvasGroup showButtonCanvasGroup = null;
        [SerializeField]
        private Toggle collapseToggle = null;
        [SerializeField]
        private Transform logRoot = null;
        [SerializeField]
        private RectTransform logsRect = null;
        [SerializeField]
        private GameObject stackTraceObject = null;
        [SerializeField]
        private RectTransform stackTraceRect = null;
        [SerializeField]
        private TextMeshProUGUI stackTraceText = null;
        [SerializeField]
        private ToggleGroup toggleGroup = null;
        [SerializeField]
        private PhenomLogItem logItemPrefab = null;
        //[SerializeField]
        //private GameObject openLogFileButton = null;
        [SerializeField]
        private int maxLogItems = 100;
        [SerializeField]
        private TextMeshProUGUI[] logTypeCountTexts = null;
        [SerializeField]
        private Toggle[] logTypeToggles = null;

        [Space]
        [SerializeField]
        private bool enableOnAwake = false;
        [SerializeField]
        private bool collapse = true;
        [SerializeField]
        private bool listenForSystemLogs = false;
        
        [Space]
        [SerializeField]
        private bool useTimestamp = true;
        [SerializeField]
        private bool useConsoleWriteLine = false;

        [SerializeField]
        private List<PhenomLog> logs = new List<PhenomLog>();
        [SerializeField]
        private List<PhenomLogItem> logItems = new List<PhenomLogItem>();
        // private OrderedDictionary<string, PhenomLog> logDict = new OrderedDictionary<string, PhenomLog>();
        //private OrderedDictionary<PhenomLog, PhenomLogItem> itemDict = new OrderedDictionary<PhenomLog, PhenomLogItem>();
        //private Dictionary<string, PhenomConsoleLogItem> logDict = new Dictionary<string, PhenomConsoleLogItem>();
        //private List<PhenomConsoleLogItem> logItems = new List<PhenomConsoleLogItem>();
        private int currentLogItemCount;
        private int totalLogCount;
        private Vector3 consoleOriginalPos;
        private int[] logTypeCounts = new int[(int)PhenomLogType.Count];
        private bool[] logTypeDisplayEnabled = new bool[(int)PhenomLogType.Count] { true, true, true, true, true };

        private void Awake()
        {
            if (!Application.isBatchMode && instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

#if !UNITY_EDITOR
                fileName = Application.persistentDataPath + "/PhenomLog_" + DateTime.UtcNow.ToString("MM-dd_HH-mm-ss") + ".txt";
                File.Create(fileName);
//#else
                //openLogFileButton.SetActive(false);
#endif
            }
            else
            {
                Destroy(gameObject);
                return;
            }

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
            {
                instance.NewLine(log, (PhenomLogType)logType, stackTrace);
#if !UNITY_EDITOR
                instance.WriteToFile(log);
#endif
            }
        }

        public static void Log(object log, PhenomLogType logType = PhenomLogType.Normal, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false)
        {
            if (useTypePrefix || (instance != null && instance.useConsoleWriteLine == true))
                log = string.Concat(GetTypePrefix(logType), log);

            // if (useTimestamp && instance != null && instance.useTimestamp == true)
            // {
            //     DateTime now = DateTime.Now;
            //     log = string.Concat(now.Hour.ToString("00"), ":", now.Minute.ToString("00"), ":", now.Second.ToString("00"), ":", now.Millisecond.ToString("000"), " : ", log);
            // }

            switch (logType)
            {
                case PhenomLogType.Normal:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.Log(log, context);
                    break;
                case PhenomLogType.Warning:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogWarning(log, context);
                    break;
                case PhenomLogType.Error:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogError(log, context);
                    break;
                case PhenomLogType.Assertion:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogAssertion(log, context);
                    break;
                case PhenomLogType.Boon:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.Log(log, context);
                    break;
            }

            if (instance != null && !instance.listenForSystemLogs)
            {
                instance.NewLine(log.ToString(), logType, stackTrace);
#if !UNITY_EDITOR
                instance.WriteToFile(log.ToString());
#endif
            }
        }
        public static void LogError(object log, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false)
        {
            Log(log, PhenomLogType.Error, context, stackTrace, useTimestamp, useTypePrefix);
        }
        public static void LogWarning(object log, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false)
        {
            Log(log, PhenomLogType.Warning, context, stackTrace, useTimestamp, useTypePrefix);
        }
        public static void LogAssertion(object log, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false)
        {
            Log(log, PhenomLogType.Assertion, context, stackTrace, useTimestamp, useTypePrefix);
        }
        public static void LogBoon(object log, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false)
        {
            Log(log, PhenomLogType.Boon, context, stackTrace, useTimestamp, useTypePrefix);
        }

        private void NewLine(string logText, PhenomLogType logType, string stackTrace)
        {
            totalLogCount++;
            logTypeCounts[(int)logType]++;
            logTypeCountTexts[(int)logType].SetText(logTypeCounts[(int)logType].ToBigNumberString());

            if (!logTypeDisplayEnabled[(int)logType])
                return;
            

            //PhenomLog l = logs.First(l => l.log == log && l.logType == logType);
            if (logs.AnyOut(l => l.log == logText && l.logType == logType, out PhenomLog log))
            {
                log.count++;
                log.timestamps.Add(DateTime.Now);
                // logs.RemoveAt(logs.IndexOf(data));
                // logs.Add(data);
                
                if (collapse)
                {
                    if (log.logItems.Count == 1 && log.logItems[0] != null)
                    {
                        log.logItems[0].UpdateInfo(log.count - 1, collapse, useTimestamp);
                        // data.logItems[0].transform.SetAsLastSibling();
                    }
                    else
                    {
                        CreateNewLogItem(log);
                        // log.logItems.Add(Instantiate(logItemPrefab, logRoot).Init(log, toggleGroup, log.count - 1, collapse, useTimestamp));
                    }
                }
                else
                {
                    CreateNewLogItem(log);
                    // log.logItems.Add(Instantiate(logItemPrefab, logRoot).Init(log, toggleGroup, log.count - 1, collapse, useTimestamp));
                }
            }
            else
            {
                log = new PhenomLog(logText, logType, stackTrace);
                CreateNewLogItem(log);
                // newLog.logItems.Add(Instantiate(logItemPrefab, logRoot).Init(newLog, toggleGroup, newLog.count - 1, collapse, useTimestamp));
                logs.Add(log);
            }

            //if(logDict.TryGetValue(log, out PhenomLog data))
            //{
            //    data.count++;
            //    if(itemDict.TryGetValue(data, out PhenomLogItem item) && item != null)
            //    {

            //    }
            //    else
            //    {

            //    }

            //    if (collapse)
            //    {

            //    }
            //}
            //else
            //{

            //}

            //LogData testData = new LogData(log, logType, stackTrace);

            //if (logDict.TryGetValue(testData, out PhenomConsoleLogItem item))
            //{
            //    LogData data = logDict.

            //    if(collapse)
            //}

            //if (collapse && logDict.TryGetValue(data, out PhenomConsoleLogItem item) /*&& item.logType == logType*/)
            //{
            //    IncrementLogCountBadge(item);
            //}
            //else
            //{
            //    if (currentLogItemCount >= maxLines)
            //    {
            //        //if (collapse)
            //        //    logDict.Remove(logItems[0].log);

            //        Destroy(logDict.GetAt(0).gameObject);
            //        //logDict.RemoveAt(0);
            //    }

            //    CreateNewLogObject(log, logType, stackTrace);
            //}
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

        //private void CreateNewLogObject(string log, PhenomLogType logType, string stackTrace)
        //{
        //    currentLogItemCount++;
        //    PhenomLog data = new PhenomLog(log, logType, stackTrace);
        //    PhenomLogItem newLogItem = Instantiate(logItemPrefab, logRoot);
        //    newLogItem.Init(data, toggleGroup);

        //    //if (collapse)
        //        logDict.Add(data, newLogItem);
        //    //else
        //    //    logItems.Add(newLogItem);

        //    //if (collapse)
        //    //    logDict.Add(log, newLogItem);
        //}

        //private void IncrementLogCountBadge(PhenomConsoleLogItem item)
        //{
        //    item.IncrementCountBadge();
        //}

        //        public void OpenLogFile()
        //        {
        //#if !UNITY_EDITOR
        //            Application.OpenURL(Application.persistentDataPath + "/PhenomLog_" + DateTime.UtcNow.ToString("MM-dd_HH-mm-ss") + ".txt");
        //#endif
        //        }

        public static void ToggleStackTrace(string text)
        {
            if (instance == null || !instance.listenForSystemLogs || string.IsNullOrWhiteSpace(text))
                return;

            instance.stackTraceText.SetText(text);

            if (instance.toggleGroup.AnyTogglesOn())
            {
                instance.logsRect.anchorMin = new Vector2(0f, instance.stackTraceRect.anchorMax.y);
                instance.stackTraceObject.SetActive(true);
            }
            else
            {
                instance.logsRect.anchorMin = new Vector2(0f, 0f);
                instance.stackTraceObject.SetActive(false);
            }
        }

#if !UNITY_EDITOR
    public void WriteToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(fileName, true))
            writer.WriteLine(text);
    }
#endif

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

        //private Vector3 showButtonOriginalPos;
        //private Vector3 showButtonOffsetPos;

        public void ToggleConsoleDisplay(bool on)
        {
            //if (showButtonOffsetPos == Vector3.zero)
            //    showButtonOffsetPos = showButtonTransform.localPosition - Vector3.right * 200f;

            DOTween.Kill(consoleCanvasGroup);
            DOTween.Kill(consoleTransform);
            DOTween.Kill(showButtonCanvasGroup);
            //DOTween.Kill(showButtonTransform);

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

                //showButtonOriginalPos = showButtonTransform.localPosition;
                //showButtonTransform.DOLocalMove(showButtonOffsetPos, .5f);
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
                //showButtonTransform.DOLocalMove(showButtonOriginalPos, .5f);
            }
        }

        public void Clear()
        {
            //for (int i = 0; i < logDict.Count; i++)
            //    Destroy(logDict.GetAt(i).gameObject);

            //logItems.Clear();
            // logDict.Clear();

            currentLogItemCount = 0;
            totalLogCount = 0;

            for (int i = 0; i < (int)PhenomLogType.Count; i++)
                logTypeCounts[i] = 0;
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

        public void Rebuild()
        {
            currentLogItemCount = 0;
            logItems.Clear();
            
            if (collapse)
            {
                // for (int i = 0; i < logs.Count; i++) 
                // for (int i = logs.Count - 1; i >= 0; i--) 
                foreach(PhenomLog log in logs)
                {
                    // PhenomLog log = logs[i];
                    for (int j = log.logItems.Count - 1; j >= 0; j--) 
                    {
                        Destroy(log.logItems[j].gameObject);
                        log.logItems.RemoveAt(j);
                    }

                    if (logTypeDisplayEnabled[(int)log.logType])
                    {
                        // if(log.logItems.Count > 0)
                        //     log.logItems[log.count - 1].UpdateInfo(log.count - 1, collapse, useTimestamp);
                        // else
                        CreateNewLogItem(log);
                            // log.logItems.Add(Instantiate(logItemPrefab, logRoot).Init(log, toggleGroup, log.count - 1, collapse, useTimestamp));
                        
                        // for (int j = log.logItems.Count - 2; j >= 0; j--) 
                        // {
                        //     Destroy(log.logItems[j].gameObject);
                        //     log.logItems.RemoveAt(j);
                        // }
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                List<KeyValuePair<DateTime, PhenomLog>> allLogs = new List<KeyValuePair<DateTime, PhenomLog>>();

                foreach (PhenomLog log in logs)
                {
                    foreach (DateTime timestamp in log.timestamps)
                    {
                        allLogs.Add(new KeyValuePair<DateTime, PhenomLog>(timestamp, log));
                    }

                    for (int i = 0; i < log.logItems.Count; i++)
                    {
                        Destroy(log.logItems[i].gameObject);
                    }

                    log.count = 0;
                    log.logItems.Clear();
                }

                allLogs = allLogs.OrderBy(l => l.Key).ToList();

                foreach (KeyValuePair<DateTime, PhenomLog> kvp in allLogs)
                {
                    PhenomLog log = kvp.Value;

                    if (logTypeDisplayEnabled[(int)log.logType])
                    {
                        CreateNewLogItem(log);
                        // log.logItems.Add(Instantiate(logItemPrefab, logRoot).Init(log, toggleGroup, log.count - 1, collapse, useTimestamp));
                    }
                }
            }

            // OrderedDictionary<string, PhenomLogItem> tempDict = new OrderedDictionary<string, PhenomLogItem>();

            //foreach (var kvp in logDict)
            //    tempDict.Add(kvp.Key, kvp.Value);

            // Clear();

            // foreach (PhenomLogItem item in tempDict.Values)
            //     NewLine(item.log, item.logType, item.stackTrace);
        }
    }
}