using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ButtonExtended : Button
    {
        [SerializeField]
        private Sound clickSound = null;

        private Animator anim;

        protected override void Awake()
        {
            anim = animator;
        }

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!IsActive() || !IsInteractable())
                return;

            SoundManager.Play2DSound(clickSound);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if(anim != null)
                anim.SetBool("Pressed", false);
        }

        public void OnPointerDown(BaseEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (anim != null)
                anim.SetBool("Pressed", true);
        }

        public void Click()
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (anim != null)
                anim.SetBool("Pressed", true);

            SoundManager.Play2DSound(clickSound);

            onClick?.Invoke();

            PhenomUtils.DelayActionByTime(.1f, () =>
            {
                if (anim != null)
                    anim.SetBool("Pressed", false);
            });
        }

        /// <summary>
        /// Play animations and sounds without invoking onClick
        /// </summary>
        public void GhostClick()
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (anim != null)
                anim.SetBool("Pressed", true);

            SoundManager.Play2DSound(clickSound);

            PhenomUtils.DelayActionByTime(.1f, () => 
            {
                if (anim != null)
                    anim.SetBool("Pressed", false);
            });
        }
    }
}