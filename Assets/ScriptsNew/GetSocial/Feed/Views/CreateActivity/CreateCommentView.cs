using System;
using GetSocialSdk.Core;
using PhenomTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBoxVR.GetSocial
{
    //TODO Make sure that media attachments work with comments
    public class CreateCommentView : CreateActivityView
    {
        [Header("Comment Fields")]
        [SerializeField]
        private ProfileImage profileImage = null;
        [SerializeField]
        private GameObject replyLabelParent = null;
        [SerializeField]
        private TextMeshProUGUI replyingToUsername = null;
        [SerializeField]
        private Image bg = null;
        [SerializeField]
        private Color replyBgColor;

        private ThreadView threadView;

        public void Initialize(ThreadView threadView)
        {
            this.threadView = threadView;
            activityItem = threadView.threadItem;
            target = PostActivityTarget.Comment(activity.Id);

            profileImage.InitGetSocial(GetSocialManager.localUser);

            Setup();
        }

        protected override void Setup()
        {
            keyboardExpander.minHeight = PhenomUtils.GetKeyboardHeightRelative((transform.parent as RectTransform).rect.height, false);
            toolbar.Initialize(this);
            mediaEditingView.Initialize(this);
        }

        public void ReplyToComment(Activity comment)
        {
            target = PostActivityTarget.Comment(comment.Id);
            bg.color = replyBgColor;
            replyingToUsername.SetText(comment.Author.DisplayName);
            replyLabelParent.SetActive(true);

            OpenEditor();
        }

        public void CancelReply()
        {
            target = PostActivityTarget.Comment(activity.Id);
            bg.color = Color.white;
            replyLabelParent.SetActive(false);
        }

        public override void ShowMentionSelectView()
        {

        }
        
        public override void Post()
        {
            inputField.interactable = false;
            postButton.interactable = false;
            base.Post();
        }

        protected override void OnPostSuccess(Activity activity)
        {
            inputField.SetText("", false);
            postButton.interactable = false;
            threadView.CreateCommentItems(activity);
            loadingView.SetActive(false);

            base.OnPostSuccess(activity);
        }

        protected override void OnPostError(GetSocialError error)
        {
            inputField.interactable = true;
            postButton.interactable = true;

            base.OnPostError(error);
        }

        public override void Close()
        {
            loadingView.SetActive(false);
            //TODO update comment item in the feed to reflect local draft 
            CloseEditor();
        }

        // protected override void GetContent(Action<ActivityContent> callback, ActivityContent content)
        // {
        //     if (mediaList.Count > 0)
        //     {
        //         content.AddProperty("Type", ActivityType.CustomMedia.ToString());
        //
        //         GetSocialUtils.GetAttachmentsList(mediaList, attachmentList =>
        //         {
        //             content.AddMediaAttachments(attachmentList);
        //             callback?.Invoke(content);
        //         });
        //     }
        //     else
        //     {
        //         content.AddProperty("Type", ActivityType.CustomText.ToString());
        //         callback?.Invoke(content);
        //     }
        // }
    }
}
