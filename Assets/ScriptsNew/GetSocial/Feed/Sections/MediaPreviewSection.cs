using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    public class MediaPreviewSection : MonoBehaviour
    {
        [SerializeField]
        private RawImage singleImage = null;
        [SerializeField]
        private LayoutElement singleImageLayout = null;
        [SerializeField]
        private GameObject galleryParent = null;
        [SerializeField]
        private GalleryPreviewItem[] galleryItems = null;

        public void Initialize(MediaResult[] mediaResults)
        {
            UpdateMediaPreviews(mediaResults.ToMediaData());
        }
        public void Initialize(List<MediaAttachment> attachments)
        {
            UpdateMediaPreviews(attachments.ToMediaData());
        }

        public void UpdateMediaPreviews(List<MediaData> mediaList)
        {
            if (mediaList.Count == 0)
            {
                singleImage.gameObject.SetActive(false);
                galleryParent.SetActive(false);
            }
            else if (mediaList.Count == 1)
            {
                singleImage.gameObject.SetActive(true);
                galleryParent.SetActive(false);

                mediaList[0].GetTexture(texture =>
                {
                    singleImage.texture = texture;
                    float ratio = (float)texture.width / texture.height;
                    singleImageLayout.preferredHeight = singleImageLayout.preferredWidth / ratio;
                });
            }
            else if (mediaList.Count > 1)
            {
                singleImage.gameObject.SetActive(false);
                galleryParent.SetActive(true);

                for (int i = 0; i < galleryItems.Length; i++)
                {
                    if (i < mediaList.Count)
                    {
                        galleryItems[i].gameObject.SetActive(true);
                        galleryItems[i].SetMedia(mediaList[i]);
                    }
                    else
                    {
                        galleryItems[i].gameObject.SetActive(false);
                    }
                }

                galleryItems[2].SetAdditionalText(mediaList.Count > 3 ? mediaList.Count - 3 : 0);
            }
        }
    }
}
