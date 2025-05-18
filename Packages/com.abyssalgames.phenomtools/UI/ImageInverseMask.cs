using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace PhenomTools.UI
{
  public class ImageInverseMask : Image
  {
    private static readonly int stencilComp = Shader.PropertyToID("_StencilComp");

    [SerializeField]
    private Transform transformToCenter;

    public override Material materialForRendering
    {
      get
      {
        Material mat = new(base.materialForRendering);
        mat.SetInt(stencilComp, (int)CompareFunction.NotEqual);
        return mat;
      }
    }

    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
      return !base.Raycast(sp, eventCamera);
    }

    private void Update()
    {
      transform.position = transformToCenter != null ? transformToCenter.position : Vector3.zero;
    }
  }
}