using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PhenomTools.Utility
{
  /// <summary>
  /// Creates an AssetReference that is restricted to having a specific Component.
  /// * This is the class that inherits from AssetReference.  It is generic and does not specify which Components it might care about.  A concrete child of this class is required for serialization to work.* At edit-time it validates that the asset set on it is a GameObject with the required Component.
  /// * At edit-time it validates that the asset set on it is a GameObject with the required Component.
  /// * At runtime it can load/instantiate the GameObject, then return the desired component.  API matches base class (LoadAssetAsync & InstantiateAsync).
  /// </summary>
  /// <typeparam name="TComponent"> The component type.</typeparam>
  public class ComponentReference<TComponent> : AssetReference where TComponent : Component
  {
    private Task<TComponent> CachedTask;
    public TComponent Cached => ((CachedTask != null) && CachedTask.IsCompletedSuccessfully) ? CachedTask.Result : null;
    public bool IsCached => Cached != null;

    public ComponentReference(string guid) : base(guid) { }

    public async UniTask<TComponent> LoadCached()
    {
      if (CachedTask == null)
      {
        Debug.LogFormat("Loading cached asset for the first time - AssetGUID:{0}", AssetGUID);
        Task<TComponent> loadTask = CachedTask = LoadAssetAsync().Task;
        TComponent cached = await loadTask;
        Debug.LogFormat("Loaded - AssetGUID:{0} Name:{1}", AssetGUID, Cached.name);
        return cached;
      }
      
      if (!IsCached)
        await CachedTask;

      Debug.LogFormat("Loaded from cache - Name:{0}", Cached.name);
      return Cached;
    }

    /*public void ReleaseCached()
    {
      Debug.LogFormat("Releasing cached asset Name:{0}", (Cached != null) ? Cached.name : "<b>not cached</b>");
      if (Cached != null)
        ReleaseAsset();
      CachedTask = null;
    }*/


    public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
    {
      return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(position, Quaternion.identity, parent), GameObjectReady);
    }

    public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
    {
      return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
    }
    public AsyncOperationHandle<TComponent> LoadAssetAsync()
    {
      return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.LoadAssetAsync<GameObject>(), GameObjectReady);
    }

    AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
    {
      var comp = arg.Result.GetComponent<TComponent>();
      return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(comp, string.Empty);
    }

    public override bool ValidateAsset(Object obj)
    {
      var go = obj as GameObject;
      return go != null && go.GetComponent<TComponent>() != null;
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
      //this load can be expensive...
      var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
      return go != null && go.GetComponent<TComponent>() != null;
#else
            return false;
#endif
    }

    public void ReleaseInstance(AsyncOperationHandle<TComponent> op)
    {
      // Release the instance
      var component = op.Result as Component;
      if (component != null)
      {
        Addressables.ReleaseInstance(component.gameObject);
      }

      // Release the handle
      Addressables.Release(op);
    }
  }
}