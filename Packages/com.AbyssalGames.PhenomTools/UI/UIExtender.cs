using UnityEngine;
using UnityEngine.UI;

namespace PhenomTools
{
    public class UIExtender : MonoBehaviour
    {
        [Space]
        public Selectable[] allSelectables;
        public Button[] allButtons;
        public Toggle[] allToggles;
        //public Slider[] allSliders;
        public ScrollRect[] allScrollRects;

        public void GatherSelectables()
        {
            allSelectables = GetComponents<Selectable>();
            allButtons = GetComponents<Button>();
            allToggles = GetComponents<Toggle>();
            //allSliders = GetComponents<Slider>();
            allScrollRects = GetComponents<ScrollRect>();

            //allSelectables = GetComponentsInChildren<Selectable>(true);
            //allButtons = GetComponentsInChildren<Button>(true);
            //allToggles = GetComponentsInChildren<Toggle>(true);
            //allSliders = GetComponentsInChildren<Slider>(true);
        }

        public void ExtendButtons()
        {
            if (allButtons == null || allButtons.Length == 0)
                return;

            foreach (Button b in allButtons)
            {
                if (b.GetType() == typeof(ButtonExtended))
                    continue;

                GameObject go = b.gameObject;
                Button bCache = Instantiate(b);
                Graphic targetGraphic = b.targetGraphic;
                Button.ButtonClickedEvent onClick = b.onClick;

                DestroyImmediate(b);
                go.AddComponent<ButtonExtended>().SetParameters(bCache, targetGraphic, onClick);

                DestroyImmediate(bCache.gameObject);
            }
        }

        public void ExtendToggles()
        {
            if (allToggles == null || allToggles.Length == 0)
                return;

            foreach (Toggle t in allToggles)
            {
                if (t.GetType() == typeof(ToggleExtended))
                    continue;

                GameObject go = t.gameObject;
                Toggle tCache = Instantiate(t);
                Graphic targetGraphic = t.targetGraphic;
                Graphic graphic = t.graphic;
                Toggle.ToggleEvent onValueChanged = t.onValueChanged;

                DestroyImmediate(t);
                go.AddComponent<ToggleExtended>().SetParameters(tCache, targetGraphic, graphic, onValueChanged);

                DestroyImmediate(tCache.gameObject);
            }
        }

        //public void ExtendScrollRects()
        //{
        //    if (allScrollRects == null || allScrollRects.Length == 0)
        //        return;

        //    foreach (ScrollRect sr in allScrollRects)
        //    {
        //        if (sr.GetType() == typeof(ScrollRect))
        //            continue;

        //        GameObject go = sr.gameObject;
        //        ScrollRect srCache = Instantiate(sr);

        //        DestroyImmediate(sr);
        //        go.AddComponent<ToggleExtended>().SetParameters(srCache, targetGraphic, graphic, onValueChanged);

        //        DestroyImmediate(srCache.gameObject);
        //    }
        //}
    }
}


































