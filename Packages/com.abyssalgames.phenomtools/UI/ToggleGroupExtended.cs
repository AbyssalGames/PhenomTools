using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhenomTools.UI
{
  public class ToggleGroupExtended : UIBehaviour
  {
    [field: SerializeField]
    public bool AllowSwitchOff { get; set; }

    [ShowInInspector, ReadOnly]
    private readonly List<ToggleBase> toggles = new();

    protected override void Start()
    {
      EnsureValidState();
      base.Start();
    }

    protected override void OnEnable()
    {
      EnsureValidState();
      base.OnEnable();
    }

    private void ValidateToggleIsInGroup(ToggleBase toggle)
    {
      if (toggle == null || !toggles.Contains(toggle))
        throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] { toggle, this }));
    }

    public void NotifyToggleOn(ToggleBase toggle, bool sendCallback = true)
    {
      ValidateToggleIsInGroup(toggle);

      foreach (ToggleBase t in toggles.Where(t => t != toggle))
      {
        if (sendCallback)
          t.IsOn = false;
        else
          t.SetIsOnWithoutNotify(false);
      }
    }

    public void UnregisterToggle(ToggleBase toggle)
    {
      if (toggles.Contains(toggle))
        toggles.Remove(toggle);
    }

    public void RegisterToggle(ToggleBase toggle)
    {
      if (!toggles.Contains(toggle))
        toggles.Add(toggle);
    }

    public void EnsureValidState()
    {
      if (!AllowSwitchOff && !AnyTogglesOn() && toggles.Count != 0)
      {
        toggles[0].IsOn = true;
        NotifyToggleOn(toggles[0]);
      }

      IEnumerable<ToggleBase> activeToggles = ActiveToggles();

      if (activeToggles.Count() > 1)
      {
        ToggleBase firstActive = GetFirstActiveToggle();

        foreach (ToggleBase toggle in activeToggles)
        {
          if (toggle == firstActive)
            continue;

          toggle.IsOn = false;
        }
      }
    }

    public bool AnyTogglesOn()
    {
      return toggles.Find(x => x.IsOn) != null;
    }

    public IEnumerable<ToggleBase> ActiveToggles()
    {
      return toggles.Where(x => x.IsOn);
    }
    
    public ToggleBase GetFirstActiveToggle()
    {
      IEnumerable<ToggleBase> activeToggles = ActiveToggles();
      return activeToggles.Any() ? activeToggles.First() : null;
    }
    
    public void SetAllTogglesOff(bool sendCallback = true)
    {
      bool oldAllowSwitchOff = AllowSwitchOff;
      AllowSwitchOff = true;

      if (sendCallback)
      {
        foreach (ToggleBase t in toggles)
          t.IsOn = false;
      }
      else
      {
        foreach (ToggleBase t in toggles)
          t.SetIsOnWithoutNotify(false);
      }

      AllowSwitchOff = oldAllowSwitchOff;
    }
  }
}
