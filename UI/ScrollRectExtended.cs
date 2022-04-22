using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ScrollRectExtended : ScrollRect, IBeginDragHandler, IEndDragHandler
    {
        public UnityEvent onMove = new UnityEvent();
        public UnityEvent onDrag = new UnityEvent();
        public UnityEvent onScroll = new UnityEvent();
        public UnityEvent onBeginDrag = new UnityEvent();
        public UnityEvent onEndDrag = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();
            onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(Vector2 _)
        {
            onMove?.Invoke();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            onDrag?.Invoke();
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
            onScroll?.Invoke();
        }

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