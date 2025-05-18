using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhenomTools.UI
{
  public abstract class View : MonoBehaviour, IView
  {
    [field: SerializeField, Required] 
    public CanvasType CanvasType { get; private set; } = CanvasType.Main;

    public virtual void Initialize() {}
    public virtual UniTask OnClosed() 
    {
      return UniTask.CompletedTask;
    }

    public virtual void Show()
    {
      gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
      gameObject.SetActive(false);
    }
  }
}
