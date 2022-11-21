using System;
using GetSocialSdk.Core;
using PhenomTools;
using TMPro;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class CommentItem : ActivityItem
    {
        [Header("CommentItem Fields")]
        public ProfileImage profileImage = null;

        [SerializeField]
        private TextMeshProUGUI usernameText = null;
        // [SerializeField]
        // private TextMeshProUGUI messageText = null;

        [NonSerialized]
        public PostItem rootPostItem;
        [NonSerialized]
        public CommentItem parentCommentItem;

        public void Initialize(Activity activity, CommentItem parentCommentItem, PostItem rootPostItem, ScrollRectExtended scrollRect)
        {
            base.Initialize(activity, scrollRect);
            this.parentCommentItem = parentCommentItem;
            this.rootPostItem = rootPostItem;
        }

        protected override void FirstBecameVisible()
        {
            base.FirstBecameVisible();

            profileImage.InitGetSocial(activity.Author);
            usernameText.SetText(activity.Author.DisplayName);
            // messageText.SetText(activity.Text);
        }

        public override void Edit()
        {
            if (rootPostItem is FeedItem feedItem)
            {
                Debug.Log("Edit Comment from Feed", gameObject);
                feedItem.OpenThread().commentsSection.EditComment(activity);
            }
            else if (rootPostItem is ThreadItem threadItem)
            {
                Debug.Log("Edit Comment from Thread", gameObject);
                threadItem.threadView.createCommentView.Initialize(this);
                threadItem.threadView.createCommentView.SelectInputField();
            }
        }
    }
}
