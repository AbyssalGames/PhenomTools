using System;
using GetSocialSdk.Core;
using PhenomTools;

namespace BlackBoxVR.GetSocial
{
    public class ThreadItem : PostItem
    {
        [NonSerialized]
        public ThreadView threadView;

        public override void Initialize(Activity activity, ScrollRectExtended scrollRect)
        {
            base.Initialize(activity, scrollRect);
            FirstBecameVisible();
        }

        protected override void FirstBecameVisible()
        {
            base.FirstBecameVisible();
            ForceRefresh(activity);
        }
    }
}
