﻿using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools.UI
{
  public static class ScrollRectUtility
  {
    /// <summary>
    /// Position a rect in its scroll rect such that it is fully visible.
    /// If rect is already visible rect is not scrolled
    /// </summary>
    /// <param name="rectTransform">Rect to be made visible. Rect must have a ScrollRect parent.</param>
    public static void MakeVisible(RectTransform rectTransform)
    {
      ScrollRect scrollRect = rectTransform.gameObject.GetComponentInParent<ScrollRect>();
      if (scrollRect == null)
      {
        Debug.LogError("Parent ScrollRect Not Found.");
        return;
      }

      Canvas.ForceUpdateCanvases();
      RectTransform viewport = scrollRect.viewport;

      //Get Points relative to scroll rect
      Vector2 contentPtInViewport = (Vector2)scrollRect.viewport.InverseTransformPoint(scrollRect.content.position);
      Vector2 targetPtInViewport = (Vector2)viewport.InverseTransformPoint(rectTransform.position);

      //Here we calculate the content anchor point that puts our RectTransform's pivot at the top left of the scroll view
      Vector2 newAnchorPoint = contentPtInViewport - targetPtInViewport;

      float targetTop = targetPtInViewport.y;
      targetTop += rectTransform.rect.height * (1 - rectTransform.pivot.y);

      float targetBottom = targetPtInViewport.y;
      targetBottom -= rectTransform.rect.height * rectTransform.pivot.y;

      float targetLeft = targetPtInViewport.x;
      targetLeft -= rectTransform.rect.width * rectTransform.pivot.x;

      float targetRight = targetPtInViewport.x;
      targetRight += rectTransform.rect.width * (1 - rectTransform.pivot.x);

      float viewportTop = viewport.rect.height * (1 - viewport.pivot.y);
      float viewportBottom = -viewport.rect.height * viewport.pivot.y;
      float viewportLeft = viewport.rect.width * viewport.pivot.x;
      float viewportRight = viewport.rect.width * (1 - viewport.pivot.x);

      if (!scrollRect.vertical)
      {
        newAnchorPoint.y = scrollRect.content.anchoredPosition.y;
      }
      else if (targetTop > viewportTop)
      {
        newAnchorPoint.y -= rectTransform.rect.height * (1 - rectTransform.pivot.y);
      }
      else if (targetBottom < viewportBottom)
      {
        newAnchorPoint.y -= viewport.rect.height;
        newAnchorPoint.y += rectTransform.rect.height * rectTransform.pivot.y;
      }
      else
      {
        //No need to scroll
        newAnchorPoint.y = scrollRect.content.anchoredPosition.y;
      }

      if (!scrollRect.horizontal)
      {
        newAnchorPoint.x = scrollRect.content.anchoredPosition.x;
      }
      else if (targetLeft < viewportLeft)
      {
        newAnchorPoint.x += rectTransform.rect.width * rectTransform.pivot.x;
      }
      else if (targetRight > viewportRight)
      {
        newAnchorPoint.x += viewport.rect.width;
        newAnchorPoint.x -= rectTransform.rect.width * (1 - rectTransform.pivot.x);
      }
      else
      {
        //No need to scroll
        newAnchorPoint.x = scrollRect.content.anchoredPosition.x;
      }

      //Check if we've overstretched our bounds and get the offset that will put us into valid bounds
      //This prevents us from over scrolling and seeing bounce back
      Bounds viewportBounds = scrollRect.GetViewportBounds();
      Bounds contentBounds = scrollRect.GetContentBounds();
      Vector2 delta = newAnchorPoint - scrollRect.content.anchoredPosition;
      Vector2 offset = CalculateOffset(ref viewportBounds, ref contentBounds, scrollRect.horizontal,
          scrollRect.vertical, ref delta);

      scrollRect.velocity = Vector2.zero;
      scrollRect.content.anchoredPosition = newAnchorPoint + offset;
    }

    public static void CenterOn(this ScrollRect scrollRect, RectTransform rectTransform) => CenterInScrollView(rectTransform, scrollRect);

    /// <summary>
    /// Centers a rect transform within the viewport of its parent scroll rect
    /// </summary>
    /// <param name="rectTransform">rect transform to be centered</param>
    public static void CenterInScrollView(RectTransform rectTransform, ScrollRect scrollRect = null)
    {
      if (scrollRect == null)
        scrollRect = rectTransform.gameObject.GetComponentInParent<ScrollRect>();

      if(rectTransform == null)
        scrollRect.content.anchoredPosition = Vector2.zero;
      
      scrollRect.velocity = Vector2.zero;
      scrollRect.content.anchoredPosition = GetPositionInScrollView(rectTransform, scrollRect);
    }

    /// <summary>
    /// Makes a rect visible and positions it within the viewport of its parent rect
    /// </summary>
    /// <param name="rectTransform">rect to be made visible</param>
    /// <param name="position">position in viewport as a relative percentage. (1,1) is top left. (0,0) is bottom right.</param>
    public static Vector2 GetPositionInScrollView(RectTransform rectTransform, ScrollRect scrollRect = null)
    {
      if (scrollRect == null)
        scrollRect = rectTransform.gameObject.GetComponentInParent<ScrollRect>();

      if (scrollRect == null)
      {
        Debug.LogError("Parent ScrollRect Not Found.");
        return Vector2.zero;
      }

      Canvas.ForceUpdateCanvases();
      RectTransform viewport = scrollRect.viewport;

      //Get Points relative to scroll rect
      Vector2 contentPtInViewport = (Vector2)scrollRect.viewport.InverseTransformPoint(scrollRect.content.position);
      Vector2 targetPtInViewport = (Vector2)viewport.InverseTransformPoint(rectTransform.position);

      //Calculate the content anchor point that puts our RectTransform's pivot at the top left of the scroll view
      Vector2 newAnchorPoint = contentPtInViewport - targetPtInViewport;

      Vector2 topLeftAnchorPoint = newAnchorPoint;
      topLeftAnchorPoint.y -= rectTransform.rect.height * (1 - rectTransform.pivot.y);
      topLeftAnchorPoint.x += rectTransform.rect.width * rectTransform.pivot.x;

      Vector2 bottomRightAnchorPoint = newAnchorPoint;
      Rect viewportRect = viewport.rect;
      bottomRightAnchorPoint.y -= viewportRect.height;
      bottomRightAnchorPoint.y += rectTransform.rect.height * rectTransform.pivot.y;
      bottomRightAnchorPoint.x += viewportRect.width;
      bottomRightAnchorPoint.x -= rectTransform.rect.width * (1 - rectTransform.pivot.x);

      newAnchorPoint.x = Mathf.Lerp(bottomRightAnchorPoint.x, topLeftAnchorPoint.x, .5f);
      newAnchorPoint.y = Mathf.Lerp(bottomRightAnchorPoint.y, topLeftAnchorPoint.y, .5f);

      if (!scrollRect.vertical)
      {
        newAnchorPoint.y = scrollRect.content.anchoredPosition.y;
      }

      if (!scrollRect.horizontal)
      {
        newAnchorPoint.x = scrollRect.content.anchoredPosition.x;
      }

      //Calculate the bounds of our scroll view and if we've extended beyond them.
      //This prevents overscrolling the content and causing the position to bounce back
      Bounds viewportBounds = scrollRect.GetViewportBounds();
      Bounds contentBounds = scrollRect.GetContentBounds();
      Vector2 delta = newAnchorPoint - scrollRect.content.anchoredPosition;
      Vector2 offset = CalculateOffset(ref viewportBounds, ref contentBounds, scrollRect.horizontal,
          scrollRect.vertical, ref delta);

      return newAnchorPoint + offset;
    }

    private static Bounds GetViewportBounds(this ScrollRect scrollRect)
    {
      Rect rect = scrollRect.viewport.rect;
      return new Bounds(rect.center, rect.size);
    }

    private static Bounds GetContentBounds(this ScrollRect scrollRect)
    {
      return GetBounds(scrollRect.viewport, scrollRect.content);
    }

    private static readonly Vector3[] corners = new Vector3[4];
    private static Bounds GetBounds(RectTransform viewRect, RectTransform contentRect)
    {
      if (contentRect == null)
      {
        return new Bounds();
      }
      contentRect.GetWorldCorners(corners);
      Matrix4x4 worldToLocalMatrix = viewRect.worldToLocalMatrix;
      return InternalGetBounds(corners, ref worldToLocalMatrix);
    }

    private static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
    {
      Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 vector3_2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      for (int index = 0; index < 4; ++index)
      {
        Vector3 lhs = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[index]);
        vector3_1 = Vector3.Min(lhs, vector3_1);
        vector3_2 = Vector3.Max(lhs, vector3_2);
      }
      Bounds bounds = new Bounds(vector3_1, Vector3.zero);
      bounds.Encapsulate(vector3_2);
      return bounds;
    }

    private static Vector2 CalculateOffset(ref Bounds viewBounds, ref Bounds contentBounds, bool horizontal, bool vertical, ref Vector2 delta)
    {
      Vector2 zero = Vector2.zero;
      Vector2 min = contentBounds.min;
      Vector2 max = contentBounds.max;
      if (horizontal)
      {
        min.x += delta.x;
        max.x += delta.x;
        float num1 = viewBounds.max.x - max.x;
        float num2 = viewBounds.min.x - min.x;
        if (num2 < -1.0 / 1000.0)
        {
          zero.x = num2;
        }
        else if (num1 > 1.0 / 1000.0)
        {
          zero.x = num1;
        }
      }
      if (vertical)
      {
        min.y += delta.y;
        max.y += delta.y;
        float num1 = viewBounds.max.y - max.y;
        float num2 = viewBounds.min.y - min.y;
        if (num1 > 1.0 / 1000.0)
        {
          zero.y = num1;
        }
        else if (num2 < -1.0 / 1000.0)
        {
          zero.y = num2;
        }
      }
      return zero;
    }
  }
}

