using PhenomTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public float time;
    public int count;
    private IEnumerator reference;

    [ContextMenu("Start")]
    public void Start()
    {
        //PhenomUtils.DelayActionByTime(time, Callback);
        //PhenomUtils.DelayActionByFrames(count, Callback);
        //PhenomUtils.RepeatActionByTime(time, count, Callback, Complete);
        PhenomUtils.RepeatActionByFrames(60, count, Callback, Complete);

        //reference = PhenomUtils.RepeatActionByTime(1f, Repeat);
        //reference = PhenomUtils.RepeatActionByTime(1f, 10, Repeat, () => Debug.Log("Complete"));
        //reference = PhenomUtils.RepeatActionByFrames(60, Repeat);
    }

    private void Callback()
    {
        Debug.Log("Thing");
    }

    private void Complete()
    {
        Debug.Log("Complete");
    }

    //private void Repeat()
    //{
    //    count++;
    //    Debug.Log("count: " + count);
    //}

    [ContextMenu("Stop")]
    public void Stop()
    {
        reference.Stop();
    }
}
