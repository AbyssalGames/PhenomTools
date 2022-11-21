using GetSocialSdk.Core;
using System;
using UnityEngine;
// using BlackBoxVR.MobileApp.ServerData;
using TMPro;

namespace BlackBoxVR.GetSocial
{
    public abstract class PostItem : ActivityItem
    {
        [Header("PostItem Fields")]
        [SerializeField]
        protected PlayerProfileTag profileTag = null;
        [SerializeField]
        protected TextMeshProUGUI feelingAndMentionsText = null;

        // [NonSerialized]
        // public SocialFriendInfo data;

        public void OnOpenReactionsView()
        {
            //ThreadView threadView = ActivityController.OpenThread(this);
            //threadView.OnOpenReactionsList();
        }

        public override void Edit()
        {
            ActivityController.OpenEditPostView(this);
        }

        protected override void BecameVisible()
        {
            base.BecameVisible();
            content.BecameVisible();
        }

        protected override void FirstBecameVisible()
        {
            base.FirstBecameVisible();

            //TODO replace this with GetSocial friend system
            string dataId = activity.Author.GetDataId();
            // APIManager.Instance.SocialGetFriendData(dataId, response => data = response.Data);
            profileTag.Init(dataId);

            string newText = "";
            if (activity.Properties.TryGetValue("Feeling", out string rawFeeling)) 
            {
                int feeling = int.Parse(rawFeeling);

                if(feeling > 0)
                    newText += "Feeling<size=12> " + GetSocialManager.feelingData[feeling].emoji + " </size><b>" + GetSocialManager.feelingData[feeling].key + "</b>";
            }

            if(activity.Mentions.Count > 0)
            {
                Communities.GetUser(UserId.Create(activity.Mentions[0].UserId), user =>
                {
                    newText += " with " + user.DisplayName;

                    if (activity.Mentions.Count == 2)
                        newText += "and<b> 1 other</b>";
                    else if (activity.Mentions.Count > 2)
                        newText += "and<b> " + (activity.Mentions.Count - 1) + " others</b>";

                }, error => Debug.LogError(error.Message));
            }

            feelingAndMentionsText.gameObject.SetActive(!string.IsNullOrEmpty(newText));
            feelingAndMentionsText.SetText(newText);
            content.FirstBecameVisible();
        }

        protected override void BecameHidden()
        {
            base.BecameHidden();
            content.BecameHidden();
        }

        protected override void BecameFullyVisible()
        {
            base.BecameFullyVisible();
            content.BecameFullyVisible();
        }

        protected override void BecamePartiallyHidden()
        {
            base.BecamePartiallyHidden();
            content.BecamePartiallyHidden();
        }
    }
}
