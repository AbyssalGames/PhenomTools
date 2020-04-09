using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ButtonExtended : Button
    {
        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onDown = new UnityEvent();
        public UnityEvent onUp = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onReenter = new UnityEvent();
        public UnityEvent onGhostClick = new UnityEvent();

        [SerializeField]
        private Sound hoverSound = null;
        [SerializeField]
        private Sound downSound = null;
        [SerializeField]
        private Sound clickSound = null;

        [HideInInspector]
        public bool isPressed;

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

            if(isPressed)
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

        public void DoClick()
        {
            if (!IsActive() || !IsInteractable())
                return;

            SoundManager.Play2DSound(clickSound);

            onClick?.Invoke();
        }

        /// <summary>
        /// Play animations and sounds without invoking onClick
        /// </summary>
        public void GhostClick()
        {
            if (!IsActive() || !IsInteractable())
                return;

            SoundManager.Play2DSound(clickSound);

            onGhostClick?.Invoke();
        }
    }
}