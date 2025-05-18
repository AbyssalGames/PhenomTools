using UnityEngine;
using MPUIKIT;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace PhenomTools.UI
{
  /// <summary>
  /// Converts the color values on a UI object into using the figma theme db.
  /// </summary>
  public class FigmaColorPreset : MonoBehaviour
  {
    [SerializeField]
    private Graphic graphic;
    [SerializeField, Required]
    private FigmaSwatch startSwatch;
    [ShowInInspector, ReadOnly] 
    private FigmaSwatch cachedSwatch;
    [SerializeField, Required] 
    private bool outlineColor;

    private void OnValidate()
    {
      if (!enabled)
        return;
      
      cachedSwatch = null;
      Redraw();
    }

    [ContextMenu("Redraw")]
    private void Redraw()
    {
      if (cachedSwatch == null)
        cachedSwatch = startSwatch;
      
      UpdateGraphic(cachedSwatch);
    }

    public void UpdateGraphic(FigmaSwatch swatch) => UpdateGraphicInternal(swatch);

    private void UpdateGraphicInternal(FigmaSwatch swatch)
    {
      if (graphic == null)
        return;
      
      if (swatch == null)
      {
        if (Application.isPlaying) Debug.LogWarning("FigmaLabelElement.UpdateLabel: swatch is null\n\r");
        return;
      }

      if (swatch.Type != FigmaSwatchType.Normal) 
        return;
      
      if (outlineColor)
      {
        if (graphic is MPImage mpImage)
          mpImage.OutlineColor = swatch.Color;
        else if (graphic is MPImageBasic mpImageBasic)
          mpImageBasic.OutlineColor = swatch.Color;
        else
          outlineColor = false;
      }

      if(!outlineColor)
        graphic.color = swatch.Color;
    }
    
    private void Reset()
    {
      graphic = GetComponent<Graphic>();
    }
  }
}
