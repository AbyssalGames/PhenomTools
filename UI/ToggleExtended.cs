using PhenomTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PhenomTools.UI
{
  public class ToggleExtended : ToggleBase
  {
    [ShowIf("ShowLongPressConfig")]
    public float LongPressDuration = .5f;
    [ShowIf("ShowLongPressConfig")]
    public bool VibrateOnLongPress;

    [FoldoutGroup("Events")]
    public UnityEvent OnClick, OnHover, OnDown, OnUp, OnExit, OnReenter, OnLongPress, OnGhostClick = new();

    private bool isPressed;
    private bool isPointerDown;
    private bool longPressTriggered;
    private float timePressStarted;

    private void Update()
    {
      if (isPointerDown && !longPressTriggered && Time.time - timePressStarted > LongPressDuration)
      {
        longPressTriggered = true;
        
        if (VibrateOnLongPress)
          Vibration.Vibrate(100);

        OnLongPress?.Invoke();
      }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);

      OnHover?.Invoke();

      if (isPressed)
      {
        isPointerDown = true;
        timePressStarted = Time.time;
        OnReenter?.Invoke();
      }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      base.OnPointerDown(eventData);

      if (!IsActive() || !IsInteractable())
        return;

      isPressed = true;
      isPointerDown = true;
      timePressStarted = Time.time;
      longPressTriggered = false;

      OnDown?.Invoke();
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
      base.OnPointerExit(eventData);
      isPointerDown = false;

      //if (isPressed)
      OnExit?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerUp(eventData);

      OnUp?.Invoke();
      isPressed = false;
      isPointerDown = false;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);

      if (!IsActive() || !IsInteractable() || longPressTriggered)
        return;
      
      OnClick?.Invoke();
    }

    public void DoClick()
    {
      if (!IsActive() || !IsInteractable() || longPressTriggered)
        return;

      OnClick?.Invoke();
    }

    /// <summary>
    /// Play animations and sounds without invoking onClick
    /// </summary>
    public void GhostClick()
    {
      if (!IsActive() || !IsInteractable())
        return;

      OnGhostClick?.Invoke();
    }

    public bool ShowLongPressConfig() => OnLongPress.GetPersistentEventCount() > 0;
  }
}