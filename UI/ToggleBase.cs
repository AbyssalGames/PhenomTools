using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace PhenomTools.UI
{
  public class ToggleBase : SelectableExtended, IPointerClickHandler, ISubmitHandler, ICanvasElement
  {
    public enum EToggleTransition
    {
      None,
      Fade
    }

    public EToggleTransition ToggleTransition = EToggleTransition.Fade;

    [SerializeField, OnValueChanged("EditorToggle")]
    private bool isOn;
    public bool IsOn
    {
      get => isOn;
      set => Set(value);
    }
    
    [SerializeField, OnValueChanged("EditorSetToggleGroup")]
    private ToggleGroupExtended group;
    public ToggleGroupExtended Group
    {
      get => group;
      set
      {
        SetToggleGroup(value, true);
        PlayEffect(true);
      }
    }

    public Graphic[] Graphics;

    [FoldoutGroup("Events")]
    public UnityEvent<bool> OnValueChanged;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
      base.OnValidate();

      if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
        CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
    }

#endif // if UNITY_EDITOR

    public virtual void Rebuild(CanvasUpdate executing)
    {
#if UNITY_EDITOR
      if (executing == CanvasUpdate.Prelayout)
        OnValueChanged.Invoke(IsOn);
#endif
    }

    protected override void OnDestroy()
    {
      if (group != null)
        group.EnsureValidState();
      base.OnDestroy();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      SetToggleGroup(group, false);
      PlayEffect(true);
    }

    protected override void OnDisable()
    {
      SetToggleGroup(null, false);
      base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
      if (Graphics != null && Graphics[0] != null)
      {
        bool oldValue = !Mathf.Approximately(Graphics[0].canvasRenderer.GetColor().a, 0);
        if (IsOn != oldValue)
        {
          IsOn = oldValue;
          Set(!oldValue);
        }
      }

      base.OnDidApplyAnimationProperties();
    }

    private void EditorSetToggleGroup(ToggleGroupExtended newGroup)
    {
      SetToggleGroup(newGroup, true);
      PlayEffect(true);
    }

    private void SetToggleGroup(ToggleGroupExtended newGroup, bool setMemberValue)
    {
      if (group != null)
        group.UnregisterToggle(this);

      if (setMemberValue)
        group = newGroup;

      if (newGroup != null && IsActive())
        newGroup.RegisterToggle(this);

      if (newGroup != null && isOn && IsActive())
        newGroup.NotifyToggleOn(this);
    }

    public void SetIsOnWithoutNotify(bool value)
    {
      Set(value, false);
    }

    private void Set(bool value, bool sendCallback = true)
    {
      isOn = value;
      
      if (group != null && group.isActiveAndEnabled && IsActive())
      {
        if (isOn || (!group.AnyTogglesOn() && !group.AllowSwitchOff))
        {
          isOn = true;
          group.NotifyToggleOn(this, sendCallback);
        }
      }

      PlayEffect(ToggleTransition == EToggleTransition.None);
      if (sendCallback)
      {
        UISystemProfilerApi.AddMarker("Toggle.value", this);
        OnValueChanged.Invoke(isOn);
      }
    }

    private void PlayEffect(bool instant)
    {
      if (Graphics == null || Graphics.Length == 0)
        return;

#if UNITY_EDITOR
      if (!Application.isPlaying)
      {
        foreach (Graphic graphic in Graphics)
        {
          graphic.canvasRenderer.SetAlpha(IsOn ? 1f : 0f);
          EditorUtility.SetDirty(graphic.canvasRenderer);
        }
      }
      else
#endif
      {
        foreach (Graphic graphic in Graphics)
          graphic.CrossFadeAlpha(IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        
      }
    }

    protected override void Start()
    {
      PlayEffect(true);
    }

    private void InternalToggle()
    {
      if (!IsActive() || !IsInteractable())
        return;

      IsOn = !isOn;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;

      InternalToggle();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      InternalToggle();
    }

    private void EditorToggle(bool on)
    {
      Set(on, false);
      PlayEffect(true);
      
#if UNITY_EDITOR
      if (!Application.isPlaying)
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }

    public virtual void LayoutComplete() { }

    public virtual void GraphicUpdateComplete() { }
  }
}
