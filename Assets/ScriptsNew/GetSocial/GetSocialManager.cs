using System;
using System.Collections.Generic;
// using BlackBoxVR.MobileApp.ServerData;
using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public partial class GetSocialManager : PhenomTools.Singleton<GetSocialManager>
    {
        public static CurrentUser localUser;
        public static Dictionary<User, Texture> userAvatarCache = new Dictionary<User, Texture>();

        private const string DEFAULT_PROVIDER = "BBVR";
        private const string DEBUG_PROVIDER = "Debug";

        [SerializeField]
        private ReactionData[] _reactionData = null;
        public static ReactionData[] reactionData => Instance._reactionData;

        [SerializeField]
        private FeelingData[] _feelingData = null;
        public static FeelingData[] feelingData => Instance._feelingData;

        #region Initialization Methods
        ///<summary>
        /// Initialize the GetSocialManager using the JWT Authentication.
        ///</summary>
        public static void Init(string jwt, Action onSuccess, Action<GetSocialError> onError)
        {
            Init(Identity.Trusted(DEFAULT_PROVIDER, jwt), onSuccess, onError);
        }

        ///<summary>
        /// Initialize the GetSocialManager using a username and password.
        ///</summary>
        public static void Init(string username, string passwordToSend, Action onSuccess, Action<GetSocialError> onError)
        {
            Init(Identity.Custom(DEBUG_PROVIDER, username, passwordToSend), onSuccess, onError);
        }

        ///<summary>
        /// Initialize the GetSocialManager using a specified identity.
        ///</summary>
        private static void Init(Identity id, Action onSuccess, Action<GetSocialError> onError)
        {
            if (GetSocialSdk.Core.GetSocial.IsInitialized)
                return;

            GetSocialSdk.Core.GetSocial.Init(id, () =>
                {
                    localUser = GetSocialSdk.Core.GetSocial.GetCurrentUser();
                    InitializeUserIfNeeded();
                    onSuccess?.Invoke();
                }, 
                onError);
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if(transform.parent == null)
                DontDestroyOnLoad(gameObject);

            Instance = this;
        }
        #endregion

        #region User Methods
        public static void InitializeUserIfNeeded()
        {
            // if (!localUser.PublicProperties.ContainsKey("DataId"))
            //     APIManager.Instance.GetUserProfile(response => BulkUpdateUserDetails(response.Data));
        }

        public static void GetUserAvatar(User user, Action<Texture> onSuccess, Action<string> onError)
        {
            if (userAvatarCache.TryGetValue(user, out Texture tex))
            {
                onSuccess?.Invoke(tex);
            }
            else
            {
                PhenomUtils.GetTextureFromURL(user.AvatarUrl, texture =>
                {
                    userAvatarCache.Add(user, texture);
                    onSuccess?.Invoke(texture);
                }, onError);
            }
        }

        // public static void BulkUpdateUserDetails(UserProfile profile)
        // {
        //     Debug.Log("Start Bulk User Details Update for user: " + localUser.DisplayName + ", ID: " + localUser.Id);
        //
        //     localUser.UpdateDetails(new UserUpdate
        //         {
        //             DisplayName = profile.username,
        //             _publicProperties =
        //             {
        //                 { "DataId", ServerDataManager.SharedInstance.GameSparksUserId() },
        //                 //{ "AvatarType", profile.playerPrefs.avatarType }
        //             }
        //         },
        //         () => localUser = GetSocialSdk.Core.GetSocial.GetCurrentUser(),
        //         e => Debug.LogError(e.Message));
        // }

        //public static void SyncFriendsList(Action<int> onComplete)
        //{
        //    APIManager.Instance.SocialGetFriendsList(response =>
        //    {
        //        List<string> ids = new List<string>();
        //        foreach (SocialFriendInfo friendInfo in response.Data.Result.friendList)
        //        {
        //            ids.Add(friendInfo.userID);

        //            //if (!string.IsNullOrEmpty(friendInfo.getSocialId))
        //            //    ids.Add(friendInfo.getSocialId);

        //            //would need to store getsocial id on backend service -- this wouldn't work because then everyone would have to log in to populate it, and we can't transfer them as a friend to other's friends lists until they do
        //            //or figure out how to make getsocial's id the same as the ones we already have -- doesn't look like we can actually set this, as GetSocial automatically assigns them an id, plus theirs is only integers, while gamespark's includes letters too

        //            //would need to create a separate program/function that would loop through all current users on gamesparks and create a GetSocial account for them and update the gamesparks data to include the new GetSocial UserID
        //            //then after every user has a GetSocial account, we can populate the friends list, which would probably take a while, as each entry would have to loop through the entire list of users. O(n^2)

        //            //should be able to hardcode the jwt in the separate program.
        //        }

        //        UserIdList userIds = UserIdList.Create(ids);
        //        Communities.SetFriends(userIds, onComplete, e => Debug.LogError(e.Message));

        //        //if (ids.Count > 0)
        //        //    Communities.AddFriends(new UserIdList { UserIds = ids }, onComplete, e => Debug.LogError(e.Message));
        //        //else
        //        //    onComplete?.Invoke(-1);
        //    });
        //}

        //Blocking system will be replaced by built-in system provided by GetSocial, just waiting on an update from them.
        


        

        //public static void SendFriendRequest(string userId, Action onSuccess, Action<GetSocialError> onError)
        //{
        //    //can use the add friends function, and when the other user logs on again, compare to cache of their current friends, if there are any new ones show the request dialogue.
        //    //if they accept the request, then add the requesting user to the cache. if they reject, call the remove friends function.
        //    //a problem with this is that the sender won't be able to tell if, when, and how the other user responds, and will only be able to assume that they have accepted.
        //    //could put friends list cache in the public properties to fix this maybe
        //}

        //public static void GetProfileImage(string url, Action<Texture> onSuccess, Action<string> onError)
        //{
        //    if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        //    {
        //        onError?.Invoke("Invalid URL: " + url);
        //        return;
        //    }

        //    Instance.StartCoroutine(GetProfileImageRoutine(url, onSuccess, onError));
        //}

        //private static IEnumerator GetProfileImageRoutine(string url, Action<Texture> onSuccess, Action<string> onError)
        //{
        //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        //    yield return www.SendWebRequest();

        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        onError?.Invoke(www.error);
        //    }
        //    else
        //    {
        //        onSuccess?.Invoke(DownloadHandlerTexture.GetContent(www));
        //    }
        //}

        //public static void AddIdentity()
        //{
            // TO-DO: Revise this
            //    user.AddIdentity(id,
            //    () =>
            //        {
            //            Debug.Log("New Identity added!");

            //            FinalizeLogIn?.Invoke(username, passwordToSend, this.keepMeLoggedInToggle.isOn, false);
            //    LoginStatus?.Invoke(true);
            //},
            //        c =>
            //        {
            //            Debug.LogError("Conflict: " + c.DisplayName);

            //            GetSocial.SwitchUser(id, () =>
            //            {
            //                FinalizeLogIn?.Invoke(username, passwordToSend, this.keepMeLoggedInToggle.isOn, false);
            //                LoginStatus?.Invoke(true);
            //            }, e => Debug.LogError(e.Message));
            //        },
            //e => Debug.LogError(e.Message));
            //}
        //}

        //public static void IsFriend(string userId, Action<bool> onResult)
        //{
        //    // TO-DO: Refactor this later to not create this every single time
        //    Communities.IsFriend(UserId.Create(userId),
        //        result =>
        //        {
        //            // Return if user was friend or not
        //            onResult?.Invoke(result);
        //        },
        //        e => Debug.LogError($"Error check if user was friend: {e.Message}"));
        //}
        #endregion

        #region Post Methods
        /// <summary>
        /// Checks subscription status
        /// </summary>
        //TODO Move this to a different class
        public static bool CanPost
        {
            get => true;// ContextManager.Instance.userContext.SubData.amActive || ContextManager.Instance.userContext.SubData.amPartnerUser();
        }
        #endregion
    }
}