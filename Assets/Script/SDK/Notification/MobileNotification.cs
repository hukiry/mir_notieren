using System;
namespace Hukiry.SDK
{
    public class MobileNotification
    {
//#if UNITY_ANDROID
//        static Unity.Notifications.Android.AndroidNotificationChannel? channel = null;
//#endif
        public static void StartNotification(string Title, string Body, long timestemp, string Identifier = "CategoryIdentifier1")
        {
//            DateTime date = DateTime.Now.AddSeconds(timestemp);
//#if UNITY_ANDROID
//            if (!channel.HasValue)
//            {
//                channel = new Unity.Notifications.Android.AndroidNotificationChannel()
//                {
//                    Id = "chanel1",
//                    Name = "game chanel",
//                    Description = "game description",
//                    Importance = Unity.Notifications.Android.Importance.Default,
//                    CanBypassDnd = false,
//                    CanShowBadge = true,
//                    EnableLights = false,
//                    EnableVibration = true,
//                    LockScreenVisibility = Unity.Notifications.Android.LockScreenVisibility.Public,
//                    VibrationPattern = null,
//                };
//                Unity.Notifications.Android.AndroidNotificationCenter.RegisterNotificationChannel(channel.Value);
//            }
//            Unity.Notifications.Android.AndroidNotification notification = new Unity.Notifications.Android.AndroidNotification();
//            notification.Title = Title;
//            notification.Text = Body;
//            notification.Group = Identifier;
//            notification.FireTime = date;
//            notification.LargeIcon = "app_icon";
//            notification.SmallIcon = "app_icon";
         
//            Unity.Notifications.Android.AndroidNotificationCenter.SendNotification(notification, Identifier);
//#elif UNITY_IOS
//            Unity.Notifications.iOS.iOSNotification notification = new Unity.Notifications.iOS.iOSNotification();
//            notification.Title = Title;
//            notification.Body = Body;
//            notification.CategoryIdentifier = Identifier;
//            notification.ForegroundPresentationOption = Unity.Notifications.iOS.PresentationOption.Sound;
//            notification.Trigger = new Unity.Notifications.iOS.iOSNotificationCalendarTrigger
//            {
//                Year = date.Year,
//                Month = date.Month,
//                Day = date.Day,
//                Hour = date.Hour,
//                Minute = date.Minute,
//                Second = date.Second,
//            };
//            notification.Badge = 1;
//            //间隔推送触发的时间=测试ok
//            //notification.Trigger = new Unity.Notifications.iOS.iOSNotificationTimeIntervalTrigger
//            //{
//            //    TimeInterval = TimeSpan.FromSeconds(timestemp),
//            //    Repeats = true,
//            //};
//            Unity.Notifications.iOS.iOSNotificationCenter.ScheduleNotification(notification);
//#endif
        }

        public static void CancelNotifications()
        {
//#if UNITY_ANDROID
//            Unity.Notifications.Android.AndroidNotificationCenter.CancelAllNotifications();
//#elif UNITY_IOS
//            Unity.Notifications.iOS.iOSNotificationCenter.RemoveAllScheduledNotifications();
//#endif
        }

        /// <summary>
        /// 获取时间戳 秒
        /// </summary>
        public static long GetTimeSecond(int addtimeseconds)
        {
            DateTime unixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            long timeStamp = (DateTime.Now.ToLocalTime() - unixStartTime).Ticks / TimeSpan.TicksPerSecond;
            return timeStamp + addtimeseconds;
        }
    }
}
