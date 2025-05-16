using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhenomTools
{
    public class ScrollRectExtended : ScrollRect
    {
        public UnityEvent onMove = new UnityEvent();
        public UnityEvent onDrag = new UnityEvent();
        public UnityEvent onScroll = new UnityEvent();
        public UnityEvent onBeginDrag = new UnityEvent();
        public UnityEvent onEndDrag = new UnityEvent();

        public bool isDragging { get; private set; }

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
            isDragging = true;
            onBeginDrag?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            isDragging = false;
            onEndDrag?.Invoke();
        }
        
        public void CustomSetVerticalPosition(float value)
        {
            if(isDragging)
            {
                float anchoredYBeforeSet = content.anchoredPosition.y;
                content.anchoredPosition = Vector2.up * value;
                m_ContentStartPosition += new Vector2(0f, content.anchoredPosition.y - anchoredYBeforeSet);
            }
            else
                content.anchoredPosition = Vector2.up * value;
        }
        
        public void CustomSetVerticalNormalizedPosition(float value)
        {
            if(isDragging)
            {
                float anchoredYBeforeSet = content.anchoredPosition.y;
                SetNormalizedPosition(value, 1);
                m_ContentStartPosition += new Vector2(0f, content.anchoredPosition.y - anchoredYBeforeSet);
            }
            else
                SetNormalizedPosition(value, 1);
        }
    }
}