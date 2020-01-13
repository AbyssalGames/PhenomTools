using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ScrollRectExtended : ScrollRect, IBeginDragHandler, IEndDragHandler
    {
        public event Action onBeginDragHandler;
        public event Action onEndDragHandler;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            onBeginDragHandler?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            onEndDragHandler?.Invoke();
        }
    }
}