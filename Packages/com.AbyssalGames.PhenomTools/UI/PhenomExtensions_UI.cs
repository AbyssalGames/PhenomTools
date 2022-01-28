using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public static partial class PhenomExtensions
    {
        #region RectTransform
        public static Tweener DODeltaScale(this RectTransform rectTransform, Vector2 endValue, float duration = .5f/*, bool ignoreTimescale = false*/)
        {
            //DOTween.Kill(rectTransform);
            return DOTween.To(() => rectTransform.sizeDelta, newSize => rectTransform.sizeDelta = newSize, endValue, duration);//.SetUpdate(UpdateType.Normal, ignoreTimescale);
        }

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

            int visibleCorners = 0;
            Vector3 tempScreenSpaceCorner; // Cached
            for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
            {
                tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
                if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
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

        // This one doesn't seem to work as intended
        //public static Rect GetWorldSpaceRect(this RectTransform rt)
        //{
        //    var r = rt.rect;
        //    r.center = rt.TransformPoint(r.center);
        //    r.size = rt.TransformVector(r.size);
        //    return r;
        //}

        //public static bool Overlaps(this RectTransform rectTrans1, RectTransform rectTrans2)
        //{
        //    Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        //    Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        //    return rect1.Overlaps(rect2);
        //}

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

        public static bool Overlaps(this RectTransform rt1, RectTransform rt2)
        {
            Rect rect1 = GetWorldSpaceRect(rt1);
            Rect rect2 = GetWorldSpaceRect(rt2);

            return rect1.Overlaps(rect2);
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

        public static Tweener Fade(this CanvasGroup group, bool enabled, float duration = .5f, float animateScale = 1f, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null, Action updateCallback = null, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            return Fade(group, enabled ? 1f : 0f, duration, animateScale, disableOnFadeOut, destroyOnFadeOut, completeCallback, updateCallback, updateMode);
        }

        public static Tweener Fade(this CanvasGroup group, float endValue, float duration = .5f, float animateScale = 1f, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null, Action updateCallback = null, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            if (group.alpha == endValue)
                return null;

            DOTween.Kill(group);

            group.gameObject.SetActive(true);

            if (endValue == 0)
                group.SetInteractable(false);

            if (animateScale != 1f)
            {
                if(group.alpha > endValue)
                {
                    group.transform.DOScale(animateScale, duration);
                }
                else
                {
                    group.transform.localScale = Vector3.one * animateScale;
                    group.transform.DOScale(1f, duration);
                }
            }

            Tweener fadeTween = group.DOFade(endValue, duration);

            fadeTween.onComplete += () =>
            {
                if (endValue > 0)
                    group.SetInteractable(true);

                if (endValue == 0)
                {
                    if (disableOnFadeOut)
                        group.gameObject.SetActive(false);

                    if (destroyOnFadeOut)
                        UnityEngine.Object.Destroy(group.gameObject);
                }

                completeCallback?.Invoke();
            };

            fadeTween.onUpdate += () => updateCallback?.Invoke();

            if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                fadeTween.SetUpdate(UpdateType.Fixed);
            else if(updateMode == AnimatorUpdateMode.UnscaledTime)
                fadeTween.SetUpdate(UpdateType.Normal, true);
            else
                fadeTween.SetUpdate(UpdateType.Normal);

            return fadeTween;
        }

        //public static void Fade(this CanvasGroup canvasGroup, bool enabled, UnityEngine.Object customKey, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
        //{
        //    canvasGroup.gameObject.SetActive(true);

        //    if (!enabled)
        //    {
        //        canvasGroup.interactable = false;
        //        canvasGroup.blocksRaycasts = false;
        //    }

        //    if (customKey == null)
        //        coroutineHolder.StartCoroutine(FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
        //    else
        //        coroutineHolder.RegisterCoroutine(customKey, FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
        //}
        //public static IEnumerator FadeOverTime(this CanvasGroup canvasGroup, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
        //{
        //    yield return coroutineHolder.StartCoroutine(FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
        //}
        //private static IEnumerator FadeCanvasGroupOverTime(CanvasGroup canvasGroup, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
        //{
        //    if (enabled)
        //    {
        //        canvasGroup.interactable = true;
        //        canvasGroup.blocksRaycasts = true;
        //    }

        //    if (animateScale)
        //        canvasGroup.transform.localScale = Vector3.one * (enabled ? 1.05f : 1f);

        //    float time = 0f;
        //    while (time < 1f)
        //    {
        //        if (canvasGroup == null)
        //            yield break;

        //        time += Time.deltaTime / duration;
        //        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, enabled ? 1f : 0f, time);

        //        if (animateScale)
        //            canvasGroup.transform.localScale = Vector3.one * Mathf.Lerp(canvasGroup.transform.localScale.x, animateScale ? 1f : 1.05f, time);

        //        yield return null;
        //    }

        //    if (!enabled && disableOnFadeOut && canvasGroup != null && canvasGroup.gameObject != null)
        //        canvasGroup.gameObject.SetActive(false);

        //    if (!enabled && destroyOnFadeOut)
        //        UnityEngine.Object.Destroy(canvasGroup.gameObject);

        //    completeCallback?.Invoke();
        //}
        #endregion

        #region Text
        public static string ToCreditsDisplay(this int valueIn, bool isCurrencyDisplay = false)
        {
            return isCurrencyDisplay ? valueIn.ToCurrency() : valueIn.ToString();
        }

        public static string ToCurrency(this int valueToConvert, bool removeZeroCents = false, bool hideDollarSign = false)
        {
            double tempDouble = valueToConvert * 0.01d;
            string tempString;
            if (hideDollarSign)
                tempString = string.Format("{0:N}", tempDouble); // Does not have Currency Symbol
            else
                tempString = string.Format("{0:C}", tempDouble); // Has Currency Symbol

            if (removeZeroCents && tempString.Substring(tempString.Length - 2, 2) == "00")
                tempString = tempString.Substring(0, tempString.Length - 3);

            return tempString;
        }

        public static Tweener Roll(this TMP_Text text, int valueIn, int valueOut, float duration, bool? isCurrency = null)
        {
            int value = valueIn;
            return value.Roll(() => value, x => value = x, valueOut, duration).OnUpdate(() => text.SetText(isCurrency == true ? value.ToCurrency() : value.ToCreditsDisplay()));
        }

        //public static void Roll(this TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
        //{
        //    coroutineHolder.StartCoroutine(RollToValueOverTime(text, valueIn, valueOut, duration, isCurrency));
        //}
        //public static IEnumerator RollOverTime(this TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
        //{
        //    yield return coroutineHolder.StartCoroutine(RollToValueOverTime(text, valueIn, valueOut, duration, isCurrency));
        //}
        //private static IEnumerator RollToValueOverTime(TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
        //{
        //    float time = 0f;
        //    while (time < 1f)
        //    {
        //        time += Time.deltaTime / duration;
        //        int newValue = Mathf.FloorToInt(Mathf.Lerp(valueIn, valueOut, time));
        //        text.text = newValue.ToCreditsDisplay(isCurrency);
        //        yield return null;
        //    }
        //}
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