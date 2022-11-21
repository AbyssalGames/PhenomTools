using System;
using UnityEngine;
using DG.Tweening;

namespace BlackBoxVR.GetSocial
{
    public class ReactionSelectionIcon : MonoBehaviour
    {
        [NonSerialized]
        public bool isHovered = false;

        private ReactionSelectionPopup popup;
        private int index = -1;

        public void Initialize(ReactionSelectionPopup popup, int index)
        {
            this.popup = popup;
            this.index = index;
        }

        public void OnHover()
        {
            isHovered = true;
            popup.OnHoverIcon(index);

            transform.SetAsLastSibling();
            transform.DOScale(1.2f, .25f);
        }

        public void OnExit()
        {
            isHovered = false;
            popup.OnUnhoverIcon();

            transform.DOScale(1f, .25f);
        }
    }
}
