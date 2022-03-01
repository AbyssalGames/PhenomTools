using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class TextToTMP : MonoBehaviour
    {
        public TMP_FontAsset fontAsset;

        public void ConvertTextToTMP()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Text text = GetComponent<Text>();
            Vector2 currentRect = rectTransform.sizeDelta;

            string currentText = text.text;
            TextAnchor oldAnchor = text.alignment;
            int currentSize = text.fontSize;
            Color oldColor = text.color;
            float lineSpacing = text.lineSpacing;
            bool richText = text.supportRichText;
            bool horizontalWrap = text.horizontalOverflow == HorizontalWrapMode.Wrap;
            VerticalWrapMode verticalOverflow = text.verticalOverflow;
            bool autoSize = text.resizeTextForBestFit;
            float minSize = text.resizeTextMinSize;
            float maxSize = text.resizeTextMaxSize;
            bool raycast = text.raycastTarget;
            bool maskable = text.maskable;

            DestroyImmediate(text);

            TextMeshProUGUI tmpText = gameObject.AddComponent<TextMeshProUGUI>();
            rectTransform.sizeDelta = new Vector2(currentRect.x, currentRect.y);
            tmpText.font = fontAsset;

            tmpText.text = currentText;
            tmpText.alignment = GetNewAlignmentFromOldAnchor(oldAnchor);
            tmpText.fontSize = currentSize;
            tmpText.color = oldColor;
            tmpText.lineSpacing = lineSpacing;
            tmpText.richText = richText;
            tmpText.enableWordWrapping = horizontalWrap;
            tmpText.overflowMode = GetOverflowFromVerticalWrapMode(verticalOverflow);
            tmpText.enableAutoSizing = autoSize;
            tmpText.fontSizeMin = minSize;
            tmpText.fontSizeMax = maxSize;
            tmpText.raycastTarget = raycast;
            tmpText.maskable = maskable;

            if (TryGetComponent(out Outline outline))
                DestroyImmediate(outline);
            if (TryGetComponent(out Shadow shadow))
                DestroyImmediate(shadow);

            DestroyImmediate(this);
        }

        private TextAlignmentOptions GetNewAlignmentFromOldAnchor(TextAnchor oldAnchor)
        {
            switch (oldAnchor)
            {
                default:
                case TextAnchor.UpperLeft:
                    return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperRight:
                    return TextAlignmentOptions.TopRight;
                case TextAnchor.UpperCenter:
                    return TextAlignmentOptions.Top;
                case TextAnchor.MiddleLeft:
                    return TextAlignmentOptions.Left;
                case TextAnchor.MiddleRight:
                    return TextAlignmentOptions.Right;
                case TextAnchor.MiddleCenter:
                    return TextAlignmentOptions.Center;
                case TextAnchor.LowerLeft:
                    return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight:
                    return TextAlignmentOptions.BottomRight;
                case TextAnchor.LowerCenter:
                    return TextAlignmentOptions.Bottom;
            }
        }

        private TextOverflowModes GetOverflowFromVerticalWrapMode(VerticalWrapMode wrapMode)
        {
            switch (wrapMode)
            {
                default:
                case VerticalWrapMode.Truncate:
                    return TextOverflowModes.Truncate;
                case VerticalWrapMode.Overflow:
                    return TextOverflowModes.Overflow;
            }
        }
    }
}
