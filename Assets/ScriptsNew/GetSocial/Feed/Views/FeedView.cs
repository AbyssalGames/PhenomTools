using System;
using DG.Tweening;
using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

// GetSocial Docs specifically say that loading previous/arbitrary pages is not supported. so either we have to keep all the activity items loaded
// when a user scrolls, or find some other way of keeping track of pages.
// TODO should retain list of Activities, but destroy ActivityItems, and regenerate them when a user scrolls back up
namespace BlackBoxVR.GetSocial
{
    public class FeedView : MonoBehaviour
    {
        [SerializeField]
        protected ScrollRectExtended scrollRect = null;
        [SerializeField]
        protected FeedItem feedItemPrefab = null;
        [SerializeField]
        protected Transform activityItemParent = null;
        [SerializeField]
        protected RectTransform intersectionRect = null;
        [SerializeField]
        protected CanvasGroup canvasGroup = null;

        protected OrderedDictionary<string, FeedItem> feedItems = new OrderedDictionary<string, FeedItem>();
        protected ActivitiesQuery query;
        protected PagingQuery<ActivitiesQuery> pagingQuery;
        protected string nextCursor;

        protected FeedType feedType;

        public virtual void Initialize(FeedType feedType = FeedType.Global, string id = null, Action callback = null)
        {
            this.feedType = feedType;
            query = GetQuery(feedType, id);

            Setup(callback);
        }

        protected virtual void Setup(Action callback = null)
        {
            ActivityController.onActivityRemoved += OnActivityRemoved;

            pagingQuery = new PagingQuery<ActivitiesQuery>(query);
            Communities.GetActivities(pagingQuery, result =>
            {
                OnPostsReceived(result);
                callback?.Invoke();
            }, e => Debug.LogError(e.Message));

            canvasGroup.alpha = 0f;
        }

        protected virtual void OnPostsReceived(PagingResult<Activity> result)
        {
            nextCursor = result.NextCursor;

            for (int i = 0; i < result.Entries.Count; i++)
            {
                FeedItem feedItem = CreateFeedItem(result.Entries[i]);

                // Pagination - at the middle of the list, listen for when the new ActivityItem first becomes visible and then load the next page
                if (i == result.Entries.Count / 2)
                    feedItem.onFirstBecameVisible += GetNextPage;
            }

            if (canvasGroup.alpha < 1f)
                PhenomUtils.DelayActionByTime(.5f, () => canvasGroup.DOFade(1f, .5f));
        }

        public FeedItem CreateFeedItem(Activity activity)
        {
            Debug.Log("Create Feed Item: " + activity.Author.DisplayName);
            if (feedItems.ContainsKey(activity.Id) && feedItems[activity.Id] != null)
                //if (activities.Any(a => a.activity.Id == activity.Id))
                return null;

            FeedItem newItem = Instantiate(feedItemPrefab, activityItemParent);
            newItem.Initialize(activity, scrollRect);
            newItem.BeginVisibilityChecks(intersectionRect, scrollRect.onMove);
            newItem.feedView = this;

            //TODO sorting may be unnecessary since they're all loaded in order and never unloaded, and completely deleted upon refreshing
            if (feedItems.Count > 0 && activity.CreatedAt > feedItems.GetAt(feedItems.Count - 1).activity.CreatedAt)
            {
                for (int i = 0; i < feedItems.Count; i++)
                {
                    if (activity.CreatedAt > feedItems.GetAt(i).activity.CreatedAt)
                    {
                        newItem.transform.SetSiblingIndex(i);
                        feedItems.Insert(i, activity.Id, newItem);
                        return newItem;
                    }
                }
            }

            feedItems.Add(activity.Id, newItem);
            return newItem;
        }

        public virtual void GetNextPage()
        {
            if (pagingQuery == null || string.IsNullOrEmpty(nextCursor))
                return;

            pagingQuery.Next(nextCursor);
            Communities.GetActivities(pagingQuery, OnPostsReceived, e => Debug.LogError(e.Message));
        }

        public virtual void OnItemDestroyed(FeedItem feedItem)
        {
            feedItems.Remove(feedItem.activity.Id);
        }

        protected virtual void Refresh()
        {
            Clear();

            pagingQuery = new PagingQuery<ActivitiesQuery>(query);
            Communities.GetActivities(pagingQuery, OnPostsReceived, e => Debug.LogError(e.Message));
        }

        //TODO create filtered feeds
        public void ApplyFilter()
        {
            //Clear();

            //query = ActivitiesQuery.
            //pagingQuery = new PagingQuery<ActivitiesQuery>(query);
            //GetSocialManager.GetPosts(pagingQuery, OnPostsReceived, _ => { });

            //foreach (ActivityItem item in activities.Values)
            //    item.gameObject.SetActive(false);

            //foreach (KeyValuePair<Activity, ActivityItem> entry in activities)
            //{
            //    Communities.IsFriend(new UserId { Id = entry.Key.Author.Id },
            //        isFriend =>
            //        {
            //            // pagination wouldnt work correctly, need to regenerate entire list or something
            //        },
            //        e => Debug.LogError(e.Message));
            //}
        }

        public void Clear()
        {
            // unparenting and delaying destroy fixes a problem with textmeshpro caused when destroying a non-ui textmeshpro object that exists in a scrollrect, mainly the achievement icon
            GameObject tempParent = new GameObject("Temp");

            foreach (PostItem item in feedItems.Values)
                item.transform.SetParent(tempParent.transform);

            Destroy(tempParent);

            feedItems.Clear();
            canvasGroup.alpha = 0f;
        }

        private void OnActivityRemoved(Activity activity)
        {
            if(feedItems.TryGetValue(activity.Id, out FeedItem item))
            {
                feedItems.Remove(activity.Id);

                if (item != null)
                    Destroy(item.gameObject);
            }
        }

        public virtual void CheckForManualRefresh()
        {
            if(scrollRect.content.anchoredPosition.y < -20)
                Refresh();
        }

        public virtual void OnClose()
        {
            Destroy(gameObject);
        }

        protected virtual ActivitiesQuery GetQuery(FeedType feedType, string id)
        {
            switch (feedType)
            {
                case FeedType.Timeline:
                    return ActivitiesQuery.Timeline();
                case FeedType.Topic:
                    return ActivitiesQuery.ActivitiesInTopic(id);
                case FeedType.User:
                    return ActivitiesQuery.FeedOf(UserId.Create(id));
                default:
                    return ActivitiesQuery.Everywhere();
            }
        }

        protected virtual void Reset()
        {
            activityItemParent = transform.FindDeepChild("ActivityItemParent");
            scrollRect = gameObject.GetComponentInChildren<ScrollRectExtended>();
            intersectionRect = transform.FindDeepChild("IntersectionRect") as RectTransform;
        }
    }
}
