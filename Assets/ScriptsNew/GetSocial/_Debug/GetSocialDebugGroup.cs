using System;
using System.Collections;
using System.Collections.Generic;
// using BlackBoxVR.MobileApp;
// using BlackBoxVR.MobileApp.ServerData;
using GetSocialSdk.Core;
using UnityEngine;
using PhenomTools;

namespace BlackBoxVR.GetSocial
{
    public class GetSocialDebugGroup : DebugGroup
    {
        public void LoginAsStest()
        {
            GetSocialManager.Init("stest", "stest_1", null, Debug.LogError);
        }
        public void LoginAsStest1()
        {
            GetSocialManager.Init("stest1", "stest_1", null, Debug.LogError);
        }
        
        public void TestCreateGetSocialUserWithoutCreds()
        {
            GetSocialSdk.Core.GetSocial.Init();
            GetSocialSdk.Core.GetSocial.AddOnInitializedListener(() =>
            {
                // string id = "5d7bd66478be5904f9e97c36"; //Arena4
                string id = "5d7bd8db61094004f15632a8"; //Arena5
                
                //We can create an Identity in GetSocial that is represented by the ID of the user we are trying to make a GetSocial account for
                //This would not be the Identity that the user would later on use to log into GetSocial
                Identity identity = Identity.Custom("Data", id, "p"); //password/accessToken field cannot be empty, but value doesn't actually matter
                
                //init with identity to see if GetSocial has an identity setup already for a user with this DataID
                GetSocialSdk.Core.GetSocial.SwitchUser(identity, () =>
                {
                    Debug.Log("Got User: " + identity.ProviderUserId);
                },
                error =>
                {
                    Debug.LogError(error);

                    //5070: Invalid user - When this is the case we know this user needs a GetSocial account created for them
                    if (error.ErrorCode == 5070)
                    {
                        GetSocialSdk.Core.GetSocial.ResetUser(() =>
                        {
                            Debug.Log("User Reset back to anonymous");
                            CurrentUser currentUser = GetSocialSdk.Core.GetSocial.GetCurrentUser();
                            currentUser.AddIdentity(identity, () =>
                                {
                                    Debug.Log("Created GetSocial Identity: " + identity.ProviderUserId);
                                    // APIManager.Instance.SocialGetFriendUserProfile(id, response =>
                                    // {
                                    //     BulkUpdateUserDetails(currentUser, response.Data, id);
                                    // });
                                },
                                user =>
                                {
                                    Debug.LogError("Identity Already Exists: " + identity.ProviderUserId);
                                }, Debug.LogError);
                        }, Debug.LogError);
                    }
                    
                });
                
                //At a later point, when the user logs in for the first time, they will add the trusted identity using the JWT to the same account, in addition to this "Data" one.
                //The data identity is also useful later on as it can be used to get the DataId for use in AWS API calls.
                string dataId = GetSocialSdk.Core.GetSocial.GetCurrentUser().Identities["Data"];
                Debug.Log("Get Data ID from GetSocial Identities: " + dataId);
            });
        }
        
        // public static void BulkUpdateUserDetails(CurrentUser currentUser, UserProfile profile, string dataId)
        // {
        //     Debug.Log("Start Bulk User Details Update for user: " + currentUser.DisplayName + ", ID: " + currentUser.Id);
        //
        //     currentUser.UpdateDetails(new UserUpdate
        //     {
        //         DisplayName = profile.username,
        //         _publicProperties =
        //         {
        //             { "DataId", dataId }
        //         }
        //     },
        //     () =>
        //     {
        //         
        //     }, Debug.LogError);
        // }
        
        public void PromptNotificationPermission()
        {
            // PushNotificationsManager.SharedInstance.ShowPermissionPrompt();
            Notifications.SetPushNotificationsEnabled(true, () => Debug.Log("Successfully Registered for Push Notifications"), Debug.LogError);
        }
        
        public void SendFriendRequestToStest()
        {
            string username = "stest";
            UsersQuery query = UsersQuery.Find(username);
            PagingQuery<UsersQuery> pagingQuery = new PagingQuery<UsersQuery>(query);
            Communities.GetUsers(pagingQuery, result =>
                {
                    if(result.Entries.Count > 0 && result.Entries.AnyOut(entry => entry.DisplayName == username, out User user))
                        GetSocialManager.SendFriendRequest(user.Id, () => Debug.Log("Sent Friend Request to: " + user.DisplayName), error => Debug.LogError(error.Message));
                },
                Debug.LogError
            );
        }

        public void LogNotifications()
        {
            Debug.Log("Device ID: " + GetSocialSdk.Core.GetSocial.Device.Identifier);
            Debug.Log("Is Test Device: " + GetSocialSdk.Core.GetSocial.Device.IsTestDevice);
            Notifications.ArePushNotificationsEnabled(on => Debug.Log("Push Notifications enabled: " + on), Debug.LogError);
            
            NotificationsQuery query = NotificationsQuery.WithStatuses(NotificationStatus.Unread);
            Notifications.Count(query,
                c =>
                {
                    Debug.Log("Notifications count is: " + c);
                },
                error => {
                    Debug.LogError("Failed to get notifications count, error: " + error);
                }
            );
            
            var friendsQuery = NotificationsQuery.WithStatuses(NotificationStatus.Unread).OfTypes("friend_request_received");
            Notifications.Count(friendsQuery,
                c =>
                {
                    Debug.Log("Friend Request count is: " + c);
                },
                error => {
                    Debug.LogError("Failed to get Friend Request count, error: " + error);
                }
            );
            
            PagingQuery<NotificationsQuery> pagingQuery = new PagingQuery<NotificationsQuery>(query);
            Notifications.Get(pagingQuery, result =>
                {
                    Debug.Log("Get Notifications Length: " + result.Entries.Count);
                    if (result.Entries.Count == 0)
                        return;

                    foreach (Notification notification in result.Entries)
                    {
                        Debug.Log("Type: " + notification.Type + " " + notification.Text + ", Action Button Count: " + notification.ActionButtons.Count
                                  + ", Action Count: " + notification.Action.Data.Count);

                        foreach (NotificationButton notificationActionButton in notification.ActionButtons)
                        {
                            Debug.Log("Action Title: " + notificationActionButton.Title);
                        }
                    }
                    
                }, Debug.LogError
            );
        }
    }
}
