﻿using Cysharp.Threading.Tasks;
using MPUIKIT;
using Sirenix.OdinInspector;
using PhenomTools.UI;
using PhenomTools.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools.UI
{
  [AddComponentMenu("UI/Effects/Gradient")]
  public class UIGradient : BaseMeshEffect
  {
    [SerializeField, ValidateInput("@gradientSwatch == null || gradientSwatch.Type == FigmaSwatchType.Gradient", "Swatch is not Gradient type")]
    private FigmaSwatch gradientSwatch;
    [SerializeField, Required, ShowIf("@gradientSwatch == null")] 
    private Gradient gradient = new();
    
    [Range(-180f, 180f)]
    [SerializeField]
    private float angle;
    [SerializeField]
    private bool ignoreRatio = true;
    [SerializeField, ShowIf(nameof(ShowInvertOption))]
    private bool invert;

    public override void ModifyMesh(VertexHelper vh)
    {
      if (!enabled) 
        return;
        
      if (graphic is MPImage { GradientEffect: { Enabled: true } } mpImage)
      {
        UniTask.DelayFrame(1).ContinueWith(() =>
        {
          GradientEffect effect = mpImage.GradientEffect;
          
          if (effect.GradientType == GradientType.Corner)
          {
            Gradient grad = gradientSwatch == null ? gradient : gradientSwatch.Gradient;
          
            effect.CornerGradientColors[0] = grad.Evaluate(invert ? 0f.RemapInverseNormalized(0, 1) : 0f);
            effect.CornerGradientColors[1] = grad.Evaluate(invert ? .33f.RemapInverseNormalized(0, 1) : .33f);
            effect.CornerGradientColors[2] = grad.Evaluate(invert ? .66f.RemapInverseNormalized(0, 1) : .66f);
            effect.CornerGradientColors[3] = grad.Evaluate(invert ? 1f.RemapInverseNormalized(0, 1) : 1f);
          }
          else
          {
            effect.Gradient = gradientSwatch == null ? gradient : gradientSwatch.Gradient;
            effect.Rotation = angle;
          }
          
          mpImage.GradientEffect = effect;
        }).Forget();
      }
      else
      {
        Rect rect = graphic.rectTransform.rect;
        Vector2 dir = RotationDir(angle);

        if (!ignoreRatio)
          dir = CompensateAspectRatio(rect, dir);

        Matrix2x3 localPositionMatrix = LocalPositionMatrix(rect, dir);

        UIVertex vertex = default;
        for (int i = 0; i < vh.currentVertCount; i++)
        {
          vh.PopulateUIVertex(ref vertex, i);
          Vector2 localPosition = localPositionMatrix * vertex.position;
          float evalPos = invert ? localPosition.y.RemapInverseNormalized(0, 1) : localPosition.y;
          vertex.color *= gradientSwatch == null ? gradient.Evaluate(evalPos) : gradientSwatch.Gradient.Evaluate(evalPos);

          vh.SetUIVertex(vertex, i);
        }
      }
    }

    public struct Matrix2x3
    {
      public float m00, m01, m02, m10, m11, m12;

      public Matrix2x3(float m00, float m01, float m02, float m10, float m11, float m12)
      {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
      }

      public static Vector2 operator *(Matrix2x3 m, Vector2 v)
      {
        float x = (m.m00 * v.x) - (m.m01 * v.y) + m.m02;
        float y = (m.m10 * v.x) + (m.m11 * v.y) + m.m12;
        return new Vector2(x, y);
      }
    }

    public static Matrix2x3 LocalPositionMatrix(Rect rect, Vector2 dir)
    {
      float cos = dir.x;
      float sin = dir.y;
      Vector2 rectMin = rect.min;
      Vector2 rectSize = rect.size;
      float c = 0.5f;
      float ax = rectMin.x / rectSize.x + c;
      float ay = rectMin.y / rectSize.y + c;
      float m00 = cos / rectSize.x;
      float m01 = sin / rectSize.y;
      float m02 = -(ax * cos - ay * sin - c);
      float m10 = sin / rectSize.x;
      float m11 = cos / rectSize.y;
      float m12 = -(ax * sin + ay * cos - c);
      return new Matrix2x3(m00, m01, m02, m10, m11, m12);
    }

    static Vector2[] ms_verticesPositions = new Vector2[] { Vector2.up, Vector2.one, Vector2.right, Vector2.zero };

    public static Vector2[] VerticePositions
    {
      get { return ms_verticesPositions; }
    }

    public static Vector2 RotationDir(float angle)
    {
      float angleRad = angle * Mathf.Deg2Rad;
      float cos = Mathf.Cos(angleRad);
      float sin = Mathf.Sin(angleRad);
      return new Vector2(cos, sin);
    }

    public static Vector2 CompensateAspectRatio(Rect rect, Vector2 dir)
    {
      float ratio = rect.height / rect.width;
      dir.x *= ratio;
      return dir.normalized;
    }

    public static float InverseLerp(float a, float b, float v)
    {
      return a != b ? (v - a) / (b - a) : 0f;
    }

    public static Color Bilerp(Color a1, Color a2, Color b1, Color b2, Vector2 t)
    {
      Color a = Color.LerpUnclamped(a1, a2, t.x);
      Color b = Color.LerpUnclamped(b1, b2, t.x);
      return Color.LerpUnclamped(a, b, t.y);
    }

    public static void Lerp(UIVertex a, UIVertex b, float t, ref UIVertex c)
    {
      c.position = Vector3.LerpUnclamped(a.position, b.position, t);
      c.normal = Vector3.LerpUnclamped(a.normal, b.normal, t);
      c.color = Color32.LerpUnclamped(a.color, b.color, t);
      c.tangent = Vector3.LerpUnclamped(a.tangent, b.tangent, t);
      c.uv0 = Vector3.LerpUnclamped(a.uv0, b.uv0, t);
      c.uv1 = Vector3.LerpUnclamped(a.uv1, b.uv1, t);
      // c.uv2 = Vector3.LerpUnclamped(a.uv2, b.uv2, t);
      // c.uv3 = Vector3.LerpUnclamped(a.uv3, b.uv3, t);		
    }

    private bool ShowInvertOption => graphic != null && graphic is not MPImage or MPImage { GradientEffect: { Enabled: false } } or MPImage { GradientEffect: { GradientType: GradientType.Corner } };
  }
}