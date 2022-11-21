using UnityEngine;
using PhenomTools;
using TMPro;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    public class PostInteractionSection : InteractionSection
    {
        [SerializeField]
        protected TextMeshProUGUI commentsCountText = null;
        [SerializeField]
        protected GameObject topParent = null;

        private PostItem postItem => activityItem as PostItem;

        public override void Initialize(ActivityItem activityItem)
        {
            base.Initialize(activityItem);
            topParent.SetActive(activity.CommentsCount > 0 || activity.GetAllReactionsCountSafe() > 0);

            if (activityItem.activity.CommentsCount > 0)
                commentsCountText.SetText(activity.CommentsCount.ToBigNumberString() + (activity.CommentsCount == 1 ? " comment" : " comments"));
        }

        public override void Refresh()
        {
            base.Refresh();
            topParent.SetActive(activity.CommentsCount > 0 || activity.GetAllReactionsCountSafe() > 0);

            if (activity.CommentsCount > 0)
                commentsCountText.SetText(activity.CommentsCount.ToBigNumberString() + (activity.CommentsCount == 1 ? " comment" : " comments"));
        }

        public virtual void ViewComments()
        {
            if (activityItem != null && activityItem is FeedItem feedItem)
                feedItem.OpenThread();

            // if thread item and the comments section is long enough to scroll down to
        }

        public virtual void OpenShareView()
        {

        }
    }
}
