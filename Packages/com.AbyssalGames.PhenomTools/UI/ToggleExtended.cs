using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ToggleExtended : Toggle
    {
        public Graphic[] graphics = new Graphic[0];
        public float longPressDuration = .5f;
        public bool vibrateOnLongPress = false;

        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onDown = new UnityEvent();
        public UnityEvent onUp = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onReenter = new UnityEvent();
        public UnityEvent onLongPress = new UnityEvent();
        public UnityEventBool onGhostToggle = new UnityEventBool();

#if PhenomAudio
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
#endif

        [HideInInspector]
        public bool isPressed;

        private bool isPointerDown;
        private bool longPressTriggered = false;
        private float timePressStarted;

        protected override void OnEnable()
        {
            onValueChanged.AddListener(OnValueChanged);
            
            foreach (Graphic graphic in graphics)
                PlayEffect(graphic, true);
            
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            onValueChanged.RemoveListener(OnValueChanged);
            base.OnDisable();
        }

        private void Update()
        {
            if (isPointerDown && !longPressTriggered && Time.time - timePressStarted > longPressDuration)
            {
                longPressTriggered = true;
                
                if (vibrateOnLongPress)
                    Vibration.Vibrate(100);
                
                onLongPress.Invoke();
            }
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
#if PhenomAudio
            SoundManager.Play2DSound(hoverSound);
#endif

            if (isPressed)
            {
                isPointerDown = true;
                timePressStarted = Time.time;
                onReenter?.Invoke();
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
#if PhenomAudio
            SoundManager.Play2DSound(downSound);
#endif

            onDown?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            isPointerDown = false;

            //if (isPressed)
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

#if PhenomAudio
            SoundManager.Play2DSound(clickSound);
#endif
        }

        private void OnValueChanged(bool on)
        {
            foreach (Graphic graphic in graphics)
                PlayEffect(graphic, toggleTransition == ToggleTransition.None);

#if PhenomAudio
            SoundManager.Play2DSound(on ? toggleOnSound : toggleOffSound);
#endif
        }

        private void PlayEffect(Graphic graphic, bool instant)
        {
            if (graphic == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                graphic.canvasRenderer.SetAlpha(isOn ? 1f : 0f);
            else
#endif
                graphic.CrossFadeAlpha(isOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }

        public void DoClick(bool? on = null)
        {
            if (!IsActive() || !IsInteractable())
                return;

            bool newValue = on == null ? !isOn : on.Value;

#if PhenomAudio
            SoundManager.Play2DSound(clickSound);
            SoundManager.Play2DSound(newValue ? toggleOnSound : toggleOffSound);
#endif

            onValueChanged?.Invoke(newValue);
        }

        /// <summary>
        /// Play animations and sounds without invoking onValueChanged
        /// </summary>
        public void GhostToggle(bool on, bool playSounds = false)
        {
            if (!IsActive() || !IsInteractable() || on == isOn)
                return;

#if PhenomAudio
            if (playSounds)
            {
                SoundManager.Play2DSound(clickSound);
                SoundManager.Play2DSound(on ? toggleOnSound : toggleOffSound);
            }
#endif

            SetIsOnWithoutNotify(on);
            onGhostToggle?.Invoke(on);
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