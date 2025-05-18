using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools.UI
{
  public static partial class PhenomExtensions
  {
    #region RectTransform
    /// <summary>
    /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
    /// </summary>
    /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
    /// <param name="rectTransform">Rect transform.</param>
    /// <param name="camera">Camera.</param>
    private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
      Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
      Vector3[] objectCorners = new Vector3[4];
      rectTransform.GetWorldCorners(objectCorners);

      return objectCorners.Select(camera.WorldToScreenPoint).Count(tempScreenSpaceCorner => screenBounds.Contains(tempScreenSpaceCorner));
    }

    /// <summary>
    /// Determines if this RectTransform is fully visible from the specified camera.
    /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
    /// </summary>
    /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
    /// <param name="rectTransform">Rect transform.</param>
    /// <param name="camera">Camera.</param>
    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
      return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
    }

    /// <summary>
    /// Determines if this RectTransform is at least partially visible from the specified camera.
    /// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
    /// </summary>
    /// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
    /// <param name="rectTransform">Rect transform.</param>
    /// <param name="camera">Camera.</param>
    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
      return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corners are visible
    }

    public static Rect GetWorldSpaceRect(this RectTransform rectTransform)
    {
      Vector3[] corners = new Vector3[4];
      rectTransform.GetWorldCorners(corners);
      // Get the bottom left corner.
      Vector3 position = corners[0];

      Vector2 size = new Vector2(
        rectTransform.lossyScale.x * rectTransform.rect.size.x,
        rectTransform.lossyScale.y * rectTransform.rect.size.y);

      return new Rect(position, size);
    }

    /// <summary>
    /// Returns true if at least 1 corner of rect2 is within the bounds of rect1
    /// </summary>
    /// <param name="rt1"></param>
    /// <param name="rt2"></param>
    /// <returns></returns>
    public static bool Overlaps(this RectTransform rt1, RectTransform rt2)
    {
      Rect rect1 = GetWorldSpaceRect(rt1);
      Rect rect2 = GetWorldSpaceRect(rt2);

      return rect1.Overlaps(rect2);
    }

    /// <summary>
    /// Returns true if all 4 corners of rect2 are within the bounds of rect1
    /// </summary>
    /// <param name="rt1"></param>
    /// <param name="rt2"></param>
    /// <returns></returns>
    public static bool Contains(this RectTransform rt1, RectTransform rt2)
    {
      Rect rect1 = GetWorldSpaceRect(rt1);
      Vector3[] corners = new Vector3[4];
      rt2.GetWorldCorners(corners);

      return rect1.Contains(corners[0]) && rect1.Contains(corners[1]) && rect1.Contains(corners[2]) && rect1.Contains(corners[3]);
    }

    public static void StretchAndFitInParent(this RectTransform rectTransform)
    {
      rectTransform.anchorMin = Vector2.zero;
      rectTransform.anchorMax = Vector2.one;
      rectTransform.sizeDelta = Vector2.zero;
      rectTransform.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region Button

    public static void SetInteractable(this Button button, bool on)
    {
      button.interactable = on;
    }

    #endregion

    #region Toggle

    public static void SetInteractable(this Toggle toggle, bool on)
    {
      toggle.interactable = on;
    }

    public static void SetIsOn(this Toggle toggle, bool on)
    {
      toggle.isOn = on;
    }

    #endregion

    #region Canvas Group

    public static void SetInteractable(this CanvasGroup group, bool enabled)
    {
      group.interactable = enabled;
      group.blocksRaycasts = enabled;
    }

    public static void SetVisibility(this CanvasGroup group, bool enabled)
    {
      DOTween.Kill(group);

      if (enabled)
        group.gameObject.SetActive(true);

      group.alpha = enabled ? 1f : 0f;
      group.interactable = enabled;
      group.blocksRaycasts = enabled;
    }
    
    #endregion

    #region Text

    public static string ToCurrency(this int valueToConvert, bool removeZeroCents = false, bool hideDollarSign = false)
    {
      double tempDouble = valueToConvert * 0.01d;
      string tempString;
      if (hideDollarSign)
        tempString = $"{tempDouble:N}"; // Does not have Currency Symbol
      else
        tempString = $"{tempDouble:C}"; // Has Currency Symbol

      if (removeZeroCents && tempString.Substring(tempString.Length - 2, 2) == "00")
        tempString = tempString.Substring(0, tempString.Length - 3);

      return tempString;
    }

    #endregion

    #region Image

    public static void PrepareFillImage(this Image image, int fillOrigin)
    {
      image.fillAmount = 0;
      image.fillOrigin = fillOrigin;
    }

    public static void PrepareFillImage(this Image image, bool clockwise)
    {
      image.fillAmount = 0;
      image.fillClockwise = clockwise;
    }

    #endregion
  }
}