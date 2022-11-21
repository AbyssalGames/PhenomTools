using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBoxVR.GetSocial
{
    public class GalleryPreviewItem : MonoBehaviour
    {
        [SerializeField]
        private RawImage rawImage = null;
        [SerializeField]
        private AspectRatioFitter fitter = null;
        [SerializeField]
        private TextMeshProUGUI additionalMediaCountLabel = null;

        public void SetMedia(MediaData media)
        {
            media.GetTexture(texture =>
            {
                if (rawImage == null)
                    return;

                rawImage.texture = texture;
                float ratio = (float)texture.width / texture.height;
                fitter.aspectRatio = ratio;
            });
        }

        public void SetAdditionalText(int amount)
        {
            if(amount == 0)
            {
                rawImage.color = Color.white;
                additionalMediaCountLabel.SetText("");
            }
            else
            {
                rawImage.color = Color.gray;
                additionalMediaCountLabel.gameObject.SetActive(true);
                additionalMediaCountLabel.SetText("+" + amount.ToString());
            }
        }
    }
}
