using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExtentionTools : MonoBehaviour
{
    [Space]
    public Selectable[] allSelectables;
    public Button[] allButtons;
    public Toggle[] allToggles;
    public Slider[] allSliders;

    public void GatherSelectables()
    {
        allSelectables = GetComponentsInChildren<Selectable>(true);
        allButtons = GetComponentsInChildren<Button>(true);
        allToggles = GetComponentsInChildren<Toggle>(true);
        allSliders = GetComponentsInChildren<Slider>(true);
    }

    public void ExtendButtons()
    {
        if (allButtons != null && allButtons.Length > 0)
        {
            foreach (Button b in allButtons)
            {
                if (b.GetType() != typeof(ButtonExtended))
                {
                    GameObject go = b.gameObject;
                    Button bCache = Instantiate(b);
                    Graphic targetGraphic = b.targetGraphic;
                    Button.ButtonClickedEvent onClick = b.onClick;

                    DestroyImmediate(b);
                    go.AddComponent<ButtonExtended>().SetParameters(bCache, targetGraphic, onClick);

                    DestroyImmediate(bCache.gameObject);
                }
                //else
                //{
                //    Debug.LogError("Button already Extended", b.gameObject);
                //}
            }
        }
    }

    public void ExtendToggles()
    {
        if (allToggles != null && allToggles.Length > 0)
        {
            foreach (Toggle t in allToggles)
            {
                if (t.GetType() != typeof(ToggleExtended))
                {
                    GameObject go = t.gameObject;
                    Toggle tCache = Instantiate(t);
                    Graphic targetGraphic = t.targetGraphic;
                    Graphic graphic = t.graphic;
                    Toggle.ToggleEvent onValueChanged = t.onValueChanged;

                    DestroyImmediate(t);
                    go.AddComponent<ToggleExtended>().SetParameters(tCache, targetGraphic, graphic, onValueChanged);

                    DestroyImmediate(tCache.gameObject);
                }
                //else
                //{
                //    Debug.LogError("Toggle already Extended", t.gameObject);
                //}
            }
        }
    }
}



































