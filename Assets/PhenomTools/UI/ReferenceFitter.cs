using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ReferenceFitter : ContentSizeFitter
    {
        [SerializeField]
        private RectTransform reference = null;
        [SerializeField]
        private LayoutElement layout = null;

        private void Update()
        {
            if (reference == null || layout == null)
                return;

            if (verticalFit == FitMode.PreferredSize)
                layout.preferredHeight = reference.rect.height;
        }
    }
}
