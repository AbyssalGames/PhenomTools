using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class GlobalFeedView : MonoBehaviour
    {
        [SerializeField]
        private FilterTogglesSection filterSection = null;
        [SerializeField]
        private FeedCreatePostSection createPostSection = null;
        [SerializeField]
        private FeedView feedView = null;

        public void Initialize()
        {
            filterSection.Initialize(feedView);
            createPostSection.Initialize(feedView);
            feedView.Initialize();
        }
    }
}