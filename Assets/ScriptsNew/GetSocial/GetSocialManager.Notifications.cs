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
        public static void SendFriendRequest(string userId, Action onSuccess, Action<GetSocialError> onError)
        {
            Dictionary<string, string> actionData = new Dictionary<string, string>() {
                { GetSocialActionKeys.AddFriend.UserId, localUser.Id }
            };
            GetSocialAction action = GetSocialAction.Create(GetSocialActionType.AddFriend, actionData);

            NotificationContent content = NotificationContent.CreateWithTemplate("friend_request_received").WithAction(action);
            content.AddActionButtons(new[]{
                NotificationButton.Create("Accept", NotificationButton.ConsumeAction),
                NotificationButton.Create("Decline", NotificationButton.IgnoreAction)
            });
            
            Notifications.Send(content, SendNotificationTarget.Users(UserIdList.Create(userId)), () => AddToCustomList("SentFriendRequests", userId, false, onSuccess, onError), onError);

            // there's also problems with this, in that the notifications system is paginated, so if we had a "Pending friend requests" section it wouldn't show all of them if you have a lot of notifications
            // would need to use an actual server to do this. i guess just use the friends system we currently have.
            // Actually, i think this would work because you are able to get the count for notifications without paginating and separate then by type or action 
            var query = NotificationsQuery.WithStatuses(NotificationStatus.Unread).OfTypes("friend_request_received");
            Notifications.Count(query,
                (count) =>
                {
                    Debug.Log("Notifications count is: " + count);
                },
                (error) => {
                    Debug.Log("Failed to get notifications count, error: " + error);
                });
        }
    }
}
