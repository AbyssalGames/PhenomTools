using PhenomTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Tester : PersistentSingleton<Tester>
{
    //public string testString;

    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        // StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine()
    {
        PhenomConsole.LogAssertion("Test1");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.Log("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogAssertion("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogError("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogAssertion("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogError("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogAssertion("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.LogError("Test");
        yield return new WaitForSeconds(.2f);
        PhenomConsole.Log("Test");
        yield return new WaitForSeconds(.2f);
        StartCoroutine(TestRoutine());
    }

    [ContextMenu("Toggle")]
    public void Start()
    {
        // PhenomConsole.ToggleDebugMode(!PhenomConsole.isDebugMode);
        Test();
    }

    public void Vibrate()
    {
        Debug.Log("Vibrate");
    }
        //Debug.Log(testString.ToNonCamelCase());
    //}

    //    [SerializeField]
    //    private TextMeshProUGUI text = null;

    //private string testString = "house home go www.monstermmorpg.com nice hospital http://www.monstermmorpg.com this is incorrect url http://www.monstermmorpg.commerged continue";

    //public DynamicVisibilityRect rect;
    //public RectTransform other;
    //public ScrollRectExtended scrollRect;

    //private void Start()
    //{
    //    text.SetText(ToRich(testString));

    //Tester t = Instance;
    //rect.onBecameVisible += () => Debug.Log("Ye");
    //rect.BeginVisibilityChecks(other, 0);
    //rect.BeginVisibilityChecks(other, 1f);
    //rect.BeginVisibilityChecks(other, 30);
    //rect.BeginVisibilityChecks(other, scrollRect.onMove);

    //SceneManager.LoadScene("Empty");
    //}

    //private string ToRich(string raw)
    //{
    //    return Regex.Replace(raw,
    //            @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
    //            "<a target='_blank' href='$1'>$1</a>");
    //}

    //public OrderedDictionary<string, string> test = new OrderedDictionary<string, string>();

    //public RectTransform rt1, rt2;

    //public float time;
    //public int count;
    //private IEnumerator reference;

    //[ContextMenu("Start")]
    //public void Start()
    //{
    //    reference = PhenomUtils.RepeatActionByFrames(100, 5, Repeat, Complete, false, 300);

    //    //test.Add("thing1", "thing1Value");
    //    //test.Add("thing2", "thing2Value");
    //    //test.Add("thing3", "thing3Value");

    //    //Debug.Log(test.GetAt(1));

    //    //PhenomUtils.DelayActionByTime(time, Callback);
    //    //PhenomUtils.DelayActionByFrames(count, Callback);
    //    //PhenomUtils.RepeatActionByTime(time, count, Callback, Complete);
    //    //PhenomUtils.RepeatActionByFrames(60, count, Callback, Complete);

    //    //reference = PhenomUtils.RepeatActionByTime(1f, Repeat);
    //    //reference = PhenomUtils.RepeatActionByTime(1f, 10, Repeat, () => Debug.Log("Complete"));
    //    //reference = PhenomUtils.RepeatActionByFrames(60, Repeat);
    //}

    //private void Update()
    //{
    //    //if (rt1.Overlaps(rt2))
    //    //    Debug.Log("Overlaps");

    //    //if (rt1.Contains(rt2))
    //    //    Debug.Log("Contains");
    //}

    ////private void Callback()
    ////{
    ////    Debug.Log("Thing");
    ////}

    //private void Complete()
    //{
    //    Debug.Log("Complete");
    //}

    //private void Repeat()
    //{
    //    count++;
    //    Debug.Log("count: " + count);
    //}

    //[ContextMenu("Stop")]
    //public void Stop()
    //{
    //    reference.Stop();
    //}
}
