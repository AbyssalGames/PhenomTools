using PhenomTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int count;
    private IEnumerator reference;

    [ContextMenu("Start")]
    public void Start()
    {
        //reference = PhenomUtils.RepeatActionByTime(1f, Repeat);
        //reference = PhenomUtils.RepeatActionByTime(1f, 10, Repeat, () => Debug.Log("Complete"));
        reference = PhenomUtils.RepeatActionByFrames(60, Repeat);
    }

    private void Repeat()
    {
        count++;
        Debug.Log("count: " + count);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        reference.Stop();
    }
}
