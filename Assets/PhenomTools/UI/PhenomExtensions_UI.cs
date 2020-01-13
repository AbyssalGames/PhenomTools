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
        public static Tweener Grow(this RectTransform rectTransform, Vector2 endValue, float duration = .5f)
        {
            DOTween.Kill(rectTransform);
            return DOTween.To(() => rectTransform.sizeDelta, newSize => rectTransform.sizeDelta = newSize, endValue, duration);
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

        public static Tweener Fade(this CanvasGroup group, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
        {
            return Fade(group, enabled ? 1f : 0f, duration, animateScale, disableOnFadeOut, destroyOnFadeOut, completeCallback);
        }

        public static Tweener Fade(this CanvasGroup group, float endValue, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
        {
            if (group.alpha == endValue)
                return null;

            DOTween.Kill(group);

            group.gameObject.SetActive(true);

            if (endValue == 0)
                group.SetInteractable(false);

            return group.DOFade(endValue, duration).OnComplete(() =>
            {
                if (endValue > 0)
                    group.SetInteractable(true);

                completeCallback?.Invoke();
            });
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