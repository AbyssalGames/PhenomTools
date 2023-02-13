using System;
using PhenomTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Tester : PhenomTools.Singleton<Tester>
{
    public RectTransform intersectionRect;
    public ScrollRectExtended scrollRect;
    public Transform parent;
    public TestItem testItemPrefab;
    public List<TestItem> items;
    
    [ContextMenu("Start")]
    public void Start()
    {
        foreach (TestItem item in items)
        {
            item.BeginVisibilityChecks(intersectionRect, scrollRect.onMove);
            item.BeginVisibilityChecks(intersectionRect, 1f);
            item.BeginVisibilityChecks(intersectionRect, 90);
        }
        
        for (int i = 0; i < 10; i++)
        {
            TestItem item = Instantiate(testItemPrefab, parent);
            item.Initialize();
            item.BeginVisibilityChecks(intersectionRect, scrollRect.onMove);
            item.BeginVisibilityChecks(intersectionRect, 1f);
            item.BeginVisibilityChecks(intersectionRect, 90);
            items.Add(item);
        }
        
        // foreach (TestItem testItem in items)
        // {
        //     testItem.BeginVisibilityChecks(other);
        // }
    }

    [ContextMenu("End")]
    public void End()
    {
        foreach (TestItem item in items)
        {
            item.EndVisibilityChecks();
        }
    }
    
    

    //public string testString;
    
    // [SerializeField]
    // private AdvancedInputFieldPlugin.AdvancedInputField inputField = null;
    // [SerializeField]
    // private LayoutElement keyboardExpander = null;
    //
    // public bool isKeyboardOpen;

    // [ContextMenu("Test")]
    // public void Test()
    // {
    //     Debug.Log("verylongstringverylongstringverylongstringverylongstringverylongstring".Truncate(10));
    //     Debug.Log("short".Truncate(10));
    //     // Debug.Log(SystemInfo.deviceUniqueIdentifier);
    //     // StartCoroutine(TestRoutine());
    // }

    // public void Open()
    // {
    //     isKeyboardOpen = true;
    //
    //     // if (isKeyboardOpen)
    //     //     return;
    //     // inputField.
    //
    //     // toolbar.gameObject.SetActive(true);
    //     // keyboardExpander.gameObject.SetActive(true);
    //     //
    //     // inputField.Select();
    //
    //     // float height = TouchScreenKeyboard.area.height;
    //     // PhenomUtils.DelayActionByTime(1f, () =>
    //     // {
    //     // float height = PhenomUtils.GetKeyboardHeightRelative((keyboardExpander.transform.parent as RectTransform).rect.height, false);
    //     // keyboardExpander.minHeight = height;
    //     Debug.Log("Open");
    //     // });
    // }

    // public void Close()
    // {
    //     isKeyboardOpen = false;
    //     // if (!isKeyboardOpen)
    //     //     return;
    //     Debug.Log("Close");
    //     keyboardExpander.minHeight = 0;
    // }
    //
    // protected virtual void Update()
    // {
    //     if (isKeyboardOpen)
    //     {
    //         // isKeyboardOpen = true;
    //         keyboardExpander.minHeight = PhenomUtils.GetKeyboardHeightRelative((keyboardExpander.transform.parent as RectTransform).rect.height, false);
    //     }
    //     // else if(isKeyboardOpen)
    //     // {
    //         // Close();
    //         // isKeyboardOpen = false;
    //     // }
    // }
    //
    // public void OnTextSelectionChanged(int startIndex, int endIndex)
    // {
    //     // if(endIndex - startIndex > 0)
    //     //     Debug.Log("Selected: " + inputField.Text.Substring(startIndex, endIndex-startIndex));
    // }

    // private IEnumerator TestRoutine()
    // {
    //     PhenomConsole.LogAssertion("Test1");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.Log("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogAssertion("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogError("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogAssertion("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogError("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogAssertion("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.LogError("Test");
    //     yield return new WaitForSeconds(.2f);
    //     PhenomConsole.Log("Test");
    //     yield return new WaitForSeconds(.2f);
    //     StartCoroutine(TestRoutine());
    // }

    // [ContextMenu("Toggle")]
    // public void Start()
    // {
    //     // PhenomConsole.ToggleDebugMode(!PhenomConsole.isDebugMode);
    //     Test();
    // }

    // public void Vibrate()
    // {
    //     Debug.Log("Vibrate");
    // }
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
