using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// TODO: Add a toggle group and make every log item a toggle so that they can be clicked on in order to see the whole message, like the regular console

public enum LogType
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
    private Animator animator = null;
    [SerializeField]
    private RectTransform logRoot = null;
    [SerializeField]
    private GameObject logItemPrefab = null;
    //[SerializeField]
    //private Scrollbar scrollbar = null;
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

    private Dictionary<string, int> logDict = new Dictionary<string, int>();
    private int currentLogCount;
    private List<GameObject> logObjects = new List<GameObject>();

    private static string fileName;

    private void Awake()
    {
        if(instance == null)
        { 
            instance = this;

#if !UNITY_EDITOR
            fileName = Application.dataPath + "/PhenomLog_" + DateTime.UtcNow.ToString("MM-dd_HH-mm-ss") + ".txt";
            File.Create(fileName);
#endif
        }
        else
        {
            Destroy(gameObject);
        }
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

    private void SystemLogReceived(string log, string stackTrace, UnityEngine.LogType type)
    {
        Log(log, (LogType)type);
    }

    public static void Log(object log, LogType logType = LogType.Normal, Object context = null, bool useTimestamp = true, bool useTypePrefix = false)
    {
        if (useTypePrefix || instance?.useConsoleWriteLine == true)
            log = string.Concat(GetTypePrefix(logType), log);

        if (useTimestamp && instance?.useTimestamp == true)
        {
            DateTime now = DateTime.Now;
            log = string.Concat(now.Hour.ToString("00"), ":", now.Minute.ToString("00"), ":", now.Second.ToString("00"), ":", now.Millisecond.ToString("000"), " : ", log);
        }
        
        switch (logType)
        {
            case LogType.Normal:
                if (instance?.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.Log(log, context);
                break;
            case LogType.Warning:
                if (instance?.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogWarning(log, context);
                break;
            case LogType.Error:
                if (instance?.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogError(log, context);
                break;
            case LogType.Assertion:
                if (instance?.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.LogError(log, context);
                break;
            case LogType.Boon:
                if (instance?.useConsoleWriteLine == true) Console.WriteLine(log); else Debug.Log(log, context);
                break;
        }

        if (instance)
        {
            instance.NewLine(log.ToString(), logType);
#if !UNITY_EDITOR
            instance.WriteToFile(log.ToString());
#endif
        }
    }
    public static void LogError(object log, Object context = null, bool useTimestamp = true, bool useTypePrefix = false) => Log(log, LogType.Error, context, useTimestamp, useTypePrefix);

    private void NewLine(string log, LogType logType)
    {
        if (currentLogCount >= maxLines)
        {
            Destroy(logObjects[0]);
            logObjects.RemoveAt(0);
        }

        if (collapse && logDict.TryGetValue(log, out int count))
            logDict[log] = count;
        else
            CreateNewLogObject(log, logType);
    }

    private void CreateNewLogObject(string log, LogType logType)
    {
        PhenomConsoleLogItem newLogItem = Instantiate(logItemPrefab, logRoot).GetComponent<PhenomConsoleLogItem>();
        newLogItem.Init(log.ToString(), logType, currentLogCount++);
        logObjects.Add(newLogItem.gameObject);
    }

    private void IncrementLogCountBadge()
    {

    }

#if !UNITY_EDITOR
    public void WriteToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(fileName, true))
            writer.WriteLine(text);
    }
#endif

    public static Color GetColorOfLogType(LogType type, float value, float alpha)
    {
        switch (type)
        {
            default:
            case LogType.Normal:
                return new Color(1 * value, 1 * value, 1 * value, alpha);
            case LogType.Assertion:
                return new Color(1 * value, .5f * value, 0 * value, alpha);
            case LogType.Warning:
                return new Color(1 * value, .92f * value, .016f * value, alpha);
            case LogType.Error:
                return new Color(1 * value, 0 * value, 0 * value, alpha);
            case LogType.Boon:
                return new Color(0 * value, 1 * value, 0 * value, alpha);
        }
    }

    public static string GetTypePrefix(LogType type)
    {
        switch (type)
        {
            default:
                return "";
            case LogType.Assertion:
                return "Assertion: ";
            case LogType.Boon:
                return "Boon: ";
            case LogType.Error:
                return "Error: ";
            case LogType.Warning:
                return "Warning: ";
        }
    }

    public void ToggleConsoleDisplay(bool on)
    {
        animator.SetBool("On", on);
    }
}
