using System;
using System.Collections.Generic;
using R3;
using Sirenix.OdinInspector;
using PhenomTools.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools.UI
{
  [ExecuteAlways]
  [SelectionBase]
  [DisallowMultipleComponent]
  public class SelectableExtended
    :
      UIBehaviour,
      IPointerDownHandler, IPointerUpHandler,
      IPointerEnterHandler, IPointerExitHandler,
      ISelectHandler, IDeselectHandler
  {
    protected static SelectableExtended[] selectables = new SelectableExtended[10];
    protected static int selectableCount;
    private bool mEnableCalled;

    public static SelectableExtended[] AllSelectablesArray
    {
      get
      {
        SelectableExtended[] temp = new SelectableExtended[selectableCount];
        Array.Copy(selectables, temp, selectableCount);
        return temp;
      }
    }

    /// <summary>
    /// How many selectable elements are currently active.
    /// </summary>
    public static int AllSelectableCount => selectableCount;

    /// <summary>
    /// A List instance of the allSelectablesArray to maintain API compatibility.
    /// </summary>

    [Obsolete("Replaced with allSelectablesArray to have better performance when disabling a element", false)]
    public static List<SelectableExtended> AllSelectables => new(AllSelectablesArray);

    public static int AllSelectablesNoAlloc(Selectable[] selectables)
    {
      int copyCount = selectables.Length < selectableCount ? selectables.Length : selectableCount;
      Array.Copy(SelectableExtended.selectables, selectables, copyCount);
      return copyCount;
    }

    /// <summary>
    ///Transition mode for a Selectable.
    /// </summary>
    public enum TransitionType
    {
      None,
      ColorTint,
      SpriteSwap,
      Animation
    }
    
    // Graphic that will be colored.
    [SerializeField]
    private Graphic targetGraphic;
    // Type of the transition that occurs when the button state changes.
    [SerializeField]
    [ShowIf("@targetGraphic != null")]
    private TransitionType transition = TransitionType.ColorTint;

    // Colors used for a color tint-based transition.
    [SerializeField]
    [ShowIf("@targetGraphic != null && transition == TransitionType.ColorTint")]
    private ColorBlock colors = ColorBlock.defaultColorBlock;

    // Sprites used for a Image swap-based transition.
    [SerializeField]
    [ShowIf("@targetGraphic != null && transition == TransitionType.SpriteSwap")]
    private SpriteState spriteState;

    [SerializeField]
    [ShowIf("@targetGraphic != null && transition == TransitionType.Animation")]
    private AnimationTriggers animationTriggers = new AnimationTriggers();

    private bool groupsAllowInteraction = true;
    private int currentIndex = -1;

    public TransitionType Transition
    {
      get => transition;
      set
      {
        if (SetStruct(ref transition, value)) OnSetProperty();
      }
    }

    public ColorBlock Colors
    {
      get => colors;
      set
      {
        if (SetStruct(ref colors, value)) OnSetProperty();
      }
    }

    public SpriteState SpriteState
    {
      get => spriteState;
      set
      {
        if (SetStruct(ref spriteState, value)) OnSetProperty();
      }
    }

    public AnimationTriggers AnimationTriggers
    {
      get => animationTriggers;
      set
      {
        if (SetClass(ref animationTriggers, value)) OnSetProperty();
      }
    }

    public Graphic TargetGraphic
    {
      get => targetGraphic;
      set
      {
        if (SetClass(ref targetGraphic, value)) OnSetProperty();
      }
    }

    [field: SerializeField, OnValueChanged(nameof(OnInteractableChangedEditor))]
    public SerializableReactiveProperty<bool> Interactable { get; private set; } = new(true);

    private IDisposable localInteractableSub;

    private void OnInteractableChangedEditor(SerializableReactiveProperty<bool> prop) => OnInteractableChanged(prop.Value);
    private void OnInteractableChanged(bool value)
    {
      if (!value && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        EventSystem.current.SetSelectedGameObject(null);
      OnSetProperty();
    }

    private bool IsPointerInside { get; set; }
    private bool IsPointerDown { get; set; }
    private bool HasSelection { get; set; }

    /// <summary>
    /// Convenience function that converts the referenced Graphic to a Image, if possible.
    /// </summary>
    public Image Image
    {
      get => targetGraphic as Image;
      set => targetGraphic = value;
    }

#if PACKAGE_ANIMATION
    public Animator animator
    {
      get { return GetComponent<Animator>(); }
    }
#endif

    protected override void Awake()
    {
      if (targetGraphic == null)
        targetGraphic = GetComponent<Graphic>();

      localInteractableSub = Interactable.Subscribe(OnInteractableChanged);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      localInteractableSub?.Dispose();
    }

    private readonly List<CanvasGroup> mCanvasGroupCache = new List<CanvasGroup>();

    protected override void OnCanvasGroupChanged()
    {
      var parentGroupAllowsInteraction = ParentGroupAllowsInteraction();

      if (parentGroupAllowsInteraction != groupsAllowInteraction)
      {
        groupsAllowInteraction = parentGroupAllowsInteraction;
        OnSetProperty();
      }
    }

    private bool ParentGroupAllowsInteraction()
    {
      Transform t = transform;
      while (t != null)
      {
        t.GetComponents(mCanvasGroupCache);
        for (var i = 0; i < mCanvasGroupCache.Count; i++)
        {
          if (mCanvasGroupCache[i].enabled && !mCanvasGroupCache[i].interactable)
            return false;

          if (mCanvasGroupCache[i].ignoreParentGroups)
            return true;
        }

        t = t.parent;
      }

      return true;
    }

    public virtual bool IsInteractable()
    {
      return groupsAllowInteraction && Interactable.Value;
    }

    // Call from unity if animation properties have changed
    protected override void OnDidApplyAnimationProperties()
    {
      OnSetProperty();
    }

    // Select on enable and add to the list.
    protected override void OnEnable()
    {
      //Check to avoid multiple OnEnable() calls for each selectable
      if (mEnableCalled)
        return;

      base.OnEnable();

      if (selectableCount == selectables.Length)
      {
        SelectableExtended[] temp = new SelectableExtended[selectables.Length * 2];
        Array.Copy(selectables, temp, selectables.Length);
        selectables = temp;
      }

      if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
      {
        HasSelection = true;
      }

      currentIndex = selectableCount;
      selectables[currentIndex] = this;
      selectableCount++;
      IsPointerDown = false;
      groupsAllowInteraction = ParentGroupAllowsInteraction();
      DoStateTransition(CurrentSelectionState, true);

      mEnableCalled = true;
    }

    protected override void OnTransformParentChanged()
    {
      base.OnTransformParentChanged();

      // If our parenting changes figure out if we are under a new CanvasGroup.
      OnCanvasGroupChanged();
    }

    private void OnSetProperty()
    {
#if UNITY_EDITOR
      if (!Application.isPlaying)
        DoStateTransition(CurrentSelectionState, true);
      else
#endif
        DoStateTransition(CurrentSelectionState, false);
    }

    // Remove from the list.
    protected override void OnDisable()
    {
      //Check to avoid multiple OnDisable() calls for each selectable
      if (!mEnableCalled)
        return;

      selectableCount--;

      // Update the last elements index to be this index
      selectables[selectableCount].currentIndex = currentIndex;

      // Swap the last element and this element
      selectables[currentIndex] = selectables[selectableCount];

      // null out last element.
      selectables[selectableCount] = null;

      InstantClearState();
      base.OnDisable();

      mEnableCalled = false;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
      if (!hasFocus && IsPressed())
      {
        InstantClearState();
      }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
      base.OnValidate();
      colors.fadeDuration = Mathf.Max(colors.fadeDuration, 0.0f);

      // OnValidate can be called before OnEnable, this makes it unsafe to access other components
      // since they might not have been initialized yet.
      // OnSetProperty potentially access Animator or Graphics. (case 618186)
      if (isActiveAndEnabled)
      {
        if (!Interactable.Value && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
          EventSystem.current.SetSelectedGameObject(null);
        // Need to clear out the override image on the target...
        DoSpriteSwap(null);

        // If the transition mode got changed, we need to clear all the transitions, since we don't know what the old transition mode was.
        StartColorTween(Color.white, true);
        TriggerAnimation(animationTriggers.normalTrigger);

        // And now go to the right state.
        DoStateTransition(CurrentSelectionState, true);
      }
    }

    protected override void Reset()
    {
      targetGraphic = GetComponent<Graphic>();
    }

#endif // if UNITY_EDITOR

    protected SelectionState CurrentSelectionState
    {
      get
      {
        if (!IsInteractable())
          return SelectionState.Disabled;
        if (IsPointerDown)
          return SelectionState.Pressed;
        if (HasSelection)
          return SelectionState.Selected;
        if (IsPointerInside)
          return SelectionState.Highlighted;
        return SelectionState.Normal;
      }
    }

    /// <summary>
    /// Clear any internal state from the Selectable (used when disabling).
    /// </summary>
    protected virtual void InstantClearState()
    {
      string triggerName = animationTriggers.normalTrigger;

      IsPointerInside = false;
      IsPointerDown = false;
      HasSelection = false;

      switch (transition)
      {
        case TransitionType.ColorTint:
          StartColorTween(Color.white, true);
          break;
        case TransitionType.SpriteSwap:
          DoSpriteSwap(null);
          break;
        case TransitionType.Animation:
          TriggerAnimation(triggerName);
          break;
      }
    }

    /// <summary>
    /// Transition the Selectable to the entered state.
    /// </summary>
    /// <param name="state">State to transition to</param>
    /// <param name="instant">Should the transition occur instantly.</param>
    protected virtual void DoStateTransition(SelectionState state, bool instant)
    {
      if (!gameObject.activeInHierarchy)
        return;

      Color tintColor;
      Sprite transitionSprite;
      string triggerName;

      switch (state)
      {
        case SelectionState.Normal:
          tintColor = colors.normalColor;
          transitionSprite = null;
          triggerName = animationTriggers.normalTrigger;
          break;
        case SelectionState.Highlighted:
          tintColor = colors.highlightedColor;
          transitionSprite = spriteState.highlightedSprite;
          triggerName = animationTriggers.highlightedTrigger;
          break;
        case SelectionState.Pressed:
          tintColor = colors.pressedColor;
          transitionSprite = spriteState.pressedSprite;
          triggerName = animationTriggers.pressedTrigger;
          break;
        case SelectionState.Selected:
          tintColor = colors.selectedColor;
          transitionSprite = spriteState.selectedSprite;
          triggerName = animationTriggers.selectedTrigger;
          break;
        case SelectionState.Disabled:
          tintColor = colors.disabledColor;
          transitionSprite = spriteState.disabledSprite;
          triggerName = animationTriggers.disabledTrigger;
          break;
        default:
          tintColor = Color.black;
          transitionSprite = null;
          triggerName = string.Empty;
          break;
      }

      switch (transition)
      {
        case TransitionType.ColorTint:
          StartColorTween(tintColor * colors.colorMultiplier, instant);
          break;
        case TransitionType.SpriteSwap:
          DoSpriteSwap(transitionSprite);
          break;
        case TransitionType.Animation:
          TriggerAnimation(triggerName);
          break;
      }
    }
    
    protected enum SelectionState
    {
      Normal,
      Highlighted,
      Pressed,
      Selected,
      Disabled,
    }

    private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
    {
      if (rect == null)
        return Vector3.zero;
      if (dir != Vector2.zero)
        dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
      dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
      return dir;
    }

    private void StartColorTween(Color targetColor, bool instant)
    {
      if (targetGraphic == null)
        return;

      targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
    }

    private void DoSpriteSwap(Sprite newSprite)
    {
      if (Image == null)
        return;

      Image.overrideSprite = newSprite;
    }

    private void TriggerAnimation(string triggername)
    {
#if PACKAGE_ANIMATION
      if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
          return;

      animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
      animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
      animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
      animator.ResetTrigger(m_AnimationTriggers.selectedTrigger);
      animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);

      animator.SetTrigger(triggername);
#endif
    }

    protected bool IsHighlighted()
    {
      if (!IsActive() || !IsInteractable())
        return false;
      return IsPointerInside && !IsPointerDown && !HasSelection;
    }

    /// <summary>
    /// Whether the current selectable is being pressed.
    /// </summary>
    protected bool IsPressed()
    {
      if (!IsActive() || !IsInteractable())
        return false;
      return IsPointerDown;
    }

    // Change the button to the correct state
    private void EvaluateAndTransitionToSelectionState()
    {
      if (!IsActive() || !IsInteractable())
        return;

      DoStateTransition(CurrentSelectionState, false);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;

      IsPointerDown = true;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;

      IsPointerDown = false;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      IsPointerInside = true;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      IsPointerInside = false;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
      HasSelection = true;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
      HasSelection = false;
      EvaluateAndTransitionToSelectionState();
    }

    public virtual void Select()
    {
      if (EventSystem.current == null || EventSystem.current.alreadySelecting)
        return;

      EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public static bool SetColor(ref Color currentValue, Color newValue)
    {
      if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
        return false;

      currentValue = newValue;
      return true;
    }

    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    {
      if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
        return false;

      currentValue = newValue;
      return true;
    }

    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
    {
      if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
        return false;

      currentValue = newValue;
      return true;
    }
  }
}
