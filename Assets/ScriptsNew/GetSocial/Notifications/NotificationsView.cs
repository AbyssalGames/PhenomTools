using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class NotificationsView : MonoBehaviour
    {
        [SerializeField]
        private NotificationItem notificationItem = null;
        [SerializeField]
        private Transform itemParent = null;
        [SerializeField]
        private RectTransform intersectionRect = null;

        protected OrderedDictionary<Notification, NotificationItem> notificationItems = new OrderedDictionary<Notification, NotificationItem>();

        protected NotificationsQuery query;
        protected PagingQuery<NotificationsQuery> pagingQuery;
        protected string nextCursor;

        public void Initialize()
        {
            query = NotificationsQuery.WithAllStatuses();
            //APIManager.Instance.GetNotifications(OnGameNotificationsReceived);
        }

        public virtual void GetNextPage()
        {
            if (pagingQuery == null || string.IsNullOrEmpty(nextCursor))
                return;

            pagingQuery.Next(nextCursor);
            Notifications.Get(pagingQuery, OnSocialNotificationsReceived, e => Debug.LogError(e.Message));
        }

        protected virtual void OnSocialNotificationsReceived(PagingResult<Notification> result)
        {
            nextCursor = result.NextCursor;

            for (int i = 0; i < result.Entries.Count; i++)
            {
                Notification notification = result.Entries[i];
                NotificationItem item = CreateItem(notification);

                // Pagination - at the middle of the list, listen for when the new ActivityItem first becomes visible and then load the next page
                if (i == result.Entries.Count / 2)
                {
                    item.onFirstBecameVisible += GetNextPage;
                }
            }
        }

        //private void OnGameNotificationsReceived(ModelDataResponse<TrayNotifications> response)
        //{

        //}

        protected NotificationItem CreateItem(Notification notification)
        {
            if (notificationItems.ContainsKey(notification) && notificationItems[notification] != null)
                //if (activities.Any(a => a.activity.Id == activity.Id))
                return null;

            NotificationItem newItem = Instantiate(notificationItem, itemParent);
            newItem.BeginVisibilityChecks(intersectionRect);
            newItem.Initialize(notification);

            notificationItems.Add(notification, newItem);
            return newItem;
        }
    }
}
