using System;
using System.Collections;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;
using PhenomTools;

namespace BlackBoxVR.GetSocial
{
    public partial class GetSocialManager
    {
        public static void BlockUser(string userId, Action onSuccess, Action<GetSocialError> onError) => AddToCustomList("BlockedUsers", userId, false, onSuccess, onError);
        public static void UnblockUser(string userId, Action onSuccess, Action<GetSocialError> onError) => RemoveFromCustomList("BlockedUsers", userId, onSuccess, onError);
        public static List<string> GetBlockedUsers() => GetCustomList("BlockedUsers");
        public static bool IsUserBlocked(string userId) => GetBlockedUsers().Contains(userId);
        
        public static void AddToCustomList(string listName, string value, bool allowDuplicates, Action onSuccess, Action<GetSocialError> onError)
        {
            List<string> currentList = GetCustomList(listName);

            if (!allowDuplicates && currentList.Contains(value))
                return;

            currentList.Add(value);
            UserUpdate update = new UserUpdate { _privateProperties = { { listName, JsonUtility.ToJson(currentList) } } };
            localUser.UpdateDetails(update, onSuccess, onError);
        }
        
        public static void AddRangeToCustomList(string listName, string[] values, bool allowDuplicates, Action onSuccess, Action<GetSocialError> onError)
        {
            List<string> currentList = GetCustomList(listName);

            foreach (string value in values)
            {
                if (allowDuplicates || !currentList.Contains(value))
                    currentList.Add(value);
            }

            UserUpdate update = new UserUpdate { _privateProperties = { { listName, JsonUtility.ToJson(currentList) } } };
            localUser.UpdateDetails(update, onSuccess, onError);
        }

        public static void RemoveFromCustomList(string listName, string value, Action onSuccess, Action<GetSocialError> onError)
        {
            List<string> currentList = GetCustomList(listName);

            if (!currentList.Contains(value))
                return;

            currentList.Remove(value);
            UserUpdate update = new UserUpdate { _privateProperties = { { listName, JsonUtility.ToJson(currentList) } } };
            localUser.UpdateDetails(update, onSuccess, onError);
        }

        public static List<string> GetCustomList(string listName)
        {
            if (localUser.PrivateProperties.TryGetValue(listName, out string rawList))
                return JsonUtility.FromJson<List<string>>(rawList);
            else
                return new List<string>();
        }

        public static bool IsValueInCustomList(string listName, string userId)
        {
            return GetCustomList(listName).Contains(userId);
        }
    }
}
