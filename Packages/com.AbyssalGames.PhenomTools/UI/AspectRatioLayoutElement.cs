using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhenomTools;

namespace PhenomTools
{
    public class AspectRatioLayoutElement : LayoutElement
    {
        public enum AspectMode
        {
            None,
            WidthControlsHeight,
            HeightControlsWidth
        }

        [SerializeField]
        private AspectMode m_aspectMode = AspectMode.None;
        [SerializeField]
        private float m_aspectRatio = 1f;

        private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        public virtual AspectMode aspectMode { get { return m_aspectMode; } set { if (m_aspectMode != value) { m_aspectMode = value; SetDirty(); } } }
        public virtual float aspectRatio { get { return m_aspectRatio; } set { if (m_aspectRatio != value) { m_aspectRatio = value; SetDirty(); } } }

        public override void CalculateLayoutInputVertical()
        {
            Recalc();
        }
        public override void CalculateLayoutInputHorizontal()
        {
            Recalc();
        }

        private void Recalc()
        {
            if (aspectRatio == 0)
                return;

            if (aspectMode == AspectMode.WidthControlsHeight)
            {
                minHeight = rectTransform.rect.width / aspectRatio;
                preferredHeight = rectTransform.rect.width / aspectRatio;
            }
            else if (aspectMode == AspectMode.HeightControlsWidth)
            {
                minWidth = rectTransform.rect.height / aspectRatio;
                preferredWidth = rectTransform.rect.height / aspectRatio;
            }
        }
    }
}