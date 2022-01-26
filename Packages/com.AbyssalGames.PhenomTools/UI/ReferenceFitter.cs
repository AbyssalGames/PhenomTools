using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
//[RequireComponent(typeof(LayoutElement))]
public class ReferenceFitter : ContentSizeFitter
{
    public RectTransform reference = null;
    //public LayoutElement layout = null;

    public float additionalVerticalSize;
    public float additionalHorizontalSize;

    [System.NonSerialized] private RectTransform m_Rect;
    private RectTransform rectTransform
    {
        get
        {
            if (m_Rect == null)
                m_Rect = transform as RectTransform;
            return m_Rect;
        }
    }

    public override void SetLayoutHorizontal()
    {
        if (reference == null /*|| layout == null || layout.ignoreLayout*/)
            return;

        if (horizontalFit == FitMode.PreferredSize)
        {
            float preferredWidth = reference.rect.width + additionalHorizontalSize;

            //if (layout.minWidth >= 0f)
            //    layout.minWidth = -1f;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth);
        }
        else if (horizontalFit == FitMode.MinSize)
        {
            float minWidth = reference.rect.width + additionalHorizontalSize;

            //if (layout.preferredWidth >= 0f)
            //    layout.preferredWidth = -1f;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
        }
        //else if (horizontalFit == FitMode.Unconstrained)
        //{
        //    if (layout.preferredWidth >= 0f)
        //        layout.preferredWidth = -1f;

        //    if (layout.minWidth >= 0f)
        //        layout.minWidth = -1f;
        //}
    }

    public override void SetLayoutVertical()
    {
        if (reference == null /*|| layout == null || layout.ignoreLayout*/)
            return;

        if (verticalFit == FitMode.PreferredSize)
        {
            float preferredHeight = reference.rect.height + additionalVerticalSize;

            //if (layout.minHeight >= 0f)
            //    layout.minHeight = -1f;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
        }
        else if (verticalFit == FitMode.MinSize)
        {
            float minHeight = reference.rect.height + additionalVerticalSize;

            //if (layout.preferredHeight >= 0f)
            //    layout.preferredHeight = -1f;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
        }
        //else if (verticalFit == FitMode.Unconstrained)
        //{
        //    if (layout.preferredHeight >= 0f)
        //        layout.preferredHeight = -1f;

        //    if (layout.minHeight >= 0f)
        //        layout.minHeight = -1f;
        //}
    }

    //public override void SetLayoutHorizontal()
    //{
    //    if (reference == null || layout == null || layout.ignoreLayout)
    //        return;

    //    if (horizontalFit == FitMode.PreferredSize)
    //    {
    //        layout.preferredWidth = reference.rect.width + additionalHorizontalSize;

    //        if (layout.minWidth >= 0f)
    //            layout.minWidth = -1f;

    //        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredSize(m_Rect, 0));
    //    }
    //    else if (horizontalFit == FitMode.MinSize)
    //    {
    //        layout.minWidth = reference.rect.width + additionalHorizontalSize;

    //        if (layout.preferredWidth >= 0f)
    //            layout.preferredWidth = -1f;

    //        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetMinSize(m_Rect, 0));
    //    }
    //    else if (horizontalFit == FitMode.Unconstrained)
    //    {
    //        if (layout.preferredWidth >= 0f)
    //            layout.preferredWidth = -1f;

    //        if (layout.minWidth >= 0f)
    //            layout.minWidth = -1f;
    //    }
    //}

    //public override void SetLayoutVertical()
    //{
    //    if (reference == null || layout == null || layout.ignoreLayout)
    //        return;

    //    if (verticalFit == FitMode.PreferredSize)
    //    {
    //        layout.preferredHeight = reference.rect.height + additionalVerticalSize;

    //        if (layout.minHeight >= 0f)
    //            layout.minHeight = -1f;

    //        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredSize(m_Rect, 1));
    //    }
    //    else if (verticalFit == FitMode.MinSize)
    //    {
    //        layout.minHeight = reference.rect.height + additionalVerticalSize;

    //        if (layout.preferredHeight >= 0f)
    //            layout.preferredHeight = -1f;

    //        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetMinSize(m_Rect, 1));
    //    }
    //    else if (verticalFit == FitMode.Unconstrained)
    //    {
    //        if (layout.preferredHeight >= 0f)
    //            layout.preferredHeight = -1f;

    //        if (layout.minHeight >= 0f)
    //            layout.minHeight = -1f;
    //    }
    //}

    //private void Update()
    //{
    //    if (reference == null || layout == null || layout.ignoreLayout)
    //        return;

    //    if (verticalFit == FitMode.PreferredSize)
    //    {
    //        layout.preferredHeight = reference.rect.height + additionalVerticalSize;

    //        if(layout.minHeight >= 0f)
    //            layout.minHeight = -1f;
    //    }
    //    else if (verticalFit == FitMode.MinSize)
    //    {
    //        layout.minHeight = reference.rect.height + additionalVerticalSize;

    //        if (layout.preferredHeight >= 0f)
    //            layout.preferredHeight = -1f;
    //    }
    //    else if (verticalFit == FitMode.Unconstrained)
    //    {
    //        if (layout.preferredHeight >= 0f)
    //            layout.preferredHeight = -1f;

    //        if (layout.minHeight >= 0f)
    //            layout.minHeight = -1f;
    //    }

    //    if (horizontalFit == FitMode.PreferredSize)
    //    {
    //        layout.preferredWidth = reference.rect.width + additionalHorizontalSize;

    //        if (layout.minWidth >= 0f)
    //            layout.minWidth = -1f;
    //    }
    //    else if (horizontalFit == FitMode.MinSize)
    //    {
    //        layout.minWidth = reference.rect.width + additionalHorizontalSize;

    //        if (layout.preferredWidth >= 0f)
    //            layout.preferredWidth = -1f;
    //    }
    //    else if (horizontalFit == FitMode.Unconstrained)
    //    {
    //        if (layout.preferredWidth >= 0f)
    //            layout.preferredWidth = -1f;

    //        if (layout.minWidth >= 0f)
    //            layout.minWidth = -1f;
    //    }
    //}

//#if UNITY_EDITOR
//    protected override void Reset()
//    {
//        layout = GetComponent<LayoutElement>();
//    }
//#endif
}

