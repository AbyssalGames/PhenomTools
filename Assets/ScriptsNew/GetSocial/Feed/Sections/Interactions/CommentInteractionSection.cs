using System.Collections.Generic;
using PhenomTools;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    public class CommentInteractionSection : InteractionSection
    {
        private CommentItem commentItem => activityItem as CommentItem;

        public override void Refresh()
        {
            int reactionCount = activity.GetAllReactionsCountSafe();
            reactionCountText.SetText(reactionCount.ToBigNumberString());
            reactionCountText.gameObject.SetActive(reactionCount > 0);

            for (int i = 0; i < 6; i++)
                reactionIcons[i].SetActive(activity.GetReactionsCountSafe(GetSocialManager.reactionData[i].key) > 0);
        }

        protected override void SetReactionsInfo(int totalReactionCount, List<UserReactions> reactionsList)
        {
            reactionCountText.SetText(totalReactionCount.ToBigNumberString());

            for (int i = 0; i < 6; i++)
                reactionIcons[i].SetActive(activity.GetReactionsCountSafe(GetSocialManager.reactionData[i].key) > 0);
        }

        public void Reply()
        {
            if(commentItem.rootPostItem is ThreadItem threadItem)
                threadItem.threadView.createCommentView.ReplyToComment(activity);
            else if (commentItem.rootPostItem is FeedItem feedItem)
                feedItem.OpenThread().createCommentView.ReplyToComment(activity);
        }
    }
}
