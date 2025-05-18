using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhenomTools.UI
{
  public abstract class ViewAwaitable : View, IViewAwaitable
  {
    public UniTaskCompletionSource ModalTcs { protected set; get; }
    public CancellationTokenSource Cts{ protected set; get; }

    public virtual void OnFinished()
    {
      Debug.LogFormat("ViewAwaitable: OnFinished called on {0}", gameObject.name);
      ModalTcs?.TrySetResult(); // Complete the task successfully
      Cleanup();
    }

    public virtual void OnCancelled()
    {
      Debug.LogFormat("ViewAwaitable: OnCancelled called on {0}", gameObject.name);
      ModalTcs?.TrySetCanceled(); // Set task as canceled
      Cleanup();
    }
    
    public virtual async UniTask<bool> WaitForResult()
    {
      Debug.LogFormat("ViewAwaitable: WaitForResult called on {0}", gameObject.name);

      if (Cts != null)
      {
        Debug.LogFormat("ViewAwaitable: Canceling previous task on {0}", gameObject.name);
        Cts.Cancel();
        Cts.Dispose();
      }
      Cts = new CancellationTokenSource();

      UniTaskCompletionSource tcs = ModalTcs = new();
      Cts.Token.RegisterWithoutCaptureExecutionContext(() => tcs.TrySetCanceled());

      try
      {
        Debug.LogFormat("ViewAwaitable: Waiting for task completion on {0}", gameObject.name);
        await tcs.Task; // Await the completion source
        Debug.LogFormat("ViewAwaitable: Task completed successfully on {0}", gameObject.name);
        return true;
      }
      catch (OperationCanceledException)
      {
        Debug.LogWarningFormat("ViewAwaitable: Task was canceled on {0}", gameObject.name);
        return false;
      }
      finally
      {
        Debug.LogFormat("ViewAwaitable: WaitForResult finished on {0}", gameObject.name);
      }
    }

    protected virtual void Cleanup()
    {
      Debug.LogFormat("ViewAwaitable: Cleanup called on {0}", gameObject.name);
      Cts?.Dispose();
      Cts = null;
      ModalTcs = null;
      Destroy(gameObject, 0.1f);
    }
  }
}
