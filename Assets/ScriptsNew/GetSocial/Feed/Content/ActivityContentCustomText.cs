using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class ActivityContentCustomText : ActivityContent_Base
    {
        [SerializeField]
        protected RichSocialText text = null;

        public override void FirstBecameVisible()
        {
            text.Initialize(activityItem);
        }

        public override void Refresh()
        {
            text.Initialize(activityItem);
        }

        protected virtual void Reset()
        {
            text = GetComponentInChildren<RichSocialText>();
        }
    }
}
