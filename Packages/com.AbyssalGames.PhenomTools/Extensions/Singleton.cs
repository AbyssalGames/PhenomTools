using System;
using UnityEngine;

namespace PhenomTools
{
  public class Singleton<T> : MonoBehaviour where T : Component
  {
    private static T instance;

    public static T Instance
    {
      get
      {
        if (instance != null)
          return instance;

        if (FindObjectsOfType(typeof(T)) is not T[] { Length: > 0 } objs) 
          return instance;
        
        instance = objs[0];

        if (objs.Length > 1)
        {
          for (int i = 1; i < objs.Length; i++)
            Destroy(objs[i].gameObject);
        }

        return instance;
      }
      set => instance = value;
    }

    protected void Awake()
    {
      if(instance == null)
      {
        instance = this as T;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        DestroyImmediate(gameObject);
      }
    }
  }
}
