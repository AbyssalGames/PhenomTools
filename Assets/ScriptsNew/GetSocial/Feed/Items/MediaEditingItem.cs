using UnityEngine;
using UnityEngine.UI;

namespace BlackBoxVR.GetSocial
{
    public class MediaEditingItem : MonoBehaviour
    {
        [SerializeField]
        private RawImage image = null;
        [SerializeField]
        private LayoutElement layout = null;

        private MediaEditingView mediaEditingView;
        private MediaData media;

        public void Initialize(MediaEditingView mediaEditingView, MediaData media)
        {
            this.mediaEditingView = mediaEditingView;
            this.media = media;

            media.GetTexture(texture =>
            {
                image.texture = texture;
                float ratio = (float)texture.width / texture.height;
                layout.preferredHeight = layout.preferredWidth / ratio;
            });
        }

        public void OnRemove()
        {
            mediaEditingView.Remove(media);
            Destroy(gameObject);
        }
    }
}
