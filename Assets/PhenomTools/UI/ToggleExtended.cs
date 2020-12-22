using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ToggleExtended : Toggle
    {
        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onDown = new UnityEvent();
        public UnityEvent onUp = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onReenter = new UnityEvent();
        public UnityEventBool onGhostToggle = new UnityEventBool();

        [SerializeField]
        private Sound hoverSound = null;
        [SerializeField]
        private Sound downSound = null;
        [SerializeField]
        private Sound clickSound = null;
        [SerializeField]
        private Sound toggleOnSound = null;
        [SerializeField]
        private Sound toggleOffSound = null;

        [HideInInspector]
        public bool isPressed;

        protected override void OnEnable()
        {
            onValueChanged.AddListener(OnValueChanged);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            onValueChanged.RemoveListener(OnValueChanged);
            base.OnDisable();
        }

        public void SetParameters(Toggle t, Graphic targetGraphic, Graphic graphic, ToggleEvent onValueChanged)
        {
            interactable = t.interactable;

            transition = t.transition;
            this.targetGraphic = targetGraphic;
            colors = t.colors;

            spriteState = t.spriteState;
            animationTriggers = t.animationTriggers;

            navigation = t.navigation;

            isOn = t.isOn;
            toggleTransition = t.toggleTransition;
            this.graphic = graphic;
            group = t.group;

            this.onValueChanged = onValueChanged;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            onHover?.Invoke();
            SoundManager.Play2DSound(hoverSound);

            if (isPressed)
                onReenter?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (!IsActive() || !IsInteractable())
                return;

            isPressed = true;
            SoundManager.Play2DSound(downSound);

            onDown?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (isPressed)
                onExit?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            onUp?.Invoke();
            isPressed = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!IsActive() || !IsInteractable())
                return;

            SoundManager.Play2DSound(clickSound);
        }

        private void OnValueChanged(bool on)
        {
            SoundManager.Play2DSound(on ? toggleOnSound : toggleOffSound);
        }

        public void DoClick(bool? on = null)
        {
            if (!IsActive() || !IsInteractable())
                return;

            bool newValue = on == null ? !isOn : on.Value;

            SoundManager.Play2DSound(clickSound);
            SoundManager.Play2DSound(newValue ? toggleOnSound : toggleOffSound);

            onValueChanged?.Invoke(newValue);
        }

        /// <summary>
        /// Play animations and sounds without invoking onValueChanged
        /// </summary>
        public void GhostToggle(bool on, bool playSounds = false)
        {
            if (!IsActive() || !IsInteractable() || on == isOn)
                return;

            if (playSounds)
            {
                SoundManager.Play2DSound(clickSound);
                SoundManager.Play2DSound(on ? toggleOnSound : toggleOffSound);
            }

            onGhostToggle?.Invoke(on);
            SetIsOnWithoutNotify(on);
        }

        //public Sound onSound;
        //public Sound offSound;

        //public void SetParameters(Toggle t, Graphic targetGraphic, Graphic graphic, ToggleEvent onValueChanged)
        //{
        //    interactable = t.interactable;

        //    transition = t.transition;
        //    this.targetGraphic = targetGraphic;
        //    colors = t.colors;

        //    spriteState = t.spriteState;
        //    animationTriggers = t.animationTriggers;

        //    navigation = t.navigation;

        //    isOn = t.isOn;
        //    toggleTransition = t.toggleTransition;
        //    this.graphic = graphic;
        //    group = t.group;

        //    this.onValueChanged = onValueChanged;
        //}

        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    onValueChanged.AddListener(OnValueChanged);
        //}

        //protected override void OnDisable()
        //{
        //    base.OnDisable();
        //    onValueChanged.RemoveListener(OnValueChanged);
        //}

        //public virtual void OnValueChanged(bool on)
        //{
        //    SoundManager.Play2DSound(on ? onSound : offSound);
        //}

        //protected override void DoStateTransition(SelectionState state, bool instant)
        //{
        //    base.DoStateTransition(state, instant);

        //    if (graphic != null && targetGraphic != null)
        //        graphic.color = GetColorOfSelectionState(state);
        //}

        //private Color GetColorOfSelectionState(SelectionState state)
        //{
        //    switch (state)
        //    {
        //        default:
        //        case SelectionState.Normal:
        //            return colors.normalColor;
        //        case SelectionState.Highlighted:
        //            return colors.highlightedColor;
        //        case SelectionState.Pressed:
        //            return colors.pressedColor;
        //        case SelectionState.Selected:
        //            return colors.selectedColor;
        //        case SelectionState.Disabled:
        //            return colors.disabledColor;
        //    }
        //}
    }
}