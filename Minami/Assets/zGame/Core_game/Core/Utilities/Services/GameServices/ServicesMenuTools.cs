#if UNITY_EDITOR
using Lam.zGame.Core_game.Core.Utilities.Services.Firebase;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Services.GameServices
{
    public class ServicesMenuTools : Editor
    {
        [MenuItem("RUtilities/Services/Add Firebase Manager")]
        private static void AddFirebaseManager()
        {
            var manager = FindObjectOfType<RFirebaseManager>();
            if (manager == null)
            {
                var obj = new GameObject("RFirebaseManager");
                obj.AddComponent<RFirebaseManager>();
            }
        }

        [MenuItem("RUtilities/Services/Add Game Services")]
        private static void AddGameServices()
        {
            var manager = FindObjectOfType<GameServices>();
            if (manager == null)
            {
                var obj = new GameObject("GameServices");
                obj.AddComponent<GameServices>();
            }
        }

        //[MenuItem("RUtilities/Services/Add Ads Manager")]
        //private static void AddAdsManager()
        //{
        //    var manager = FindObjectOfType<Ads.AdsManager>();
        //    if (manager == null)
        //    {
        //        var obj = new GameObject("AdsManager");
        //        obj.AddComponent<Ads.AdsManager>();
        //    }
        //}

        //[MenuItem("RUtilities/Services/Add IAP Helper")]
        //private static void AddIAPHelper()
        //{
        //    var manager = FindObjectOfType<PaymentHelper>();
        //    if (manager == null)
        //    {
        //        var obj = new GameObject("IAPHelper");
        //        obj.AddComponent<PaymentHelper>();
        //    }
        //}

        [MenuItem("RUtilities/Services/Add Local Notification Helper")]
        private static void AddLocalNotificationHelper()
        {
            var manager = FindObjectOfType<Notification.LocalNotificationHelper>();
            if (manager == null)
            {
                var obj = new GameObject("LocalNotificationHelper");
                obj.AddComponent<Notification.LocalNotificationHelper>();
            }
        }
    }
}
#endif