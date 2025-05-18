using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using PhenomTools.Utility;
using UnityEngine;

namespace PhenomTools.UI
{
  public class ViewController : Singleton<ViewController>
  {
    [Serializable]
    private class CanvasDict : SerializedDictionary<CanvasType, Canvas> { }
    [SerializeField, Required]
    private CanvasDict canvasDict;

    [Button]
    public static async UniTask<View> Push(ViewAssetReference viewRef) => Push(await viewRef.LoadCached());

    public static async UniTask<View> Push(ViewAssetReference viewRef, Transform parent)
    {
      View instantiatedView = Instantiate(await viewRef.LoadCached(), parent);
      instantiatedView.Initialize();
      instantiatedView.Show();
      return instantiatedView;
    }

    public static View Push(View viewPrefab)
    {
      View instantiatedView = Instantiate(viewPrefab, Instance.canvasDict[viewPrefab.CanvasType].transform);
      instantiatedView.Initialize();
      instantiatedView.Show();
      return instantiatedView;
    }

    public static async UniTask<ViewAwaitable> PushAwaitable(ViewAssetReference viewRef) => PushAwaitable(await viewRef.LoadCached());

    public static ViewAwaitable PushAwaitable(View viewPrefab)
    {
      if (viewPrefab is ViewAwaitable viewAwaitablePrefab)
        return (ViewAwaitable)Push(viewAwaitablePrefab);

      throw new ArgumentException($"ViewAssetReference {viewPrefab.name} is not awaitable.");
    }
    
    public static async UniTask<bool> PushAndWaitForResult(ViewAssetReference viewRef) => await (await PushAwaitable(viewRef)).WaitForResult();
  }

  [Serializable]
  public class ViewAssetReference : ComponentReference<View> { public ViewAssetReference(string guid = "") : base(guid) { } }
  
  public enum CanvasType
  {
    /// <summary>
    /// Normal UI
    /// </summary>
    Main = 0,
    /// <summary>
    /// Toasts and Popups
    /// </summary>
    Alert = 1,
    /// <summary>
    /// Error Feedback
    /// </summary>
    Critical = 2
  }
}
