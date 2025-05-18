using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MPUIKIT
{
  //TODO Fix ordering of vert adding, will take a while. This currently only really works for squares
  public class MPImageBasic_3x3 : MPImageBasic
  {
    protected override void OnPopulateMesh(VertexHelper vh)
    {
      Rect r = GetPixelAdjustedRect();
      Vector4 v = new(r.x, r.y, r.x + r.width, r.y + r.height);

      Color32 color32 = color;
      vh.Clear();
      
      vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0f, 0f));                                              //0
      vh.AddVert(new Vector3(v.x, v.y + r.height * .33f), color32, new Vector2(0f, .33f));                          //1
      vh.AddVert(new Vector3(v.x + r.width * .33f, v.y + r.height * .33f), color32, new Vector2(.33f, .33f));       //2
      vh.AddVert(new Vector3(v.x + r.width * .33f, v.y), color32, new Vector2(.33f, 0f));                           //3
      vh.AddVert(new Vector3(v.x + r.height * .66f, v.y), color32, new Vector2(.66f, 0f));                          //4
      vh.AddVert(new Vector3(v.x + r.height * .66f, v.y + r.height * .33f), color32, new Vector2(.66f, .33f));      //5
      vh.AddVert(new Vector3(v.x + r.height * .66f, v.y + r.height * .66f), color32, new Vector2(.66f, .66f));      //6
      vh.AddVert(new Vector3(v.x + r.width * .33f, v.y + r.height * .66f), color32, new Vector2(.33f, .66f));       //7
      vh.AddVert(new Vector3(v.x, v.y + r.height * .66f), color32, new Vector2(0f, .66f));                          //8
      
      vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1f, 0f));                                              //9
      vh.AddVert(new Vector3(v.z, v.y + r.height * .33f), color32, new Vector2(1f, .33f));                          //10
      vh.AddVert(new Vector3(v.z, v.y + r.height * .66f), color32, new Vector2(1f, .66f));                          //11
      vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1f, 1f));                                              //12
      vh.AddVert(new Vector3(v.x + r.width * .33f, v.w), color32, new Vector2(.33f, 1f));                           //13
      vh.AddVert(new Vector3(v.x + r.width * .66f, v.w), color32, new Vector2(.66f, 1f));                           //14
      vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0f, 1f));                                              //15
      
      //0,0
      vh.AddTriangle(0, 1, 2);
      vh.AddTriangle(2, 3, 0);
      
      //1,0
      vh.AddTriangle(3, 2, 5);
      vh.AddTriangle(5, 4, 3);
      
      //0,1
      vh.AddTriangle(1, 8, 7);
      vh.AddTriangle(7, 2, 1);
      
      //1,1
      vh.AddTriangle(2, 7, 6);
      vh.AddTriangle(6, 5, 2);
      
      //0,2
      vh.AddTriangle(8, 15, 13);
      vh.AddTriangle(13, 7, 8);
      
      //1,2
      vh.AddTriangle(7, 13, 14);
      vh.AddTriangle(14, 6, 7);
      
      //2,2
      vh.AddTriangle(6, 14, 12);
      vh.AddTriangle(12, 11, 6);
      
      //2,1
      vh.AddTriangle(5, 6, 11);
      vh.AddTriangle(11, 10, 5);
      
      //2,0
      vh.AddTriangle(4, 5, 10);
      vh.AddTriangle(10, 9, 4);

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
