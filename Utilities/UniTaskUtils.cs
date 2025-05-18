using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PhenomTools.Utility
{
  public struct UniTaskUtils
  {
    public static async UniTask RepeatActionByTime(float timeBetween, Action onRepeat, CancellationToken cancellationToken, float? initialDelay = null, DelayType updateMode = DelayType.DeltaTime)
    {
      if (initialDelay.GetValueOrDefault() > 0)
      {
        try
        {
          await UniTask.Delay(TimeSpan.FromSeconds(initialDelay!.Value), updateMode, cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException)
        {
          return;
        }
      }
      
      while (!cancellationToken.IsCancellationRequested)
      {
        try
        {
          await UniTask.Delay(TimeSpan.FromSeconds(timeBetween), updateMode, cancellationToken: cancellationToken);
          onRepeat?.Invoke();
        }
        catch (OperationCanceledException)
        {
          break;
        }
      }
    }
    
    public static async UniTask RepeatActionByFrames(int framesBetween, Action onRepeat, CancellationToken cancellationToken)
    {
      if (framesBetween < 1)
        framesBetween = 1;

      while (!cancellationToken.IsCancellationRequested)
      {
        try
        {
          await UniTask.DelayFrame(framesBetween, cancellationToken: cancellationToken);
          onRepeat?.Invoke();
        }
        catch (OperationCanceledException)
        {
          break;
        }
      }
    }
  }
}