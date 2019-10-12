using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHolder : MonoBehaviour
{
    public Dictionary<Object, IEnumerator> coroutines = new Dictionary<Object, IEnumerator>();

    public void RegisterCoroutine(Object keyObject, IEnumerator coroutine)
    {
        TryStopCoroutine(keyObject);

        coroutines.Add(keyObject, coroutine);
        StartCoroutine(coroutine);
    }

    public void TryStopCoroutine(Object keyObject)
    {
        if (coroutines.TryGetValue(keyObject, out IEnumerator currentCoroutine))
        {
            Debug.Log("Stopped Coroutine for: " + keyObject.name);
            
            StopCoroutine(currentCoroutine);
            coroutines.Remove(keyObject);
        }
    }
}
