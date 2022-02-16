using UnityEngine;

namespace PhenomTools
{
    public class Singleton<T> : MonoBehaviour where T : Component
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
                        _instance = objs[0];
                    else if (objs.Length > 1)
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    else if(_instance == null)
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
