using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ScrollRectExtended : ScrollRect, IBeginDragHandler, IEndDragHandler
    {
        public UnityEvent onBeginDrag = new UnityEvent();
        public UnityEvent onEndDrag = new UnityEvent();

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            onBeginDrag?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            onEndDrag?.Invoke();
        }
    }
}