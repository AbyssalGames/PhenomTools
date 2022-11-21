using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

namespace BlackBoxVR.App
{
    public class ContextModalSection : MonoBehaviour
    {
        [SerializeField]
        private Image icon = null;
        [SerializeField]
        private SVGImage svgIcon = null;
        [SerializeField]
        private TextMeshProUGUI text = null;
        [SerializeField]
        private TextMeshProUGUI bodyText = null;

        private ContextModal modal;
        private ContextModal.Data data;

        public void Initialize(ContextModal modal, ContextModal.Data data)
        {
            this.modal = modal;
            this.data = data;

            if(bodyText != null)
            {
                text.SetText(data.title);
                bodyText.SetText(data.body);

                if(data.color.HasValue)
                    bodyText.color = data.color.Value;
            }
            else
            {
                text.SetText(data.title + data.body);
            }

            if (data.color.HasValue)
                text.color = data.color.Value;

            if(data.sprite == null)
            {
                icon.gameObject.SetActive(false);
            }
            else if (icon != null || svgIcon != null)
            {
                svgIcon.gameObject.SetActive(data.spriteIsSVG);
                icon.gameObject.SetActive(!data.spriteIsSVG);

                if (data.spriteIsSVG)
                {
                    svgIcon.sprite = data.sprite;

                    if (data.color.HasValue)
                        svgIcon.color = data.color.Value;
                }
                else
                {
                    icon.sprite = data.sprite;

                    if (data.color.HasValue)
                        icon.color = data.color.Value;
                }
            }
        }

        public void OnClick()
        {
            data?.callback?.Invoke();

            if (data.closeOnClick)
                modal.Close();
        }
    }
}
