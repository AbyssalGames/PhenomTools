using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using GetSocialSdk.Core;
using BlackBoxVR.App;
using TMPro;

namespace BlackBoxVR.GetSocial
{
    public abstract class ActivityItem : DynamicVisibilityRect
    {
        public event Action<Activity> onRefresh;

        [Header("ActivityItem Fields")]
        [SerializeField]
        protected CommentsSection commentsSection = null;
        [SerializeField]
        protected InteractionSection interactionsSection = null;
        [SerializeField]
        protected TextMeshProUGUI timestampText = null;
        [SerializeField]
        protected RectTransform contentParent = null;
        [SerializeField]
        protected float refreshRate = 10f;

        [Serializable]
        public class ContextModalDataDict : SerializableDictionaryBase<string, ContextModal.Data> { }
        [SerializeField]
        public ContextModalDataDict contextModalData;

        [NonSerialized]
        public Activity activity;
        [NonSerialized]
        public ScrollRectExtended scrollRect;
        [NonSerialized]
        public bool authorIsFriend;
        [NonSerialized]
        public ActivityContent_Base content = null;

        protected IEnumerator refreshRoutine;

        public virtual void Initialize(Activity activity, ScrollRectExtended scrollRect)
        {
            this.activity = activity;
            this.scrollRect = scrollRect;

            content = Instantiate(ActivityController.contentPrefabs[GetActivityType(activity)], contentParent);
            content.Initialize(this);
        }
        
        public abstract void Edit();

        public virtual void Delete()
        {
            ContextModal contextModal = UI.GetNewContextModal();
            List<ContextModal.Data> list = new List<ContextModal.Data>
            {
                new ContextModal.Data(() => {
                        Communities.RemoveActivities(RemoveActivitiesQuery.ActivityIds(new List<string> { activity.Id }), 
                        OnActivityDeleted,
                        e => {
                            Debug.LogError(e.Message);
                            UI.GetNewContextModal().Setup("Error " + e.ErrorCode.ToString(), e.Message, null, true);
                        });
                    }) 
                { title = "Yes" },
                new ContextModal.Data() { title = "Cancel" }
            };

            contextModal.Setup("Are you sure you want to delete this post?", null, list, true);
        }

        protected virtual void OnActivityDeleted()
        {
            ActivityController.ActivityRemoved(activity);
            Destroy(gameObject);
        }

        public virtual void AddAuthorAsFriend()
        {
            if (authorIsFriend)
            {
                Debug.LogError("Author is already friend!");
                return;
            }

            
        }

        public virtual void RemoveAuthorAsFriend()
        {
            if (!authorIsFriend)
            {
                Debug.LogError("Author is not a friend!");
                return;
            }
        }

        public virtual void BlockAuthor()
        {
            if (GetSocialManager.IsUserBlocked(activity.Author.Id))
            {
                Debug.LogError("Author is already blocked!");
                return;
            }
        }

        public virtual void UnblockAuthor()
        {
            if (!GetSocialManager.IsUserBlocked(activity.Author.Id))
            {
                Debug.LogError("Author is not blocked!");
                return;
            }
        }

        public virtual void ShowContextModal()
        {
            List<ContextModal.Data> options = new List<ContextModal.Data>();

            if (activity.Author.IsLocalUser())
            {
                options.Add(contextModalData["Edit"]);
                options.Add(contextModalData["Delete"]);
            }
            else
            {
                if (authorIsFriend)
                {
                    options.Add(contextModalData["RemoveFriend"]);
                }
                else if (GetSocialManager.IsUserBlocked(activity.Author.Id))
                {
                    options.Add(contextModalData["Unblock"]);
                }
                else
                {
                    options.Add(contextModalData["AddFriend"]);
                    options.Add(contextModalData["Block"]);
                }
            }

            UI.GetNewContextModal(options, true);
        }

        public virtual void Refresh()
        {
            Communities.GetActivity(activity.Id, ForceRefresh, e => Debug.LogError(e.Message));
        }

        public virtual void ForceRefresh(Activity activity)
        {
            if(activity == null)
            {
                Destroy(gameObject);
                return;
            }    

            this.activity = activity;
            content.Refresh();
            interactionsSection.Refresh();
            onRefresh?.Invoke(activity);
        }

        public virtual CommentItem CreateCommentItem(Activity comment)
        {
            return commentsSection.CreateCommentItem(comment);
        }

        protected override void FirstBecameVisible()
        {
            base.FirstBecameVisible();
            Communities.IsFriend(UserId.Create(activity.Author.Id), isFriend => authorIsFriend = isFriend, e => Debug.LogError(e.Message));
            interactionsSection.Initialize(this);
            timestampText.SetText(GetSocialUtils.GetTimeStamp(activity.CreatedAt));
        }

        protected override void BecameVisible()
        {
            base.BecameVisible();
            refreshRoutine = PhenomUtils.RepeatActionByTime(refreshRate, Refresh);
        }

        protected override void BecameHidden()
        {
            base.BecameHidden();
            refreshRoutine?.Stop();
        }

        protected virtual void OnDestroy()
        {
            refreshRoutine?.Stop();
        }

        protected virtual ActivityType GetActivityType(Activity activity)
        {
            if (activity.Properties.TryGetValue("Type", out string value))
                return (ActivityType)Enum.Parse(typeof(ActivityType), value);
            else if (activity.MediaAttachments.Count > 0)
                return ActivityType.CustomMedia;
            else
                return ActivityType.CustomText;
        }

#if UNITY_EDITOR
        [Space(5)]
        public ActivityItem copyFrom;

        [ContextMenu("Copy ContextModalData")]
        public virtual void Temp()
        {
            contextModalData = copyFrom.contextModalData;
            foreach (var thing in contextModalData.Values)
                thing.color = Color.black;
        }
#endif
    }
}
