using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using PhenomTools;
using TMPro;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class InteractionSection : MonoBehaviour
    {
        [SerializeField]
        protected ReactionToggle reactionToggle = null;
        [SerializeField]
        protected TextMeshProUGUI reactionCountText = null;
        [SerializeField]
        protected GameObject[] reactionIcons = null;

        [NonSerialized]
        public List<UserReactions> reactionsList = new List<UserReactions>();

        protected ActivityItem activityItem;
        protected Activity activity => activityItem.activity;

        public virtual void Initialize(ActivityItem activityItem)
        {
            this.activityItem = activityItem;
            reactionToggle.Initialize(activityItem);
        }

        public virtual void Refresh()
        {
            int reactionCount = activity.GetAllReactionsCountSafe();
            reactionCountText.gameObject.SetActive(reactionCount > 0);

            for (int i = 0; i < 6; i++)
                reactionIcons[i].SetActive(activity.GetReactionsCountSafe(GetSocialManager.reactionData[i].key) > 0);

            if (reactionCount > 0)
                Communities.GetReactions(new PagingQuery<ReactionsQuery>(ReactionsQuery.ForActivity(activity.Id)), a => SetReactionsInfo(reactionCount, a.Entries), e => Debug.LogError(e.Message));
        }

        protected virtual void SetReactionsInfo(int totalReactionCount, List<UserReactions> reactionsList)
        {
            string username;
            if (activity.MyReactionsList.Count > 0)
                username = "You";
            else
                username = reactionsList[0].User.DisplayName;

            this.reactionsList = reactionsList;

            if (totalReactionCount == 1)
                reactionCountText.SetText(username);
            else if (totalReactionCount == 2)
                reactionCountText.SetText(username + " and 1 other");
            else
                reactionCountText.SetText(username + " and " + (totalReactionCount - 1).ToBigNumberString() + " others");

            for (int i = 0; i < 6; i++)
                reactionIcons[i].SetActive(activity.GetReactionsCountSafe(GetSocialManager.reactionData[i].key) > 0);
            
            reactionToggle.SetReactionWithoutNotify(activity.MyCurrentReactionIndex());
        }        

        public virtual void OpenReactionsView()
        {

        }
    }
}
