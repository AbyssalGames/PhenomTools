using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class ToggleGroupCustom : MonoBehaviour
{
    [SerializeField]
    private new bool enabled = true;
    [SerializeField]
    private bool allowSwitchOff;
    [SerializeField]
    private Toggle[] toggles;

    private int currentActiveIndex = -1;

    private void Awake()
    {
        if (toggles == null || toggles.Length == 0)
            return;

        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(delegate(bool on) { OnToggle(on, index); });

            if (currentActiveIndex == -1 && toggles[i].isOn) 
                // found the toggle that was manually set on before entering play mode
                currentActiveIndex = i;
            else if (currentActiveIndex > -1 && toggles[i].isOn) 
                // if a toggle has already been set as the current, but there is another one on, turn it off
                toggles[i].SetIsOnWithoutNotify(false);
            else if (!allowSwitchOff && currentActiveIndex == -1 && i == toggles.Length - 1) 
                // if none of the toggles are on and we don't allow switch off, set the first in the list on
                toggles[0].SetIsOn(true);
        }

        //Debug.Log("Awake, " + currentActiveIndex);

        if(!enabled)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.RemoveListener(delegate (bool on) { OnToggle(on, index); });
        }
    }

    public void OnToggle(bool on, int index)
    {
        if (on)
        {
            if (index == currentActiveIndex && !allowSwitchOff)
            {
                toggles[index].SetIsOnWithoutNotify(true);
                return;
            }

            //Debug.LogError("Toggled index: " + index + " " + on + ", Toggled index: " + currentActiveIndex + " " + !on);

            int lastIndex = currentActiveIndex;
            currentActiveIndex = index;

            if (lastIndex >= 0)
                toggles[lastIndex].SetIsOn(false);

            //for (int i = 0; i < toggles.Length; i++)
            //{
            //    if (i != index)
            //        toggles[i].SetIsOnWithoutNotify(false);
            //}

            
        }
        else if (index == currentActiveIndex && !allowSwitchOff)
        {
            toggles[index].SetIsOn(true);
        }
    }
}
 