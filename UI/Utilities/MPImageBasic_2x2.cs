using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MPUIKIT
{
  public class MPImageBasic_2x2 : MPImageBasic
  {
    protected override void OnPopulateMesh(VertexHelper vh)
    {
      Rect r = GetPixelAdjustedRect();
      Vector4 v = new(r.x, r.y, r.x + r.width, r.y + r.height);

      Color32 color32 = color;
      vh.Clear();

      vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0f, 0f));                                  //0
      vh.AddVert(new Vector3(v.x, v.y + r.height / 2f), color32, new Vector2(0f, .5f));                 //1
      vh.AddVert(new Vector3(v.x + r.width / 2f, v.y + r.height / 2f), color32, new Vector2(.5f, .5f)); //2
      vh.AddVert(new Vector3(v.x + r.width / 2f, v.y), color32, new Vector2(.5f, 0f));                  //3
      vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1f, 0f));                                  //4
      vh.AddVert(new Vector3(v.z, v.y + r.height / 2f), color32, new Vector2(1f, .5f));                 //5
      vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1f, 1f));                                  //6
      vh.AddVert(new Vector3(v.x + r.width / 2f, v.w), color32, new Vector2(.5f, 1f));                  //7
      vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0f, 1f));     
      
      vh.AddTriangle(0, 1, 2);
      vh.AddTriangle(2, 3, 0);

      vh.AddTriangle(3, 2, 5);
      vh.AddTriangle(5, 4, 3);
      
      vh.AddTriangle(1, 8, 7);
      vh.AddTriangle(7, 2, 1);
      
      vh.AddTriangle(2, 7, 6);
      vh.AddTriangle(6, 5, 2);

      MPVertexStream stream = CreateVertexStream();
      UIVertex uiVert = new();

      for (int i = 0; i < vh.currentVertCount; i++)
      {
        vh.PopulateUIVertex(ref uiVert, i);

        uiVert.uv1 = stream.Uv1;
        uiVert.uv2 = stream.Uv2;
        uiVert.uv3 = stream.Uv3;
        uiVert.normal = stream.Normal;
        uiVert.tangent = stream.Tangent;

        vh.SetUIVertex(uiVert, i);
      }
    }
  }
}
