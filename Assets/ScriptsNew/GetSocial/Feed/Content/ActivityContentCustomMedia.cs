using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ActivityContentCustomMedia : ActivityContentCustomText
    {
        [SerializeField]
        private MediaPreviewSection mediaPreviewSection = null;

        public override void Refresh()
        {
            base.Refresh();
            mediaPreviewSection.Initialize(activity.MediaAttachments);
        }

        public override void FirstBecameVisible()
        {
            base.FirstBecameVisible();
            mediaPreviewSection.Initialize(activity.MediaAttachments);
        }
    }
}
