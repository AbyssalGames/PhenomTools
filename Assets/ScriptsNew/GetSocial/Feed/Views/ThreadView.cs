using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ThreadView : MonoBehaviour
    {
        public ThreadItem threadItem = null;
        public ThreadCommentsSection commentsSection = null;
        public CreateCommentView createCommentView = null;

        [SerializeField]
        private ScrollRectExtended scrollRect = null;

        private FeedItem feedItem;
        private Activity activity;

        public void Initialize(FeedItem feedItem)
        {
            this.feedItem = feedItem;
            activity = feedItem.activity;
            Setup();
        }

        public void Initialize(FeedItem feedItem, PagingActivitiesPackage pagingActivitiesPackage)
        {
            this.feedItem = feedItem;
            activity = feedItem.activity;
            Setup(pagingActivitiesPackage);
        }

        public void Initialize(Activity activity)
        {
            this.activity = activity;
            Setup();
        }

        private void Setup(PagingActivitiesPackage pagingActivitiesPackage = null)
        {
            ActivityController.onActivityRemoved += OnActivityRemoved;

            threadItem.Initialize(activity, scrollRect);
            threadItem.threadView = this;
            
            if(pagingActivitiesPackage == null)
                commentsSection.Initialize(threadItem);
            else
                commentsSection.Initialize(threadItem, pagingActivitiesPackage);
            
            createCommentView.Initialize(this);
        }

        public void OnCommentsButton()
        {
            // simply scroll down to the comments section
        }

        public void CreateCommentItems(Activity comment)
        {
            threadItem.CreateCommentItem(comment).transform.SetAsFirstSibling();
            feedItem.CreateCommentItem(comment).transform.SetAsFirstSibling();
        }

        public void CheckForManualRefresh()
        {
            if (scrollRect.content.anchoredPosition.y < -20)
                Refresh();
        }

        public void Refresh()
        {
            commentsSection.Refresh();
        }

        private void OnActivityRemoved(Activity activity)
        {
            if (activity != this.activity)
                return;


        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
