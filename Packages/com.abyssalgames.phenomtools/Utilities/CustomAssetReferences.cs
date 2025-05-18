using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace PhenomTools.Utility
{
  [Serializable]
  public abstract class AssetReferenceCached<Tasset> : AssetReferenceT<Tasset> where Tasset : UnityEngine.Object
  {
    public Tasset Cached => (OperationHandle.IsValid() && OperationHandle.IsDone) ? (Tasset)OperationHandle.Result : null;

    public AssetReferenceCached(string guid) : base(guid) { }

    public async UniTask<Tasset> LoadCached()
    {
      if (!OperationHandle.IsValid())
      {
        Debug.LogFormat("Loading cached asset for the first time - AssetGUID:{0}", AssetGUID);
        return await BeginLoadingAssetAsync();
      }
      else if (OperationHandle.IsDone)
      {
        Debug.LogFormat("Cached asset already finished loading - AssetGUID:{0}", AssetGUID);
        return (Tasset)OperationHandle.Result;
      }
      else
      {
        Debug.LogFormat("Still loading cached asset:{0}", AssetGUID);
        return await FinishPreviouslyStartedLoadingProcess();
      }
    }

    public async UniTask<Tasset> LoadCached(IProgress<float> onProgress)
    {
      if (!OperationHandle.IsValid())
      {
        Debug.LogFormat("Loading cached asset for the first time - AssetGUID:{0}", AssetGUID);
        DateTime start = DateTime.Now;
        AsyncOperationHandle<Tasset> loadHandle = LoadAssetAsync<Tasset>();
        Tasset loaded = await loadHandle.ToUniTask(onProgress);
        onProgress.Report(1f);
        Debug.LogFormat("Loaded for the first time - AssetGUID:{0} Name:{1} in {2}ms", AssetGUID, loaded.name, (int)(DateTime.Now - start).TotalMilliseconds);
        return loaded;
      }
      else if (OperationHandle.IsDone)
      {
        Debug.LogFormat("Cached asset already finished loading - AssetGUID:{0}", AssetGUID);
        onProgress.Report(1f);
        return (Tasset)OperationHandle.Result;
      }
      else
      {
        Debug.LogFormat("Still loading cached asset:{0}", AssetGUID);
        DateTime start = DateTime.Now;
        await OperationHandle.ToUniTask(onProgress);
        Tasset loaded = (Tasset)OperationHandle.Result;
        Debug.LogFormat("Loaded from cache - Name:{0} in {1}ms", loaded.name, (int)(DateTime.Now - start).TotalMilliseconds);
        return loaded;
      }
    }

    public virtual async UniTask ReleaseCached()
    {
      Debug.LogFormat("Releasing cached asset Name:{0}", (Cached != null) ? Cached.name : "<b>not cached</b>");

      if (OperationHandle.IsValid() && !OperationHandle.IsDone)
        await OperationHandle.Task;

      if (OperationHandle.IsValid())
        ReleaseAsset();
    }

    protected virtual async UniTask<Tasset> BeginLoadingAssetAsync()
    {
      DateTime start = DateTime.Now;
      Tasset loaded = await LoadAssetAsync<Tasset>();
      Debug.LogFormat("Loaded for the first time - AssetGUID:{0} Name:{1} in {2}ms", AssetGUID, loaded.name, (int)(DateTime.Now - start).TotalMilliseconds);
      return loaded;
    }

    protected async UniTask<Tasset> FinishPreviouslyStartedLoadingProcess()
    {
      DateTime start = DateTime.Now;
      Tasset loaded = (Tasset)(await OperationHandle.Task);
      Debug.LogFormat("Loaded from cache - Name:{0} in {1}ms", loaded.name, (int)(DateTime.Now - start).TotalMilliseconds);
      return loaded;
    }
  }

  [Serializable]
  public class SpriteAssetReference : AssetReferenceCached<Sprite> { public SpriteAssetReference(string guid = "") : base(guid) { } }

  [Serializable]
  public class TextMeshProSpriteAssetReference : AssetReferenceCached<TMP_SpriteAsset> { public TextMeshProSpriteAssetReference(string guid = "") : base(guid) { } }


  // #region Asset Reference Types For GameData Types
  // public interface IGameDataAssetReferenceCached
  // {
  //   public UniTask ReleaseCached();
  // }
  //
  //

  // [Serializable]
  // public abstract class GameDataAssetReferenceCached<Tasset> : AssetReferenceCached<Tasset>, IGameDataAssetReferenceCached
  //   where Tasset : GameData
  // {
  //   public GameDataAssetReferenceCached(string guid) : base(guid) { }
  //
  //   protected override async UniTask<Tasset> BeginLoadingAssetAsync()
  //   {
  //     SharedGameDataControllerProvider.Instance.RegisterGameDataAssetRefForCleanupLater(this);
  //     return await base.BeginLoadingAssetAsync();
  //   }
  //
  //   public override async UniTask ReleaseCached()
  //   {
  //     Debug.LogFormat("Releasing cached asset Name:{0}", (Cached != null) ? Cached.name : "<b>not cached</b>");
  //
  //     if (OperationHandle.IsValid() && !OperationHandle.IsDone)
  //     {
  //       Tasset loaded = await FinishPreviouslyStartedLoadingProcess();
  //       if (loaded is IGameDataAssetReferenceCached releasableGameData)
  //         await releasableGameData.ReleaseCached();
  //     }
  //
  //     if (OperationHandle.IsValid())
  //       ReleaseAsset();
  //   }
  // }
  //
  //
  //
  //
  // [Serializable]
  // public class VariableDefinitionAssetReference : GameDataAssetReferenceCached<VariableDefinition>
  // {
  //   public VariableDefinitionAssetReference() : base(string.Empty) { }
  //   public VariableDefinitionAssetReference(string guid) : base(guid) { }
  // }
  //
  // [Serializable]
  // public class SkillAssetReference : GameDataAssetReferenceCached<Skill> { public SkillAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class BasicItemAssetReference : GameDataAssetReferenceCached<BaseItem> { public BasicItemAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class CurrencyAssetReference : GameDataAssetReferenceCached<VirtualCurrency> { public CurrencyAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class GearEquipmentAssetReference : GameDataAssetReferenceCached<Gear> { public GearEquipmentAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class GameDataTagAssetReference : GameDataAssetReferenceCached<GameDataTag> { public GameDataTagAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class RoleAssetReference : GameDataAssetReferenceCached<Role> { public RoleAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class EnemyNpcAssetReference : GameDataAssetReferenceCached<EnemyNpc> { public EnemyNpcAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class ContentDefinitionAssetReference : GameDataAssetReferenceCached<ContentDefinition> { public ContentDefinitionAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class BaseObjectiveAssetReference : GameDataAssetReferenceCached<BaseObjective> { public BaseObjectiveAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class GameEventAssetReference : GameDataAssetReferenceCached<GameEvent> { public GameEventAssetReference(string guid) : base(guid) { } }
  //
  // [Serializable]
  // public class GameEventTaskAssetReference : GameDataAssetReferenceCached<GameEventTask> { public GameEventTaskAssetReference(string guid) : base(guid) { } }
  // #endregion

  #region Extensions
  public static class AssetReferenceCachedExtensions
  {
    public static void LoadAndThen<Tasset>(this AssetReferenceCached<Tasset> assetReference, Action<Tasset> action)
      where Tasset : UnityEngine.Object
    {
      Tasset cached = assetReference.Cached;
      if (cached != null)
        action(cached);
      else
        InternalLoadAndThen(assetReference, action).Forget();
    }

    public static void LoadAndThen<Tasset1, Tasset2>(
      this (AssetReferenceCached<Tasset1> a1, AssetReferenceCached<Tasset2> a2) assetRefs,
      Action<Tasset1, Tasset2> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
    {
      Tasset1 cached1 = assetRefs.a1.Cached;
      Tasset2 cached2 = assetRefs.a2.Cached;

      if ((cached1 != null) && (cached2 != null))
        action(cached1, cached2);
      else
        InternalLoadAndThen(assetRefs.a1, assetRefs.a2, action).Forget();
    }

    public static void LoadAndThen<Tasset1, Tasset2, Tasset3>(
      this (AssetReferenceCached<Tasset1> a1, AssetReferenceCached<Tasset2> a2, AssetReferenceCached<Tasset3> a3) assetRefs,
      Action<Tasset1, Tasset2, Tasset3> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
      where Tasset3 : UnityEngine.Object
    {
      Tasset1 cached1 = assetRefs.a1.Cached;
      Tasset2 cached2 = assetRefs.a2.Cached;
      Tasset3 cached3 = assetRefs.a3.Cached;

      if ((cached1 != null) && (cached2 != null) && (cached3 != null))
        action(cached1, cached2, cached3);
      else
        InternalLoadAndThen(assetRefs.a1, assetRefs.a2, assetRefs.a3, action).Forget();
    }

    public static void LoadAndThen<Tasset1, Tasset2, Tasset3, Tasset4>(
      this (AssetReferenceCached<Tasset1> a1, AssetReferenceCached<Tasset2> a2, AssetReferenceCached<Tasset3> a3, AssetReferenceCached<Tasset4> a4) assetRefs,
      Action<Tasset1, Tasset2, Tasset3, Tasset4> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
      where Tasset3 : UnityEngine.Object
      where Tasset4 : UnityEngine.Object
    {
      Tasset1 cached1 = assetRefs.a1.Cached;
      Tasset2 cached2 = assetRefs.a2.Cached;
      Tasset3 cached3 = assetRefs.a3.Cached;
      Tasset4 cached4 = assetRefs.a4.Cached;

      if ((cached1 != null) && (cached2 != null) && (cached3 != null) && (cached4 != null))
        action(cached1, cached2, cached3, cached4);
      else
        InternalLoadAndThen(assetRefs.a1, assetRefs.a2, assetRefs.a3, assetRefs.a4, action).Forget();
    }

    public static void LoadAndAssignSprite(this SpriteAssetReference spriteReference, Image imageToAssignSprite)
    {
      Sprite cached = spriteReference.Cached;
      if (cached != null)
        imageToAssignSprite.sprite = cached;
      else
        InternalLoadAndThenAssignSprite(spriteReference, imageToAssignSprite).Forget();
    }

    private static async UniTaskVoid InternalLoadAndThen<Tasset>(AssetReferenceCached<Tasset> assetReference, Action<Tasset> action)
      where Tasset : UnityEngine.Object
      => action(await assetReference.LoadCached());

    private static async UniTaskVoid InternalLoadAndThenAssignSprite(SpriteAssetReference spriteReference, Image imageComponent)
    {
      Sprite sprite = await spriteReference.LoadCached();
      if (imageComponent != null)
        imageComponent.sprite = sprite;
    }

    private static async UniTaskVoid InternalLoadAndThen<Tasset1, Tasset2>(
      AssetReferenceCached<Tasset1> assetReference1,
      AssetReferenceCached<Tasset2> assetReference2,
      Action<Tasset1, Tasset2> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
    {
      (Tasset1 asset1, Tasset2 asset2) = await UniTask.WhenAll(
        assetReference1.LoadCached(),
        assetReference2.LoadCached()
      );

      action(asset1, asset2);
    }

    private static async UniTaskVoid InternalLoadAndThen<Tasset1, Tasset2, Tasset3>(
      AssetReferenceCached<Tasset1> assetReference1,
      AssetReferenceCached<Tasset2> assetReference2,
      AssetReferenceCached<Tasset3> assetReference3,
      Action<Tasset1, Tasset2, Tasset3> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
      where Tasset3 : UnityEngine.Object
    {
      (Tasset1 asset1, Tasset2 asset2, Tasset3 asset3) = await UniTask.WhenAll(
        assetReference1.LoadCached(),
        assetReference2.LoadCached(),
        assetReference3.LoadCached()
      );

      action(asset1, asset2, asset3);
    }

    private static async UniTaskVoid InternalLoadAndThen<Tasset1, Tasset2, Tasset3, Tasset4>(
      AssetReferenceCached<Tasset1> assetReference1,
      AssetReferenceCached<Tasset2> assetReference2,
      AssetReferenceCached<Tasset3> assetReference3,
      AssetReferenceCached<Tasset4> assetReference4,
      Action<Tasset1, Tasset2, Tasset3, Tasset4> action)
      where Tasset1 : UnityEngine.Object
      where Tasset2 : UnityEngine.Object
      where Tasset3 : UnityEngine.Object
      where Tasset4 : UnityEngine.Object
    {
      (Tasset1 asset1, Tasset2 asset2, Tasset3 asset3, Tasset4 asset4) = await UniTask.WhenAll(
        assetReference1.LoadCached(),
        assetReference2.LoadCached(),
        assetReference3.LoadCached(),
        assetReference4.LoadCached()
      );

      action(asset1, asset2, asset3, asset4);
    }
  }
  #endregion
}