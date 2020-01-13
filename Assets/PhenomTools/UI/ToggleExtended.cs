using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ToggleExtended : Toggle
    {
        public Sound onSound;
        public Sound offSound;

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

        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onValueChanged.RemoveListener(OnValueChanged);
        }

        public virtual void OnValueChanged(bool on)
        {
            SoundManager.Play2DSound(on ? onSound : offSound);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (graphic != null && targetGraphic != null)
                graphic.color = GetColorOfSelectionState(state);
        }

        private Color GetColorOfSelectionState(SelectionState state)
        {
            switch (state)
            {
                default:
                case SelectionState.Normal:
                    return colors.normalColor;
                case SelectionState.Highlighted:
                    return colors.highlightedColor;
                case SelectionState.Pressed:
                    return colors.pressedColor;
                case SelectionState.Selected:
                    return colors.selectedColor;
                case SelectionState.Disabled:
                    return colors.disabledColor;
            }
        }
    }
}