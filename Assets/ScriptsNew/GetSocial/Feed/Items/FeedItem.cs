using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBoxVR.GetSocial
{
    public class FeedItem : PostItem
    {
        [Header("FeedItem Fields")]
        [SerializeField]
        private LayoutElement contentLayoutElement = null;
        [SerializeField]
        private GameObject fullPostVisibilityToggle = null;
        //[SerializeField]
        [NonSerialized]
        public FeedView feedView;
        private new PreviewCommentsSection commentsSection => base.commentsSection as PreviewCommentsSection;

        public override void Initialize(Activity activity, ScrollRectExtended scrollRect)
        {
            base.Initialize(activity, scrollRect);
            commentsSection.Initialize(this);

            // constrain really long posts to a square 
            if (contentParent.sizeDelta.y > contentParent.sizeDelta.x)
            {
                contentLayoutElement.preferredHeight = contentParent.sizeDelta.x;
                fullPostVisibilityToggle.SetActive(true);
            }
        }

        public void OpenThreadButton() => OpenThread();
        public ThreadView OpenThread()
        {
            ThreadView threadView = commentsSection.TryGetAlreadyLoadedComments(out PagingActivitiesPackage pagingActivitiesPackage) 
                ? ActivityController.OpenThread(this, pagingActivitiesPackage) 
                : ActivityController.OpenThread(this);

            threadView.threadItem.onRefresh += ForceRefresh;
            return threadView;
        }

        public void OnCommentsButton()
        {
            ThreadView threadView = OpenThread();
            threadView.OnCommentsButton();
        }

        public void SetMyCommentText(string text)
        {
            commentsSection.SetMyCommentText(text);
        }

        public void OnToggleFullPostVisibility(bool on)
        {
            if (on)
                contentLayoutElement.preferredHeight = -1;
            else
                contentLayoutElement.preferredHeight = contentParent.sizeDelta.x;
        }

        protected override void FirstBecameVisible()
        {
            base.FirstBecameVisible();
            commentsSection.Refresh();
        }

        // protected override void BecameVisible()
        // {
        //     base.BecameVisible();
        //     commentsSection.Refresh();
        // }
    }
}
