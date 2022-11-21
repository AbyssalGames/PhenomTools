using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.Android;
using System.Linq;

namespace BlackBoxVR.GetSocial
{
    public class MediaEditingView : MonoBehaviour
    {
        [SerializeField]
        private MediaEditingItem itemPrefab = null;
        [SerializeField]
        private Transform itemParent = null;

        private CreateActivityView createActivityView;
        private List<MediaData> mediaList = new List<MediaData>();
        private List<MediaEditingItem> items = new List<MediaEditingItem>();

        public void Initialize(CreateActivityView createActivityView)
        {
            this.createActivityView = createActivityView;
        }

        public void Show(List<MediaData> mediaList)
        {
            gameObject.SetActive(true);
            this.mediaList = mediaList;

            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                    Destroy(items[i].gameObject);

                items.Clear();
            }

            foreach (MediaData media in mediaList)
            {
                MediaEditingItem item = Instantiate(itemPrefab, itemParent);
                item.Initialize(this, media);
                items.Add(item);
            }
        }

        public void ShowGallery()
        {
            GetSocialUtils.AskForPermissions(new string[] { Permission.ExternalStorageRead }, () =>
            {
                Media.Gallery.Pick((error, results) =>
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError(error);
                        return;
                    }

                    if (results.Length > 0)
                    {
                        mediaList.AddRange(results.ToMediaData());
                        mediaList = mediaList.Distinct().ToList();
                    }
                });
            });
        }

        public void Remove(MediaData media)
        {
            mediaList.Remove(media);
        }

        public void OnDone()
        {
            createActivityView.UpdateMedia(mediaList);
            gameObject.SetActive(false);
        }
    }
}
