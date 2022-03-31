using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhenomTools;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace PhenomTools
{
    public class InputFieldExtended : TMP_InputField
    {
        public UnityEvent onBeginDrag;
        public UnityEvent onEndDrag;

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
