using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class FriendItem : DynamicVisibilityRect
    {
        [SerializeField]
        private PlayerProfileTag profileTag = null;

        public void Initialize(User user)
        {

        }
    }
}
