using UnityEngine;

namespace PhenomTools
{
    public class SafeZone : MonoBehaviour
    {
        public static float width;
        public static float height;

        [SerializeField]
        private RectTransform panel;

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
            width = r.width;
            height = r.height;

            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }

        private void Reset()
        {
            panel = GetComponent<RectTransform>();
        }
    }
}