using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ReplyLineConnector : MonoBehaviour
    {
        //public RectTransform t1, t2;

        //private RectTransform rect;

        public void Initialize(Vector2 t1, Vector2 t2)
        {
            //rect = transform as RectTransform;

            (transform as RectTransform).sizeDelta = new Vector2(t2.x - t1.x, t1.y - t2.y) / transform.lossyScale;
        }

        //[ContextMenu("Test")]
        //public void Test()
        //{
        //    Initialize(t1.position, t2.position);
        //}
    }
}
