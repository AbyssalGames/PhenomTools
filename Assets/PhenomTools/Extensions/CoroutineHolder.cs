using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhenomTools
{
    public class CoroutineHolder : MonoBehaviour
    {
        public Dictionary<Object, IEnumerator> coroutines = new Dictionary<Object, IEnumerator>();

        private static CoroutineHolder _instance;
        private static CoroutineHolder instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new GameObject("CoroutineHolder").AddComponent<CoroutineHolder>();
                DontDestroyOnLoad(_instance.gameObject);

                return _instance;
            }
        }

        public new static Coroutine StartCoroutine(IEnumerator routine)
        {
            return (instance as MonoBehaviour).StartCoroutine(routine);
        }

        public new static void StopCoroutine(IEnumerator routine)
        {
            (instance as MonoBehaviour).StopCoroutine(routine);
        }

        public static void StartAndRegisterCoroutine(Object keyObject, IEnumerator routine)
        {
            TryStopCoroutine(keyObject);

            instance.coroutines.Add(keyObject, routine);
            StartCoroutine(routine);
        }

        public static bool TryStopCoroutine(Object keyObject)
        {
            if (!instance.coroutines.TryGetValue(keyObject, out IEnumerator currentRoutine)) return false;

            Debug.Log("Stopped Coroutine for: " + keyObject.name);

            StopCoroutine(currentRoutine);
            instance.coroutines.Remove(keyObject);

            return true;
        }
    }
}
