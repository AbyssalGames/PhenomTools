using Cysharp.Threading.Tasks;

namespace PhenomTools.UI
{
  public interface IView
  {
    CanvasType CanvasType { get; }
    /// <summary>
    /// Executes every time the view is opened
    /// </summary>
    void Initialize();
    /// <summary>
    /// Executes every time the view is closing
    /// </summary>
    UniTask OnClosed();
    /// <summary>
    /// Sets gameObject to be active
    /// </summary>
    void Show();

    
    /// <summary>
    /// Sets gameObject to be inactive
    /// </summary>
    void Hide();
  }
}
