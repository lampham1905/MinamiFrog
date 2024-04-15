#if UNITY_IOS
using System.Collections;
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    public class Vibration : MonoBehaviour
    {
#if UNITY_ANDROID
        private static AndroidJavaClass unityPlayer;

        private static AndroidJavaClass vibrationEffect;

        private static AndroidJavaObject currentActivity;

        private static AndroidJavaObject vibrator;

        private static AndroidJavaObject context;
#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern bool _HasVibrator();

    [DllImport("__Internal")]
    private static extern void _Vibrate();

    [DllImport("__Internal")]
    private static extern void _VibratePop();

    [DllImport("__Internal")]
    private static extern void _VibratePeek();

    [DllImport("__Internal")]
    private static extern void _VibrateNope();
#endif
        private static int AndroidVersion()
        {
            int versionNumber = 0;
            if (Application.platform == RuntimePlatform.Android)
            {
                string androidVersion = SystemInfo.operatingSystem;
                int sdkPos = androidVersion.IndexOf("API-");
                versionNumber = int.Parse(androidVersion.Substring(sdkPos + 4, 2));
            }
            return versionNumber;
        }

        public static bool HasVibrator()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
                string Context_VIBRATOR_SERVICE = contextClass.GetStatic<string>("VIBRATOR_SERVICE");
                AndroidJavaObject systemService = context.Call<AndroidJavaObject>("getSystemService", Context_VIBRATOR_SERVICE);
                return systemService.Call<bool>("hasVibrator");
#elif UNITY_IOS
            return _HasVibrator();
#endif
            }
            return false;
        }

        public static bool IsInitialized
        {
            get; private set;
        }

        public static void Initialize()
        {
            if (IsInitialized) return;
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                if (AndroidVersion() >= 26) vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
#endif
            }
            IsInitialized = true;
        }

        public static void Pop()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                Vibrate(50);
#elif UNITY_IOS
            _VibratePop();
#endif
            }
        }

        public static void Peek()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                Vibrate(100);
#elif UNITY_IOS
            _VibratePeek();
#endif
            }
        }

        public static void Nope()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                long[] pattern = { 0, 50, 50, 50 };
                Vibrate(pattern, -1);
#elif UNITY_IOS
            _VibrateNope();
#endif
            }
        }

        public static void Cancel()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                vibrator.Call("cancel");
#endif
            }
        }

        public static void Vibrate(long milliseconds)
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                if (AndroidVersion() >= 26)
                {
                    AndroidJavaObject createOneShot = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, -1);
                    vibrator.Call("vibrate", createOneShot);
                }
                else vibrator.Call("vibrate", milliseconds);
#else
            Handheld.Vibrate();
#endif
            }
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                if (AndroidVersion() >= 26)
                {
                    AndroidJavaObject createWaveform = vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                    vibrator.Call("vibrate", createWaveform);
                }
                else vibrator.Call("vibrate", pattern, repeat);
#else
            Handheld.Vibrate();
#endif
            }
        }
    }
}