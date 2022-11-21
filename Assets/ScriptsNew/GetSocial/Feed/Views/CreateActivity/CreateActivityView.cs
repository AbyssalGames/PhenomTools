using System;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using GetSocialSdk.Core;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

namespace BlackBoxVR.GetSocial
{
    public class CreateActivityView : MonoBehaviour
    {
        [Header("Activity Fields")]
        // [SerializeField]
        // protected ScrollRect scrollRect = null;
        // [SerializeField]
        // protected RectTransform rootRect = null;
        [SerializeField]
        protected LayoutElement keyboardExpander = null;
        [SerializeField]
        protected AdvancedInputFieldPlugin.AdvancedInputField inputField = null;
        [SerializeField]
        private GameObject openEditorButton = null;
        [SerializeField]
        private GameObject closeEditorButton = null;
        [SerializeField]
        protected Button postButton = null;
        [SerializeField]
        protected Button doneButton = null;
        [SerializeField]
        protected CreateActivityToolbar toolbar = null;
        [SerializeField]
        protected MediaPreviewSection mediaPreviewSection = null;
        [SerializeField]
        protected MentionSelectView mentionsView = null;
        [SerializeField]
        protected MediaEditingView mediaEditingView = null;
        [SerializeField]
        protected GameObject loadingView = null;
        [SerializeField]
        protected List<GameObject> ignoreList = null;

        protected PostActivityTarget target;
        protected ActivityItem activityItem;

        protected bool isEditMode;
        protected Activity preEditedActivity;
        protected bool isKeyboardOpen;

        protected List<MediaData> mediaList = new List<MediaData>();
        protected List<User> mentionsList = new List<User>();

        protected Activity activity => activityItem.activity;

        public virtual void Initialize()
        {
            target = PostActivityTarget.Timeline();
            Setup();
        }

        /// <summary>
        /// Initialize in edit mode
        /// </summary>
        /// <param name="activityItem">The activity item being edited</param>
        public virtual void Initialize(ActivityItem activityItem)
        {
            isEditMode = true;
            this.activityItem = activityItem;
            preEditedActivity = activityItem.activity;

            postButton.gameObject.SetActive(false);
            doneButton.gameObject.SetActive(true);

            inputField.SetText(activity.Text, true);
            mediaPreviewSection.Initialize(activity.MediaAttachments);
            Setup();
        }

        /// <summary>
        /// Runs regardless of how this is initialized
        /// </summary>
        protected virtual void Setup()
        {
            toolbar.Initialize(this);
            mediaEditingView.Initialize(this);
        }

        public virtual void InputFieldSelectionChanged(bool selected)
        {
            if(selected)
                OpenEditor();
            else
                CloseEditor();
        }

        public virtual void SelectInputField()
        {
            inputField.Select();
        }

