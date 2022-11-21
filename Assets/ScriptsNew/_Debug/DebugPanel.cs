using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PhenomTools
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI versionText = null;
        [SerializeField]
        private TextMeshProUGUI fpsText = null;
        [SerializeField]
        private CanvasGroup panelCanvasGroup = null;
        [SerializeField]
        private CanvasGroup showButtonCanvasGroup = null;
        [SerializeField]
        private CanvasGroup groupSelectCanvasGroup = null;
        [SerializeField]
        private Transform panelTransform = null;
        [SerializeField]
        private DebugGroup[] groups = null;

        private int activeGroup = -1;
        private Vector3 panelOriginalPos;

        public void Start()
        {
            if (PhenomConsole.isDebugMode)
            {
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject);
                return;
            }
            
            foreach (DebugGroup debugGroup in groups)
                debugGroup.Initialize(this);
            
            versionText.SetText("v" + Application.version);
            StartCoroutine(DiagnosticsRoutine());
        }
        
        public void SetDebugGroup(int index)
        {
            if (groups.Length <= index || index == activeGroup)
                return;
            
            if(activeGroup >= 0)
                groups[activeGroup].Hide();

            if (index >= 0)
            {
                groupSelectCanvasGroup.DOFade(0f, .25f).OnComplete(() => groupSelectCanvasGroup.SetInteractable(false));
                groups[index].Show();
                activeGroup = index;
            }
            else
            {
                groupSelectCanvasGroup.DOFade(1f, .25f).OnComplete(() => groupSelectCanvasGroup.SetInteractable(true));
                activeGroup = index;
            }
        }

        private IEnumerator DiagnosticsRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(.2f);

            while (true) 
            { 
                fpsText.SetText("FPS:" + (1f / Time.deltaTime).ToString("F1"));
                // memoryText.SetText("Memory:" + Profiler.GetMonoUsedSizeLong().ToString() + "/" + Profiler.GetMonoHeapSizeLong().ToString());
                yield return wait;
            }
        }

        public void ToggleConsoleDisplay(bool on)
        {
            DOTween.Kill(panelCanvasGroup);
            DOTween.Kill(panelTransform);
            DOTween.Kill(showButtonCanvasGroup);

            if (on)
            {
                DOTween.To(() => panelCanvasGroup.alpha, a => panelCanvasGroup.alpha = a, 1f, .25f);
                panelCanvasGroup.blocksRaycasts = true;
                panelCanvasGroup.interactable = true;

                DOTween.To(() => showButtonCanvasGroup.alpha, a => showButtonCanvasGroup.alpha = a, 0f, .25f);
                showButtonCanvasGroup.blocksRaycasts = false;
                showButtonCanvasGroup.interactable = false;

                panelOriginalPos = panelTransform.localPosition;
                panelTransform.DOLocalMove(Vector3.zero, .25f);
            }
            else
            {
                DOTween.To(() => panelCanvasGroup.alpha, a => panelCanvasGroup.alpha = a, 0f, .25f);
                panelCanvasGroup.blocksRaycasts = false;
                panelCanvasGroup.interactable = false;

                DOTween.To(() => showButtonCanvasGroup.alpha, a => showButtonCanvasGroup.alpha = a, 1f, .25f);
                showButtonCanvasGroup.blocksRaycasts = true;
                showButtonCanvasGroup.interactable = true;

                panelTransform.DOLocalMove(panelOriginalPos, .25f);
            }
        }
    }
}
