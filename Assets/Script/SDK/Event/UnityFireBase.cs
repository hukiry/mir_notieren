//using Firebase;
//using Firebase.Analytics;
//using Firebase.Extensions;
using LitJson;
using UnityEngine;

namespace Hukiry.SDK
{
    class UnityFireBase
    {
        public static UnityFireBase ins { get; } = new UnityFireBase();
        
        public void InitFireBase()
        {
            //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            //    if (task.Result == DependencyStatus.Available)
            //    {
            //        LogManager.Log("Create and hold a reference to your FirebaseApp" );
            //    }
            //    else
            //    {
            //        LogManager.LogError(
            //          "Could not resolve all Firebase dependencies: " + task.Result);
            //    }
            //});
        }


        /// <summary>
        /// 事件上报
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="parameterName">数据名</param>
        /// <param name="parameterValue">数据值</param>
        public void UpEvent(string jsonParam)
        {
            //JsonData jsonData = JsonMapper.ToObject(jsonParam);
            //string stringKey1 = (string)jsonData["stringKey1"];
            //string stringKey2 = (string)(jsonData["stringKey2"] ?? string.Empty);
            //string stringKey3 = (string)(jsonData["stringKey3"] ?? string.Empty);
            //FirebaseAnalytics.LogEvent(stringKey1, stringKey2, stringKey3);
        }
    }
}
