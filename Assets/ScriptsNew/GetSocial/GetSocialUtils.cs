using System.Linq;
using GetSocialSdk.Core;
using PhenomTools;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using EasyMobile;
using System.IO;
using System.Collections;
using UnityEngine.Android;

namespace BlackBoxVR.GetSocial
{
    public static class GetSocialUtils
    {
        public static int GetReactionsCountSafe(this Activity activity, string reaction)
        {
            return activity.ReactionsCount == null ? 0 : activity.GetReactionsCount(reaction.ToLower());
        }

        public static int GetAllReactionsCountSafe(this Activity activity)
        {
            return activity.ReactionsCount == null ? 0 : activity.ReactionsCount.Values.Sum();            
        }

        public static void SetReactionsCountSafe(this Activity activity, string reaction, int count)
        {
            if (activity.ReactionsCount == null)
                activity.ReactionsCount = new Dictionary<string, int>();

            if(activity.ReactionsCount.ContainsKey(reaction))
                activity.ReactionsCount[reaction] = count;
            else
                activity.ReactionsCount.Add(reaction, count);
        }

        public static int MyCurrentReactionIndex(this Activity activity)
        {
            if(activity.MyReactionsList != null && activity.MyReactionsList.Count > 0)
            {
                for (int i = 0; i < GetSocialManager.reactionData.Length; i++)
                {
                    if (GetSocialManager.reactionData[i].key == activity.MyReactionsList[0])
                        return i;
                }
            }

            return -1;
        }

        public static bool IsLocalUser(this User user) => user.Id == GetSocialManager.localUser.Id;
        
        //TODO replace this with Data Identity
        public static string GetDataId(this User user) => user.PublicProperties["DataId"];

        public static string GetTimeStamp(long unixTime)
        {
            TimeSpan timeSince = PhenomUtils.GetTimeSince(unixTime);// DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);

            if (timeSince.TotalSeconds < 60)
                return "just now";
            if (timeSince.TotalMinutes < 60)
                return (int)timeSince.TotalMinutes + "m";
            if (timeSince.TotalHours < 24)
                return (int)timeSince.TotalHours + "h";
            if (timeSince.TotalDays < 7)
                return (int)timeSince.TotalDays + "d";
            if (timeSince.TotalDays < 365)
                return ((int)timeSince.TotalDays / 7) + "w";
            
            return (int)timeSince.TotalDays / 365 + "y";
        }

        public static void GetAttachmentsList(List<MediaData> mediaList, Action<List<MediaAttachment>> callback)
        {
            List<MediaAttachment> list = new List<MediaAttachment>();

            for (int i = 0; i < mediaList.Count; i++)
            {
                MediaData media = mediaList[i];
                int iCache = i;

                if (media.mediaType == MediaType.Image)
                {
                    media.GetTexture(texture =>
                    //PhenomUtils.GetTextureFromURL(media.url, texture =>
                    {
                        list.Add(MediaAttachment.WithImage(texture));

                        if (iCache == mediaList.Count - 1)
                            callback?.Invoke(list);

                    });//, error => Debug.LogError(error));
                }
                else if (media.mediaType == MediaType.Video)
                {
                    list.Add(MediaAttachment.WithVideo(File.ReadAllBytes(media.url)));

                    if (iCache == mediaList.Count - 1)
                        callback?.Invoke(list);
                }
            }
        }

        public static void AskForPermissions(string[] permissionsToAsk, Action callback)
        {
            GetSocialManager.Instance.StartCoroutine(AskForPermissionsRoutine(permissionsToAsk, callback));
        }

        private static IEnumerator AskForPermissionsRoutine(string[] permissionsToAsk, Action callback)
        {
            bool[] permissions = new bool[permissionsToAsk.Length];
            bool[] permissionsAsked = new bool[permissionsToAsk.Length];
            List<Action> actions = new List<Action>();

            for (int i = 0; i < permissionsToAsk.Length; i++)
            {
                string permission = permissionsToAsk[i];
                int cache = i;
                actions.Add(new Action(() =>
                {
                    permissions[cache] = Permission.HasUserAuthorizedPermission(permission);
                    if (!permissions[cache] && !permissionsAsked[cache])
                    {
                        Permission.RequestUserPermission(permission);
                        permissionsAsked[cache] = true;
                        return;
                    }
                }));
            }

            for (int i = 0; i < permissionsAsked.Length;)
            {
                actions[i].Invoke();
                if (permissions[i])
                {
                    i++;
                }
                yield return new WaitForEndOfFrame();
            }

            callback?.Invoke();
        }

        public static List<MediaResult> GetMediaList(List<MediaAttachment> attachmentList)
        {
            List<MediaResult> list = new List<MediaResult>();

            foreach (MediaAttachment attachment in attachmentList)
            {
                if (!string.IsNullOrEmpty(attachment.ImageUrl))
                    list.Add(new MediaResult(MediaType.Image, attachment.ImageUrl, attachment.ImageUrl));
                else if (!string.IsNullOrEmpty(attachment.VideoUrl))
                    list.Add(new MediaResult(MediaType.Video, attachment.VideoUrl, attachment.VideoUrl));
            }

            return list;
        }

        public static List<MediaData> ToMediaData(this MediaResult[] inList)
        {
            List<MediaData> outList = new List<MediaData>();

            foreach(MediaResult result in inList)
                outList.Add(new MediaData(result));

            return outList;
        }

        public static List<MediaData> ToMediaData(this List<MediaAttachment> inList)
        {
            List<MediaData> outList = new List<MediaData>();

            foreach (MediaAttachment result in inList)
                outList.Add(new MediaData(result));

            return outList;
        }

        public static UnityEvent AddListenerAndReturn(this UnityEvent evt, UnityAction action)
        {
            evt.AddListener(action);
            return evt;
        }
    }
}
