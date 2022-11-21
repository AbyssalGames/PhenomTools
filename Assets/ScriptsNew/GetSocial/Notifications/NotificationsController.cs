using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class NotificationsController : PhenomTools.Singleton<NotificationsController>
    {
        [SerializeField]
        private NotificationsView notificationsCenterViewPrefab = null;

        public static NotificationsView OpenNotificationsCenterView()
        {
            NotificationsView notificationsCenterView = Instantiate(Instance.notificationsCenterViewPrefab, Instance.transform);
            notificationsCenterView.Initialize();
            return notificationsCenterView;
        }
    }
}
