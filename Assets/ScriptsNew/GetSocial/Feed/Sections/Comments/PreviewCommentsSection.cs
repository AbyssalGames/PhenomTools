using System.Collections.Generic;
using GetSocialSdk.Core;
using PhenomTools;
using TMPro;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class PreviewCommentsSection : CommentsSection
    {
        [SerializeField]
        private CommentItem commentItemPrefab = null;
        [SerializeField]
        private Transform commentParent = null;
        [SerializeField]
        private TextMeshProUGUI viewAllText = null;
        [SerializeField]
        private TextMeshProUGUI addCommentText = null;
        [SerializeField]
        private ProfileImage profileImage = null;
        [SerializeField]
        private TMP_InputField inputField = null;
        [SerializeField]
        private GameObject noSubWarning = null;

        private FeedItem feedItem => activityItem as FeedItem;

        public override void Initialize(ActivityItem activityItem)
        {
            base.Initialize(activityItem);
            profileImage.InitGetSocial(GetSocialManager.localUser);
            
            if (!GetSocialManager.CanPost)
            {
                inputField.placeholder.gameObject.SetActive(false);
                noSubWarning.SetActive(true);
            }
        }

        protected override void OnCommentsReceived(List<Activity> comments)
        {
            if (comments.Count > 3)
            {
                viewAllText.SetText("View all <b> " + activity.CommentsCount + " comments.");
                viewAllText.gameObject.SetActive(true);
            }

            for (int i = 0; i < Mathf.Min(3, comments.Count); i++)
                CreateCommentItem(comments[i]);
        }

        public override CommentItem CreateCommentItem(Activity comment)
        {
            CommentItem commentItem = Instantiate(commentItemPrefab, commentParent);
            commentItem.Initialize(comment, null, feedItem, activityItem.scrollRect);
            commentItem.BeginVisibilityChecks(activityItem.scrollRect.transform as RectTransform, activityItem.scrollRect.onMove);
            PhenomUtils.DelayActionByFrames(1, commentItem.CheckVisibility);
            commentItems.Add(comment.Id, commentItem);

            return commentItem;
        }

        public void ViewAll()
        {
            feedItem.OnCommentsButton();
        }

        public void AddComment()
        {
            feedItem.OpenThread().createCommentView.SelectInputField();
        }

        public void SetMyCommentText(string text)
        {
            addCommentText.SetText(text);
        }

        // public override void Clear()
        // {
        //     GameObject tempParent = new GameObject("Temp");
        //
        //     foreach (CommentItem item in commentItems.Values)
        //         item.transform.SetParent(tempParent.transform);
        //
        //     Destroy(tempParent);
        //
        //     commentItems.Clear();
        // }
    }
}
