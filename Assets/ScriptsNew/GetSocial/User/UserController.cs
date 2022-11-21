using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class UserController : PhenomTools.Singleton<UserController>
    {
        [SerializeField]
        private Canvas canvas = null;
        [SerializeField]
        private ProfileView profileViewPrefab = null;

        public static ProfileView OpenProfileView(UserId userId)
        {
            Instance.canvas.worldCamera = Camera.main;
            ProfileView newProfileView = Instantiate(Instance.profileViewPrefab, Instance.transform);
            newProfileView.Initialize(userId);
            return newProfileView;
        }

        //public static GalleryView OpenGalleryView()
        //{
        //    Instance.canvas.worldCamera = Camera.main;
        //    GalleryView newGalleryView = Instantiate(Instance.galleryViewPrefab, Instance.transform);
        //    newGalleryView.Initialize(userId);
        //    return newGalleryView;
        //}
    }
}
