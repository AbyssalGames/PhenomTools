using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// TODO: Add a toggle group and make every log item a toggle so that they can be clicked on in order to see the whole message, like the regular console

public enum LogType
{
    Error, Assertion, Warning, Normal, Exception, Boon
}

public class PhenomConsole : MonoBehaviour
{
    private static PhenomConsole instance;

    [SerializeField]
    private RectTransform logRoot;
    [SerializeField]
    private GameObject logItemPrefab;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private int maxLines = 100;
    [SerializeField]
    private bool collapse = true;
    [SerializeField]
    private bool listenForSystemLogs;

    [Space]
    [SerializeField]
    private bool useLogFile;
    [SerializeField]
    private bool useConsoleWriteLine;
    [SerializeField]
    private string logName = "PhenomLog_";

    private Dictionary<string, int> logDict = new Dictionary<string, int>();
    private int currentLogCount;
    private List<GameObject> logObjects = new List<GameObject>();
    private string fileName;

    private void Awake()
    {
        instance = (PhenomConsole)this.SetAsSingleton(instance);

#if !UNITY_EDITOR
        if (instance.useLogFile)
        {
            fileName = Application.dataPath + "/" + logName + DateTime.UtcNow.ToString("MM-dd_HH-mm-ss") + ".txt";
            File.Create(fileName);
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

    private void SystemLogReceived(string log, string stackTrace, UnityEngine.LogType type)
    {
        Log(log, (LogType)type);
    }

    public static void Log(object log, LogType logType = LogType.Normal, Object context = null, bool useTimestamp = true, bool useTypePrefix = false)
    {
        if (useTypePrefix)
            log = string.Concat(GetTypePrefix(logType), log);

        if (useTimestamp)
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
            if (instance.useLogFile)
                instance.WriteToFile(log.ToString());
#endif
        }
    }

    private void NewLine(string log, LogType logType)
    {
        if (currentLogCount >= maxLines)
        {
            Destroy(logObjects[0]);
            logObjects.RemoveAt(0);
        }

        if (collapse && logDict.TryGetValue(log, out int count))
        {

            logDict[log] = count;
        }
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

    public void WriteToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(fileName, true))
            writer.WriteLine(text);
    }

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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        PhenomConsole.Log("Test Log");
    //    }
    //}
}
