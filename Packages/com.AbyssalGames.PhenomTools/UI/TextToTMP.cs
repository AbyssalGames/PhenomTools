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
            string currentText = text.text;
            int currentSize = text.fontSize;
            TextAnchor oldAnchor = text.alignment;
            Vector2 currentRect = rectTransform.sizeDelta;

            DestroyImmediate(text);

            TextMeshProUGUI tmpText = gameObject.AddComponent<TextMeshProUGUI>();
            rectTransform.sizeDelta = new Vector2(currentRect.x, currentRect.y);

            tmpText.text = currentText;
            tmpText.alignment = GetNewAlignmentFromOldAnchor(oldAnchor);
            tmpText.fontSize = currentSize;
            tmpText.font = fontAsset;

            Outline outline = GetComponent<Outline>();
            if (outline != null)
                DestroyImmediate(outline);

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
    }
}
