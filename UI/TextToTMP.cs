using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools.UI
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
#if UNITY_2023_1_OR_NEWER
      tmpText.textWrappingMode = horizontalWrap ? TextWrappingModes.Normal : TextWrappingModes.NoWrap;
#else
      tmpText.enableWordWrapping = horizontalWrap;
#endif
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
      return oldAnchor switch
      {
        TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
        TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
        TextAnchor.UpperCenter => TextAlignmentOptions.Top,
        TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
        TextAnchor.MiddleRight => TextAlignmentOptions.Right,
        TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
        TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
        TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
        TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
        _ => TextAlignmentOptions.TopLeft
      };
    }

    private TextOverflowModes GetOverflowFromVerticalWrapMode(VerticalWrapMode wrapMode)
    {
      return wrapMode switch
      {
        VerticalWrapMode.Truncate => TextOverflowModes.Truncate,
        VerticalWrapMode.Overflow => TextOverflowModes.Overflow,
        _ => TextOverflowModes.Truncate
      };
    }
  }
}
