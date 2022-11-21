using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PhenomTools
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DebugGroup : MonoBehaviour
    {
        [SerializeField]
        protected CanvasGroup canvasGroup = null;

        protected DebugPanel panel;

        public virtual void Initialize(DebugPanel panel)
        {
            this.panel = panel;
        }
        
        public virtual void Show()
        {
            canvasGroup.DOFade(1f, .25f).OnComplete(() => canvasGroup.SetInteractable(true));
        }

        public virtual void Hide()
        {
            canvasGroup.DOFade(0f, .25f).OnComplete(() => canvasGroup.SetInteractable(false));
        }

        public virtual void Back()
        {
            panel.SetDebugGroup(-1);
            Hide();
        }

        public virtual void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
