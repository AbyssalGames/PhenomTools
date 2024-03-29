﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ButtonExtended : Button
    {
        public float longPressDuration = .5f;
        public bool vibrateOnLongPress = false;

        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onDown = new UnityEvent();
        public UnityEvent onUp = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onReenter = new UnityEvent();
        public UnityEvent onLongPress = new UnityEvent();
        public UnityEvent onGhostClick = new UnityEvent();

#if PhenomAudio
        [SerializeField]
        private Sound hoverSound = null;
        [SerializeField]
        private Sound downSound = null;
        [SerializeField]
        private Sound clickSound = null;
#endif

        [HideInInspector]
        public bool isPressed;

        private bool isPointerDown;
        private bool longPressTriggered = false;
        private float timePressStarted;

        public void SetParameters(Button b, Graphic targetGraphic, ButtonClickedEvent onClick)
        {
            interactable = b.interactable;

            transition = b.transition;
            this.targetGraphic = targetGraphic;
            colors = b.colors;

            spriteState = b.spriteState;
            animationTriggers = b.animationTriggers;

            navigation = b.navigation;

            this.onClick = onClick;
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

        public void DoClick()
        {
            if (!IsActive() || !IsInteractable())
                return;

#if PhenomAudio
            SoundManager.Play2DSound(clickSound);
#endif

            onClick?.Invoke();
        }

        /// <summary>
        /// Play animations and sounds without invoking onClick
        /// </summary>
        public void GhostClick()
        {
            if (!IsActive() || !IsInteractable())
                return;

#if PhenomAudio
            SoundManager.Play2DSound(clickSound);
#endif

            onGhostClick?.Invoke();
        }
    }
}