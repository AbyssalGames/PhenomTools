using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class FilterTogglesSection : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup = null;

        private FeedView feedView;

        public void Initialize(FeedView feedView)
        {
            this.feedView = feedView;
        }

        public void FilterAll(bool on)
        {
            if (!on)
                return;

            ChangeFilter();
            feedView.Initialize(FeedType.Global, null, OnFilterApplied);
        }

        public void FilterFriends(bool on)
        {
            if (!on)
                return;

            ChangeFilter();
            feedView.Initialize(FeedType.Timeline, null, OnFilterApplied);
        }

        public void FilterMe(bool on)
        {
            if (!on)
                return;

            ChangeFilter();
            feedView.Initialize(FeedType.User, GetSocialManager.localUser.Id, OnFilterApplied);
        }

        private void ChangeFilter()
        {
            feedView.Clear();
            canvasGroup.SetInteractable(false);
        }

        private void OnFilterApplied()
        {
            canvasGroup.SetInteractable(true);
        }
    }
}
