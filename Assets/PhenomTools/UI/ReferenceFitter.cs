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
        public RectTransform reference = null;
        public LayoutElement layout = null;

        private void Update()
        {
            if (reference == null || layout == null)
                return;

            if (verticalFit == FitMode.PreferredSize)
                layout.preferredHeight = reference.rect.height;
            else if (verticalFit == FitMode.MinSize)
                layout.minHeight = reference.rect.height;

            if (horizontalFit == FitMode.PreferredSize)
                layout.preferredWidth = reference.rect.width;
            else if (horizontalFit == FitMode.MinSize)
                layout.minWidth = reference.rect.width;
        }
    }
}
