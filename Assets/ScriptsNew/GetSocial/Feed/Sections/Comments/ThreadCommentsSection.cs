using System.Collections.Generic;
using DG.Tweening;
using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ThreadCommentsSection : CommentsSection
    {
        [SerializeField]
        private CommentItem commentItemPrefab = null;
        [SerializeField]
        private RectTransform commentParent = null;
        [SerializeField]
        private CanvasGroup canvasGroup = null;

        private ThreadItem threadItem => activityItem as ThreadItem;

        public override void Initialize(ActivityItem activityItem)
        {
            Debug.Log("Init ThreadCommentsSection fresh");
            base.Initialize(activityItem);
            Communities.GetActivities(pagingQuery, OnCommentsReceived, e => Debug.LogError(e));
        }

        public override void Initialize(ActivityItem activityItem, PagingActivitiesPackage pagingActivitiesPackage)
        {
            Debug.Log("Init ThreadCommentsSection from pre-existing package");
            base.Initialize(activityItem, pagingActivitiesPackage);
            OnCommentsReceived(pagingActivitiesPackage.activities);
        }

        protected override void OnCommentsReceived(List<Activity> comments)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                CommentItem commentItem = CreateCommentItem(comments[i]);

                if (i == comments.Count / 2)
                    commentItem.onFirstBecameVisible += GetNextPage;
            }

            if (canvasGroup.alpha < 1f)
                PhenomUtils.DelayActionByTime(.5f, () => canvasGroup.DOFade(1f, .5f));
        }

        public override CommentItem CreateCommentItem(Activity comment)
        {
            CommentItem commentItem = Instantiate(commentItemPrefab, commentParent);
            commentItem.Initialize(comment, null, threadItem, activityItem.scrollRect);
            commentItem.BeginVisibilityChecks(activityItem.scrollRect.transform as RectTransform, activityItem.scrollRect.onMove);
            PhenomUtils.DelayActionByFrames(1, commentItem.CheckVisibility);
            commentItems.Add(comment.Id, commentItem);
            return commentItem;
        }

        public virtual void EditComment(Activity comment)
        {
            commentItems[comment.Id].Edit();
        }

        public override void Clear()
        {
            base.Clear();
            canvasGroup.alpha = 0f;
        }
    }
}
