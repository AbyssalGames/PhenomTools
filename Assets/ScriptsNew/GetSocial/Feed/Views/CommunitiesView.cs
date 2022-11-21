using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class CommunitiesView : MonoBehaviour
    {
        [SerializeField]
        private GlobalFeedView globalFeedView = null;
        [SerializeField]
        private FriendsView friendsView = null;

        public void Initialize(bool toFriendsList = false)
        {
            globalFeedView.gameObject.SetActive(!toFriendsList);
            friendsView.gameObject.SetActive(toFriendsList);

            globalFeedView.Initialize();
            friendsView.Initialize();
        }

        public void HideSubViews()
        {
            globalFeedView.gameObject.SetActive(false);
            friendsView.gameObject.SetActive(false);
        }

        public void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
