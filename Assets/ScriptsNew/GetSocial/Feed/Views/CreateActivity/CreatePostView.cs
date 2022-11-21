using System;
using GetSocialSdk.Core;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using BlackBoxVR.MobileApp.ServerData;
using BlackBoxVR.App;

namespace BlackBoxVR.GetSocial
{
    public class CreatePostView : CreateActivityView
    {
        [Header("Post Fields")]
        [SerializeField]
        private PlayerProfileTag profileTag = null;
        [SerializeField]
        protected TextMeshProUGUI headerText = null;
        [SerializeField]
        private TextMeshProUGUI titleText = null;
        [SerializeField]
        private FeelingView feelingView = null;

        private int currentFeeling = -1;
        private FeedView targetFeed;

        public void Initialize(FeedView targetFeed)
        {
            target = PostActivityTarget.Timeline();
            this.targetFeed = targetFeed;
            Setup();
        }
        public void Initialize(string topicId, FeedView targetFeed)
        {
            target = PostActivityTarget.Topic(topicId);
            this.targetFeed = targetFeed;
            Setup();
        }

        /// <summary>
        /// Initialize in edit mode
        /// </summary>
        /// <param name="activityItem">The activity item being edited</param>
        public override void Initialize(ActivityItem activityItem)
        {
            base.Initialize(activityItem);
            profileTag.Init(activityItem.activity.Author.GetDataId());
            headerText.SetText("Edit your Post");

            if (activity.Properties.TryGetValue("Feeling", out string feelingRaw))
            {
                currentFeeling = int.Parse(feelingRaw);
                feelingView.Initialize(currentFeeling);
            }
        }

        protected override void Setup()
        {
            base.Setup();
            // profileTag.Init(ServerDataManager.SharedInstance.GameSparksUserId());
            feelingView.Initialize(-1);

            UpdateTitleText();
        }

        // public void SelectInputField()
        // {
        //     inputField.Select();
        // }

        // public override void Post()
        // {
        //     if (string.IsNullOrWhiteSpace(inputField.Text))
        //         return;
        //
        //     loadingView.SetActive(true);
        //     base.Post();
        // }

        protected override void OnPostSuccess(Activity activity)
        {
            if (targetFeed != null)
                targetFeed.CreateFeedItem(activity);

            Destroy(gameObject);
        }

        // public override void Done()
        // {
        //     if (string.IsNullOrWhiteSpace(inputField.Text))
        //         return;
        //
        //     loadingView.SetActive(true);
        //     base.Done();
        // }
        //
        // protected override void OnPostError(GetSocialError error)
        // {
        //     loadingView.SetActive(false);
        //     base.OnPostError(error);
        // }

        protected override void GetContent(Action<ActivityContent> callback, ActivityContent existingContent)
        {
            if (currentFeeling >= 0)
                existingContent.AddProperty("Feeling", currentFeeling.ToString());
            
            base.GetContent(callback, existingContent);
            
            // ActivityContent content = new ActivityContent { Text = inputField.Text };

            // if(mediaList.Count > 0)
            // {
            //     content.AddProperty("Type", ActivityType.CustomMedia.ToString());
            //
            //     GetSocialUtils.GetAttachmentsList(mediaList, attachmentList =>
            //     {
            //         content.AddMediaAttachments(attachmentList);
            //
            //         if (currentFeeling >= 0)
            //             content.AddProperty("Feeling", currentFeeling.ToString());
            //
            //         callback?.Invoke(content);
            //     });
            // }
            // else
            // {
            //     content.AddProperty("Type", ActivityType.CustomText.ToString());
            //
            //     if (currentFeeling >= 0)
            //         content.AddProperty("Feeling", currentFeeling.ToString());
            //
            //     callback?.Invoke(content);
            // }
        }

        public override void ShowMentionSelectView()
        {

        }

        public void ShowFeelingSelectView()
        {
            feelingView.gameObject.SetActive(true);
        }

        public void SetFeeling(int index)
        {
            currentFeeling = index;
            UpdateTitleText();
        }

        private void UpdateTitleText()
        {
            string newText = "";

            if (currentFeeling >= 0)
                newText += "Feeling... <size=12>" + GetSocialManager.feelingData[currentFeeling].emoji + "</size> <b>" + GetSocialManager.feelingData[currentFeeling].key + "</b>";

            titleText.SetText(newText);
        }

        //TODO create post persistent draft system
        public void SaveDraft()
        {

        }

        public override void Close()
        {
            if (!HasMadeChanges())
            {
                Destroy(gameObject);
                return;
            }

            List<ContextModal.Data> options = new List<ContextModal.Data>();
            ContextModal contextModal = UI.GetNewContextModal();
            options.Add(new ContextModal.Data(() => Destroy(gameObject)) { title = "Yes" });
            options.Add(new ContextModal.Data() { title = "No" });

            if (isEditMode)
            {
                contextModal.Setup("Discard Edit?", null, options);
            }
            else
            {
                // replace this with save draft prompt

                contextModal.Setup("Discard Post?", null, options);
            }
        }
    }
}
