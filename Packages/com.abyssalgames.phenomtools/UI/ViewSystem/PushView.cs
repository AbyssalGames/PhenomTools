using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhenomTools.UI
{
  public class PushView : MonoBehaviour
  {
    [SerializeField, Required] 
    private ViewAssetReference viewRef;

    public void Push()
    {
      ViewController.Push(viewRef).Forget();
    }
  }
}
