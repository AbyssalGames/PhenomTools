using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Object = UnityEngine.Object;

namespace PhenomTools
{
    public enum PhenomLogType
    {
        Error, Assertion, Warning, Normal, Exception, Boon
    }

    /// <summary>
    /// Custom Run-time logger
    /// </summary>
    public class PhenomConsole : MonoBehaviour
    {
        private static PhenomConsole instance;

        [SerializeField]
        private RectTransform consoleTransform = null;
        [SerializeField]
        private CanvasGroup consoleCanvasGroup = null;
        [SerializeField]
        private CanvasGroup showButtonCanvasGroup = null;
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
        private PhenomConsoleLogItem logItemPrefab = null;
        //[SerializeField]
        //private GameObject openLogFileButton = null;
        [SerializeField]
        private int maxLines = 100;

        [Space]
        [SerializeField]
        private bool collapse = true;
        [SerializeField]
        private bool listenForSystemLogs = false;

        [Space]
        [SerializeField]
        private bool useTimestamp = true;
        [SerializeField]
        private bool useConsoleWriteLine = false;

        private Dictionary<string, PhenomConsoleLogItem> logDict = new Dictionary<string, PhenomConsoleLogItem>();
        private List<PhenomConsoleLogItem> logItems = new List<PhenomConsoleLogItem>();
        private int currentLogCount;

        private static string fileName;

        private void Awake()
        {
#if !TEST
            Destroy(gameObject);
            return;
#else

            if (instance == null)
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
            }
#endif
        }

        private void OnEnable()
        {
            if(listenForSystemLogs)
                Application.logMessageReceived += SystemLogReceived;
        }

        private void OnDisable()
        {
            if (listenForSystemLogs)
                Application.logMessageReceived -= SystemLogReceived;
        }

        private void SystemLogReceived(string log, string stackTrace, LogType type)
        {
            Log(log, (PhenomLogType)type, null, stackTrace, false, false, true);
        }

        public static void Log(object log, PhenomLogType logType = PhenomLogType.Normal, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false, bool isSystemLog = false)
        {
            if (useTypePrefix || (instance != null && instance.useConsoleWriteLine == true))
                log = string.Concat(GetTypePrefix(logType), log);

            if (useTimestamp && instance != null && instance.useTimestamp == true)
            {
                DateTime now = DateTime.Now;
                log = string.Concat(now.Hour.ToString("00"), ":", now.Minute.ToString("00"), ":", now.Second.ToString("00"), ":", now.Millisecond.ToString("000"), " : ", log);
            }
        
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
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogError(log, context);
                    break;
                case PhenomLogType.Boon:
                    if (instance != null && instance.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.Log(log, context);
                    break;
            }

            if (instance != null && (!instance.listenForSystemLogs || (instance.listenForSystemLogs && isSystemLog)))
            {
                instance.NewLine(log.ToString(), logType, stackTrace);
#if !UNITY_EDITOR
                instance.WriteToFile(log.ToString());
#endif
            }
        }
        public static void LogError(object log, Object context = null, string stackTrace = null, bool useTimestamp = true, bool useTypePrefix = false, bool isSystemLog = false)
        {
            Log(log, PhenomLogType.Error, context, stackTrace, useTimestamp, useTypePrefix, isSystemLog);
        }

        private void NewLine(string log, PhenomLogType logType, string stackTrace)
        {
            if (collapse && logDict.TryGetValue(log, out PhenomConsoleLogItem item))
            {
                IncrementLogCountBadge(item);
            }
            else
            {
                if (currentLogCount >= maxLines)
                {
                    if (collapse)
                        logDict.Remove(logItems[0].log);

                    Destroy(logItems[0]);
                    logItems.RemoveAt(0);
                }

                CreateNewLogObject(log, logType, stackTrace);
            }
        }

        private void CreateNewLogObject(string log, PhenomLogType logType, string stackTrace)
        {
            PhenomConsoleLogItem newLogItem = Instantiate(logItemPrefab, logRoot);
            newLogItem.Init(log.ToString(), logType, currentLogCount++, stackTrace, toggleGroup);

            logItems.Add(newLogItem);

            if (collapse)
                logDict.Add(log, newLogItem);
        }

        private void IncrementLogCountBadge(PhenomConsoleLogItem item)
        {
            item.IncrementCountBadge();
        }

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

            if(instance.toggleGroup.AnyTogglesOn())
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
                case PhenomLogType.Exception:
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

        private Vector3 consoleOriginalPos;
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
    }
}