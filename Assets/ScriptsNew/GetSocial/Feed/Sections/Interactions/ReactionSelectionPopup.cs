using System;
using UnityEngine;
using PhenomTools;
using DG.Tweening;
using System.Linq;

namespace BlackBoxVR.GetSocial
{
    public class ReactionSelectionPopup : MonoBehaviour
    {
        [NonSerialized]
        public bool isVisible;

        [SerializeField]
        private CanvasGroup canvasGroup = null;
        [SerializeField]
        private ReactionSelectionIcon[] icons = null;

        private ReactionToggle reactionToggle;
        private int hoveredIndex = -1;

        public void Initialize(ReactionToggle reactionToggle)
        {
            this.reactionToggle = reactionToggle;

            for (int i = 0; i < icons.Length; i++)
                icons[i].Initialize(this, i);
        }

        public void Show()
        {
            isVisible = true;

            canvasGroup.SetInteractable(true);
            transform.DOScale(1f, .25f);
        }

        public void Hide()
        {
            isVisible = false;

            if (hoveredIndex > -1)
                reactionToggle.SetReaction(hoveredIndex);

            canvasGroup.SetInteractable(false);
            transform.DOScale(0f, .25f);
        }

        public void OnHoverIcon(int index)
        {
            hoveredIndex = index;
        }

        public void OnUnhoverIcon()
        {
            PhenomUtils.DelayActionByFrames(1, () =>
            {
                if (!icons.Any(icon => icon.isHovered))
                    hoveredIndex = -1;
            });
        }
    }
}
