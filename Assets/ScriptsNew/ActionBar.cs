using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using AdvancedInputFieldPlugin;

namespace BlackBoxVR.GetSocial
{
    public class ActionBar : MonoBehaviour
    {
        private enum ActionType { Cut, Copy, Paste, SelectAll }
        
        [Serializable]
        private class ButtonsDict : SerializableDictionaryBase<ActionType, GameObject>{}
        [SerializeField]
        private ButtonsDict buttons = null;

        private AdvancedInputField inputField;
        
        public void Initialize(AdvancedInputField inputField)
        {
            this.inputField = inputField;
        }

        public void Cut()
        {
            //inputField.sele.SelectedText
        }
    }
}
