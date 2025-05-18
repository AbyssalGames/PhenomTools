using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhenomTools.UI
{
  public abstract class ButtonBase : SelectableExtended, IPointerClickHandler
  {
    private void Press()
    {
      if (!IsActive() || !IsInteractable())
        return;

      UISystemProfilerApi.AddMarker("Button.onClick", this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;

      Press();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      Press();
      if (!IsActive() || !IsInteractable())
        return;

      DoStateTransition(SelectionState.Pressed, false);
      StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
      float fadeTime = Colors.fadeDuration;
      float elapsedTime = 0f;

      while (elapsedTime < fadeTime)
      {
        elapsedTime += Time.unscaledDeltaTime;
        yield return null;
      }

      DoStateTransition(CurrentSelectionState, false);
    }
  }
}
