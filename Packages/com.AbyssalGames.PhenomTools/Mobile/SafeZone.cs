using UnityEngine;

namespace PhenomTools
{
    public class SafeZone : MonoBehaviour
    {
        [SerializeField]
        private RectTransform panel;
        [SerializeField]
        private bool top = true;
        [SerializeField]
        private bool bot = true;

        private void Start()
        {
            if (panel == null)
                panel = GetComponent<RectTransform>();

            Refresh();
        }

        public void Refresh()
        {
            ApplySafeArea(Screen.safeArea);
        }

        private void ApplySafeArea(Rect r)
        {
            if (bot)
            {
                Vector2 anchorMin = r.position;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                panel.anchorMin = anchorMin;
            }
            
            if (top)
            {
                Vector2 anchorMax = r.position + r.size;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                panel.anchorMax = anchorMax;
            }
        }

        private void Reset()
        {
            panel = GetComponent<RectTransform>();
        }
    }
}