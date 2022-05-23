using UnityEngine;

namespace PhenomTools
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType(typeof(T)) as T[];

                    if (objs.Length > 0)
                    {
                        _instance = objs[0];

                        if(_instance.transform.parent == null)
                            DontDestroyOnLoad(_instance.gameObject);

                        if (objs.Length > 1)
                        {
                            for (int i = 1; i < objs.Length; i++)
                            {
                                Debug.Log("Duplicate of " + typeof(T).Name + " found, destroying " + objs[i].gameObject.name);
                                Destroy(objs[i].gameObject);
                            }
                        }

                        Debug.Log("Instance of " + typeof(T).Name + " set to " + _instance.gameObject.name, _instance.gameObject);
                        return _instance;
                    }

                    Debug.LogError("No instance of " + typeof(T).Name + " in the scene.");
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
