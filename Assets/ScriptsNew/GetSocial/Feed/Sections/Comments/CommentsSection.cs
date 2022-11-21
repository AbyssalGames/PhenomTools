using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using GetSocialSdk.Core;

namespace BlackBoxVR.GetSocial
{
    public abstract class CommentsSection : MonoBehaviour
    {
        protected ActivityItem activityItem;

        protected ActivitiesQuery query;
        protected PagingQuery<ActivitiesQuery> pagingQuery;
        protected string nextCursor;
        protected List<Activity> comments = new List<Activity>();
        protected OrderedDictionary<string, CommentItem> commentItems = new OrderedDictionary<string, CommentItem>();

        protected Activity activity => activityItem.activity;

        public virtual void Initialize(ActivityItem activityItem)
        {
            this.activityItem = activityItem;
            query = ActivitiesQuery.CommentsToActivity(activity.Id);
            pagingQuery = new PagingQuery<ActivitiesQuery>(query);
            ActivityController.onActivityRemoved += OnActivityRemoved;
        }

        public virtual void Initialize(ActivityItem activityItem, PagingActivitiesPackage pagingActivitiesPackage)
        {
            this.activityItem = activityItem;
            query = ActivitiesQuery.CommentsToActivity(activity.Id);
            pagingQuery = pagingActivitiesPackage.pagingQuery;
            nextCursor = pagingActivitiesPackage.nextCursor;
            ActivityController.onActivityRemoved += OnActivityRemoved;
        }

        private void OnDestroy()
        {
            ActivityController.onActivityRemoved -= OnActivityRemoved;
        }

        protected virtual void OnCommentsReceived(PagingResult<Activity> result)
        {
            nextCursor = result.NextCursor;
            comments.AddRange(result.Entries);
            Debug.Log("Comments set to length: " + comments.Count, gameObject);
            OnCommentsReceived(result.Entries);
        }
        
        protected abstract void OnCommentsReceived(List<Activity> comments);
        public abstract CommentItem CreateCommentItem(Activity comment);

        protected virtual void GetNextPage()
        {
            if (pagingQuery == null || string.IsNullOrEmpty(nextCursor))
                return;

            pagingQuery.Next(nextCursor);
            Communities.GetActivities(pagingQuery, OnCommentsReceived, Debug.LogError);
        }

        public virtual void Refresh()
        {
            Clear();

            pagingQuery = new PagingQuery<ActivitiesQuery>(query);
            Communities.GetActivities(pagingQuery, OnCommentsReceived, Debug.LogError);
        }

        public virtual void Clear()
        {
            GameObject tempParent = new GameObject("Temp");

            foreach (CommentItem item in commentItems.Values)
                item.transform.SetParent(tempParent.transform);

            Destroy(tempParent);

            commentItems.Clear();
            comments.Clear();
        }

        public bool TryGetAlreadyLoadedComments(out PagingActivitiesPackage activitiesPackage)
        {
            if (comments.Count > 0)
            {
                activitiesPackage = new PagingActivitiesPackage(comments, pagingQuery, nextCursor);
                Debug.Log("Built activity package: " + activitiesPackage.activities.Count, gameObject);
                return true;
            }

            activitiesPackage = null;
            return false;
        }

        private void OnActivityRemoved(Activity activity)
        {
            Debug.LogError("Remove Activity: " + activity.Id + ", Contains: " + commentItems.ContainsKey(activity.Id).ToString(), gameObject);
            if (commentItems.TryGetValue(activity.Id, out CommentItem item))
            {
                commentItems.Remove(activity.Id);
                Destroy(item.gameObject);
            }
        }
    }
}
