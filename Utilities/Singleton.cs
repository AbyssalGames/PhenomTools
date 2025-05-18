using UnityEngine;

namespace PhenomTools.Utility
{
  public abstract class Singleton<T> : MonoBehaviour where T : Component
  {
    private static T instance;

    public static T Instance
    {
      get
      {
        if (instance != null)
          return instance;

        if (FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None) is not { Length: > 0 } objs)
        {
          Debug.LogWarningFormat("No instance of {0} exists in the scene.", typeof(T));
          return instance;
        }
        
        instance = objs[0];

        if (objs.Length > 1)
        {
          for (int i = 1; i < objs.Length; i++)
            DestroyImmediate(objs[i].gameObject);
        }

        return instance;
      }
    }

    protected virtual void Awake()
    {
      if(instance == null)
      {
        instance = this as T;
        DontDestroyOnLoad(gameObject);
      }
      else if(instance != this)
      {
        DestroyImmediate(gameObject);
      }
    }

    protected virtual void OnDestroy()
    {
      if (instance == this)
      {
        instance = null;
      }
    }

  }
}
