using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    public class RepliesSection : CommentsSection
    {
        [SerializeField]
        private CommentItem replyItemPrefab = null;
        [SerializeField]
        private Transform repliesParent = null;
        [SerializeField]
        private GameObject showRepliesButton = null;
        [SerializeField]
        private GameObject loadMoreButton = null;
        [SerializeField]
        private ReplyLineConnector replyLinePrefab = null;
        [SerializeField]
        private Transform replyLineParent = null;

        // private OrderedDictionary<Activity, CommentItem> commentItems = new OrderedDictionary<Activity, CommentItem>();
        private CommentItem commentItem => activityItem as CommentItem;

        public override void Initialize(ActivityItem activityItem)
        {
            base.Initialize(activityItem);

            if (activity.CommentsCount > 0)
                showRepliesButton.SetActive(true);
        }

        public override void Initialize(ActivityItem activityItem, PagingActivitiesPackage pagingActivitiesPackage)
        {
            base.Initialize(activityItem, pagingActivitiesPackage);

            if (activity.CommentsCount > 0)
                showRepliesButton.SetActive(true);
        }

        public void ShowReplies()
        {
            showRepliesButton.SetActive(false);
            Refresh();
        }

        protected override void GetNextPage()
        {
            base.GetNextPage();
            loadMoreButton.SetActive(false);
        }

        protected override void OnCommentsReceived(List<Activity> comments)
        {
            foreach (Activity comment in comments)
                CreateCommentItem(comment);

            if (commentItems.Count < activity.CommentsCount)
                loadMoreButton.SetActive(true);
        }

        public override CommentItem CreateCommentItem(Activity comment)
        {
            CommentItem replyItem = Instantiate(replyItemPrefab, repliesParent);
            replyItem.Initialize(comment, commentItem, commentItem.rootPostItem, activityItem.scrollRect);
            replyItem.BeginVisibilityChecks(activityItem.scrollRect.transform as RectTransform, activityItem.scrollRect.onMove);
            PhenomUtils.DelayActionByFrames(1, replyItem.CheckVisibility);
            commentItems.Add(comment.Id, replyItem);

            Instantiate(replyLinePrefab, replyLineParent).Initialize(commentItem.profileImage.transform.position, replyItem.profileImage.transform.position);

            return replyItem;
        }
    }
}
