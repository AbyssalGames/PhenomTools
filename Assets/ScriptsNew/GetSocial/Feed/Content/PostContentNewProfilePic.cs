using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class PostContentNewProfilePic : ActivityContent_Base
    {
        [SerializeField]
        private ProfileImage profileImage;

        public override void FirstBecameVisible()
        {
            profileImage.InitGetSocial(activity.Author);
        }
    }
}
