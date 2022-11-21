using UnityEngine;
using PhenomTools;
using GetSocialSdk.Core;
using TMPro;

namespace BlackBoxVR.GetSocial
{
    public class ReactionToggle : MonoBehaviour
    {
        [SerializeField]
        protected ToggleExtended toggle = null;
        [SerializeField]
        protected TextMeshProUGUI labelText = null;
        [SerializeField]
        protected ReactionSelectionPopup reactionSelectionPopup = null;

        protected ActivityItem activityItem;
        protected Activity activity;

        protected ReactionData[] reactionData => GetSocialManager.reactionData;

        public virtual void Initialize(ActivityItem activityItem)
        {
            this.activityItem = activityItem;
            activity = activityItem.activity;

            reactionSelectionPopup.Initialize(this);
            SetReactionWithoutNotify(activity.MyCurrentReactionIndex());
        }

        public virtual void OnDown()
        {
            activityItem.scrollRect.enabled = false;
        }

        public virtual void OpenReactionSelectionPopup()
        {
            activityItem.scrollRect.enabled = false;
            reactionSelectionPopup.Show();
        }

        public virtual void OnUp()
        {
            activityItem.scrollRect.enabled = true;

            if (reactionSelectionPopup.isVisible)
                reactionSelectionPopup.Hide();
        }

        public virtual void OnToggle(bool on)
        {
            if (on)
                SetReaction(0);
            else 
                RemoveReaction();
        }

        public virtual void SetReaction(int index)
        {
            toggle.SetIsOnWithoutNotify(true);
            
            if (activity.MyReactionsList.Count > 0)
            {
                string oldReactionKey = activity.MyReactionsList[0];

                if (!string.IsNullOrEmpty(reactionData[index].key) && oldReactionKey == reactionData[index].key)
                    return;

                Communities.RemoveReaction(oldReactionKey, activity.Id, null, e => Debug.Log(e.Message));
                activity.MyReactionsList.Remove(oldReactionKey);
                activity.SetReactionsCountSafe(oldReactionKey, activity.GetReactionsCountSafe(oldReactionKey) - 1);
            }

            if (string.IsNullOrEmpty(reactionData[index].key))
                return;

            Communities.AddReaction(reactionData[index].key, activity.Id, () => activityItem.ForceRefresh(activity), e => Debug.Log(e.Message));
            activity.MyReactionsList.Add(reactionData[index].key);
            activity.SetReactionsCountSafe(reactionData[index].key, activity.GetReactionsCountSafe(reactionData[index].key) + 1);

            labelText.SetText(reactionData[index].key.Replace(reactionData[index].key[0].ToString(), reactionData[index].key[0].ToString().ToUpper()));
            labelText.color = reactionData[index].color;
            activityItem.ForceRefresh(activity);
        }

        public virtual void SetReactionWithoutNotify(int index)
        {
            if (index < 0)
                return;

            toggle.SetIsOnWithoutNotify(true);

            labelText.SetText(reactionData[index].key.Replace(reactionData[index].key[0].ToString(), reactionData[index].key[0].ToString().ToUpper()));
            labelText.color = reactionData[index].color;
        }

        public virtual void RemoveReaction()
        {
            if (activity.MyReactionsList == null || activity.MyReactionsList.Count == 0)
                return;

            string oldReactionKey = activity.MyReactionsList[0];
            Communities.RemoveReaction(oldReactionKey, activity.Id, () => activityItem.ForceRefresh(activity), e => Debug.Log(e.Message));
            activity.MyReactionsList.Remove(oldReactionKey);
            activity.SetReactionsCountSafe(oldReactionKey, activity.GetReactionsCountSafe(oldReactionKey) - 1);

            labelText.SetText(reactionData[0].key);
            labelText.color = Color.gray;
        }
    }
}
