using PhenomTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBoxVR.GetSocial
{
    public class FeedCreatePostSection : MonoBehaviour
    {
        [SerializeField]
        private ProfileImage profileImage = null;
        [SerializeField]
        private TMP_InputField inputField = null;
        [SerializeField]
        private GameObject noSubWarning = null;
        [SerializeField]
        private Button button = null;

        private FeedView feedView;

        public void Initialize(FeedView feedView)
        {
            this.feedView = feedView;
            profileImage.InitGetSocial(GetSocialManager.localUser);

            if (!GetSocialManager.CanPost)
            {
                inputField.placeholder.gameObject.SetActive(false);
                noSubWarning.SetActive(true);
                button.SetInteractable(false);
            }
        }

        public void OnClick()
        {
            ActivityController.OpenNewPostView(feedView);
        }
    }
}
