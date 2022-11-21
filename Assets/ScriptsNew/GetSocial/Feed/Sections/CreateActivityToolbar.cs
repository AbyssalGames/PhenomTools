using UnityEngine;
using EasyMobile;
using UnityEngine.Android;

namespace BlackBoxVR.GetSocial
{
    public class CreateActivityToolbar : MonoBehaviour
    {
        protected CreateActivityView createActivityView;
         
        public void Initialize(CreateActivityView createActivityView)
        {
            this.createActivityView = createActivityView;
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
                        createActivityView.AddMedia(results.ToMediaData());
                });
            });
        }

        public void ShowCamera()
        {
            GetSocialUtils.AskForPermissions(new string[] { Permission.Camera, Permission.Microphone }, () =>
            {
                Media.Camera.TakePicture(EasyMobile.CameraType.Front, (error, result) =>
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError(error);
                        return;
                    }

                    if (result != null)
                        createActivityView.AddMedia(new MediaData(result));
                });
            });
        }

        public void ShowMentionSelectView()
        {
            createActivityView.ShowMentionSelectView();
        }
    }
}
