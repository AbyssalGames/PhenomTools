using GetSocialSdk.Core;
using PhenomTools;
using System;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ActivityController : PhenomTools.Singleton<ActivityController>
    {
        public static event Action<Activity> onActivityRemoved;
        public static event Action<Activity> onActivityEdited;

        [SerializeField]
        private CommunitiesView communitiesViewPrefab = null;
        [SerializeField]
        private ThreadView threadViewPrefab = null;
        [SerializeField]
        private CreatePostView createFeedPostViewPrefab = null;

        [Serializable]
        public class PostContentDict : SerializableDictionaryBase<ActivityType, ActivityContent_Base> { }
        [SerializeField]
        private PostContentDict _contentPrefabs = null;
        public static PostContentDict contentPrefabs => Instance._contentPrefabs;

        public static CommunitiesView OpenCommunitiesView()
        {
            CommunitiesView newFeedView = Instantiate(Instance.communitiesViewPrefab, Instance.transform);
            newFeedView.Initialize();
            return newFeedView;
        }

        public static CreatePostView OpenNewPostView(FeedView targetFeed)
        {
            CreatePostView newCreatePostView = Instantiate(Instance.createFeedPostViewPrefab, Instance.transform);
            newCreatePostView.Initialize(targetFeed);
            return newCreatePostView;
        }

        public static CreatePostView OpenEditPostView(ActivityItem activityItem)
        {
            CreatePostView newCreatePostView = Instantiate(Instance.createFeedPostViewPrefab, Instance.transform);
            newCreatePostView.Initialize(activityItem);
            return newCreatePostView;
        }

        //public static FeedView OpenTopicFeed(string topicId, Transform parent = null)
        //{
        //    FeedView newFeedView = Instantiate(Instance.feedViewPrefab, parent == null ? Instance.transform : parent);
        //    newFeedView.Initialize(topicId);
        //    return newFeedView;
        //}

        //public static FeedView OpenUserFeed(UserId userID, Transform parent = null)
        //{
        //    FeedView newFeedView = Instantiate(Instance.feedViewPrefab, parent == null ? Instance.transform : parent);
        //    newFeedView.Initialize(userID);
        //    return newFeedView;
        //}

        /// <summary>
        /// Open a Thread directly from a feed post.
        /// </summary>
        /// <param name="feedItem"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ThreadView OpenThread(FeedItem feedItem, Transform parent = null)
        {
            ThreadView newThreadView = Instantiate(Instance.threadViewPrefab, parent == null ? Instance.transform : parent);
            newThreadView.Initialize(feedItem);
            return newThreadView;
        }

        /// <summary>
        /// Open a Thread directly from a feed post and preserve the comments that it has already loaded.
        /// </summary>
        /// <param name="feedItem"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ThreadView OpenThread(FeedItem feedItem, PagingActivitiesPackage pagingActivitesPackage, Transform parent = null)
        {
            ThreadView newThreadView = Instantiate(Instance.threadViewPrefab, parent == null ? Instance.transform : parent);
            newThreadView.Initialize(feedItem, pagingActivitesPackage);
            return newThreadView;
        }

        /// <summary>
        /// Open a Thread indirectly, likely through a notification or something outside of the feed.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ThreadView OpenThread(Activity activity, Transform parent = null)
        {
            ThreadView newThreadView = Instantiate(Instance.threadViewPrefab, parent == null ? Instance.transform : parent);
            newThreadView.Initialize(activity);
            return newThreadView;
        }

        public static void ActivityRemoved(Activity activity)
        {
            onActivityRemoved?.Invoke(activity);
        }
    }
}
