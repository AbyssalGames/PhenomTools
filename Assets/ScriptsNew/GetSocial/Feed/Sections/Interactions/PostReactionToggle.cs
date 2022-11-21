using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class PostReactionToggle : ReactionToggle
    {
        [SerializeField]
        protected Transform iconParent = null;
        [SerializeField]
        protected GameObject currentIcon;

        public override void SetReaction(int index)
        {
            base.SetReaction(index);

            Destroy(currentIcon);
            currentIcon = Instantiate(reactionData[index].iconOnPrefab, iconParent);
        }

        public override void SetReactionWithoutNotify(int index)
        {
            if (index < 0)
                return;

            base.SetReactionWithoutNotify(index);

            Destroy(currentIcon);
            currentIcon = Instantiate(reactionData[index].iconOnPrefab, iconParent);
        }

        public override void RemoveReaction()
        {
            base.RemoveReaction();

            Destroy(currentIcon);
            currentIcon = Instantiate(reactionData[0].iconOffPrefab, iconParent);
        }
    }
}
