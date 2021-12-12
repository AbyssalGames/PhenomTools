using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ReferenceFitter : ContentSizeFitter
    {
        public RectTransform reference = null;
        public LayoutElement layout = null;

        public float additionalVerticalSize;
        public float additionalHorizontalSize;

        private float heightCache;
        private float widthCache;

        private void Update()
        {
            if (reference == null || layout == null || layout.ignoreLayout)
                return;

            if (verticalFit == FitMode.PreferredSize)
            {
                layout.preferredHeight = reference.rect.height + additionalVerticalSize;

                if(layout.minHeight >= 0f)
                    layout.minHeight = -1f;
            }
            else if (verticalFit == FitMode.MinSize)
            {
                layout.minHeight = reference.rect.height + additionalVerticalSize;

                if (layout.preferredHeight >= 0f)
                    layout.preferredHeight = -1f;
            }
            else if (verticalFit == FitMode.Unconstrained)
            {
                if (layout.preferredHeight >= 0f)
                    layout.preferredHeight = -1f;

                if (layout.minHeight >= 0f)
                    layout.minHeight = -1f;
            }

            if (horizontalFit == FitMode.PreferredSize)
            {
                layout.preferredWidth = reference.rect.width + additionalHorizontalSize;

                if (layout.minWidth >= 0f)
                    layout.minWidth = -1f;
            }
            else if (horizontalFit == FitMode.MinSize)
            {
                layout.minWidth = reference.rect.width + additionalHorizontalSize;

                if (layout.preferredWidth >= 0f)
                    layout.preferredWidth = -1f;
            }
            else if (horizontalFit == FitMode.Unconstrained)
            {
                if (layout.preferredWidth >= 0f)
                    layout.preferredWidth = -1f;

                if (layout.minWidth >= 0f)
                    layout.minWidth = -1f;
            }
        }

        protected override void Reset()
        {
            layout = GetComponent<LayoutElement>();
        }
    }
}
