using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityEngine.UI
{
  public class ContentSizeFitterExtended : ContentSizeFitter
  {
    [SerializeField, Required] 
    private Vector2 maximumConstraints = new(float.MaxValue, float.MaxValue);
    
    [NonSerialized] 
    private RectTransform m_Rect;
    
    private RectTransform RectTransform
    {
      get
      {
        if (m_Rect == null)
          m_Rect = GetComponent<RectTransform>();
        return m_Rect;
      }
    }
    
    public override void SetLayoutHorizontal()
    {
      base.SetLayoutHorizontal();
      HandleSelfFittingAlongAxis(0, maximumConstraints.x);
    }
    
    public override void SetLayoutVertical()
    {
      base.SetLayoutVertical();
      HandleSelfFittingAlongAxis(1, maximumConstraints.y);
    }
    
    private void HandleSelfFittingAlongAxis(int axis, float max)
    {
      FitMode fitting = axis == 0 ? horizontalFit : verticalFit;
      
      if (fitting == FitMode.MinSize)
        RectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Min(LayoutUtility.GetMinSize(RectTransform, axis), max));
      else
        RectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Min(LayoutUtility.GetPreferredSize(RectTransform, axis), max));
    }
  }
}