        [ContextMenu("Open")]
        public virtual void OpenEditor()
        {
            if (isKeyboardOpen)
                return;
            
            isKeyboardOpen = true;
            
            if(closeEditorButton != null)
                closeEditorButton.gameObject.SetActive(true);
            if(openEditorButton != null)
                openEditorButton.gameObject.SetActive(false);

            toolbar.gameObject.SetActive(true);
            keyboardExpander.gameObject.SetActive(true);
            
            if(!EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                inputField.Select();
        }

        [ContextMenu("Close")]
        public virtual void CloseEditor()
        {
            if (!isKeyboardOpen || ignoreList.Contains(EventSystem.current.currentSelectedGameObject))
                return;
            
            isKeyboardOpen = false;
            
            if(closeEditorButton != null)
                closeEditorButton.gameObject.SetActive(false);
            if(openEditorButton != null)
                openEditorButton.gameObject.SetActive(true);

            keyboardExpander.minHeight = 0;

            toolbar.gameObject.SetActive(false);
            keyboardExpander.gameObject.SetActive(false);
            
            if(EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }

        protected virtual void Update()
        {
            if (isKeyboardOpen)
                keyboardExpander.minHeight = PhenomUtils.GetKeyboardHeightRelative((transform.parent as RectTransform).rect.height, false);
        }

        public virtual void OnTextChanged(string text)
        {
            if(isEditMode)
                doneButton.interactable = !string.IsNullOrWhiteSpace(text);
            else
                postButton.interactable = !string.IsNullOrWhiteSpace(text);
            
            LayoutRebuilder.MarkLayoutForRebuild(inputField.transform as RectTransform);
        }

        // public void OnBeginDrag()
        // {
        //     scrollRect.enabled = false;
        // }
        //
        // public void OnEndDrag()
        // {
        //     scrollRect.enabled = true;
        // }

        public virtual void AddMedia(MediaData media)
        {
            if (mediaList.Contains(media))
                return;

            mediaList.Add(media);
            mediaPreviewSection.gameObject.SetActive(true);
            mediaPreviewSection.UpdateMediaPreviews(mediaList);
        }

        public virtual void AddMedia(List<MediaData> newMedia)
        {
            mediaList.AddRange(newMedia);
            mediaList = mediaList.Distinct().ToList();
            mediaPreviewSection.gameObject.SetActive(true);
            mediaPreviewSection.UpdateMediaPreviews(mediaList);
        }

        public virtual void UpdateMedia(List<MediaData> newList)
        {
            mediaList = newList;
            mediaPreviewSection.gameObject.SetActive(mediaList.Count > 0);
            mediaPreviewSection.UpdateMediaPreviews(mediaList);
        }

        public virtual void Post()
        {
            if (!HasMadeChanges())
                return;

            loadingView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);

            GetContent(content => Communities.PostActivity(content, target, OnPostSuccess, OnPostError), new ActivityContent { Text = inputField.Text });
        }

        protected virtual void OnPostSuccess(Activity activity)
        {
            CloseEditor();
        }

        /// <summary>
        /// User completed editing the post
        /// </summary>
        public virtual void Done()
        {
            // if (string.IsNullOrWhiteSpace(inputField.Text))
            //     return;

            EventSystem.current.SetSelectedGameObject(null);

            if (HasMadeChanges())
            {
                GetContent(content => Communities.UpdateActivity(activityItem.activity.Id, content, OnEditSuccess, OnPostError), new ActivityContent { Text = inputField.Text });
                loadingView.SetActive(true);
            }
            else
            {
                Close();
            }
        }

        protected virtual void OnEditSuccess(Activity activity)
        {
            activityItem.ForceRefresh(activity);
            Destroy(gameObject);
        }

        protected virtual void OnPostError(GetSocialError error)
        {
            loadingView.SetActive(false);
            Debug.LogError(error.Message);
        }

        protected virtual void GetContent(Action<ActivityContent> callback, ActivityContent existingContent)
        {
            if(mediaList.Count > 0)
            {
                existingContent.AddProperty("Type", ActivityType.CustomMedia.ToString());

                GetSocialUtils.GetAttachmentsList(mediaList, attachmentList =>
                {
                    existingContent.AddMediaAttachments(attachmentList);
                    callback?.Invoke(existingContent);
                });
            }
            else
            {
                existingContent.AddProperty("Type", ActivityType.CustomText.ToString());
                callback?.Invoke(existingContent);
            }
        }

        public void ShowMediaEditingView()
        {
            mediaEditingView.Show(mediaList);
        }

        public virtual void ShowMentionSelectView()
        {

        }

        protected virtual bool HasMadeChanges()
        {
            if (!isEditMode)
                return !string.IsNullOrEmpty(inputField.Text) || mediaList.Count > 0;

            if (preEditedActivity.Text != inputField.Text || mediaList.Count != preEditedActivity.MediaAttachments.Count)
                return true;

            List<MediaData> preEditedMedia = preEditedActivity.MediaAttachments.ToMediaData();
            for (int i = 0; i < mediaList.Count; i++)
            {
                if (mediaList[i] != preEditedMedia[i])
                    return true;
            }

            return false;
        }

        public virtual void Close()
        {
            
        }
    }
}
