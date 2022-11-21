using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class FriendsView : MonoBehaviour
    {
        [SerializeField]
        private FriendItem friendItemPrefab = null;
        [SerializeField]
        private Transform itemParent = null;
        [SerializeField]
        private RectTransform intersectionRect = null;

        protected OrderedDictionary<User, FriendItem> friendItems = new OrderedDictionary<User, FriendItem>();

        protected FriendsQuery query;
        protected PagingQuery<FriendsQuery> pagingQuery;
        protected string nextCursor;

        public void Initialize()
        {
            query = FriendsQuery.OfUser(UserId.CurrentUser());
        }

        public virtual void GetNextPage()
        {
            if (pagingQuery == null || string.IsNullOrEmpty(nextCursor))
                return;

            pagingQuery.Next(nextCursor);
            Communities.GetFriends(pagingQuery, OnPostsRecieved, e => Debug.LogError(e.Message));
        }

        protected virtual void OnPostsRecieved(PagingResult<User> result)
        {
            nextCursor = result.NextCursor;

            for (int i = 0; i < result.Entries.Count; i++)
            {
                User user = result.Entries[i];
                FriendItem friendItem = CreateItem(user);

                // Pagination - at the middle of the list, listen for when the new ActivityItem first becomes visible and then load the next page
                if (i == result.Entries.Count / 2)
                {
                    friendItem.onFirstBecameVisible += GetNextPage;
                }
            }
        }

        protected FriendItem CreateItem(User user)
        {
            if (friendItems.ContainsKey(user) && friendItems[user] != null)
                //if (activities.Any(a => a.activity.Id == activity.Id))
                return null;

            FriendItem newItem = Instantiate(friendItemPrefab, itemParent);
            newItem.BeginVisibilityChecks(intersectionRect);
            newItem.Initialize(user);

            friendItems.Add(user, newItem);
            return newItem;
        }
    }
}
