using Cysharp.Threading.Tasks;

namespace PhenomTools.UI
{
  public interface IViewAwaitable
  {
    UniTask<bool> WaitForResult();
    void OnFinished();
    void OnCancelled();
  }
}
