using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        anim?.SetBool("Pressed", false);

        if (!IsActive() || !IsInteractable())
            return;

        SoundManager.Play2DSound(clickSound);
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;

        anim?.SetBool("Pressed", true);
    }
}
