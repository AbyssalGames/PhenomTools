using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public abstract class ActivityContent_Base : MonoBehaviour
    {
        protected ActivityItem activityItem;
        protected Activity activity => activityItem.activity;

        public virtual void Initialize(ActivityItem activityItem)
        {
            this.activityItem = activityItem;
        }

        public virtual void Refresh() { }
        public virtual void BecameVisible() { }
        public virtual void FirstBecameVisible() { }
        public virtual void BecameHidden() { }
        public virtual void BecameFullyVisible() { }
        public virtual void BecamePartiallyHidden() { }
    }
}
