using GetSocialSdk.Core;
using TMPro;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class CommentPreviewItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI commentText = null;

        public void Initialize(Activity comment)
        {
            commentText.SetText("<b>" + comment.Author.DisplayName + "</b> " + comment.Text);
        }
    }
}
