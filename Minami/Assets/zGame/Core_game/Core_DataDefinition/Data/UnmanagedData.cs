using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Data
{
    public class UnmanagedData
    {
        public static Action onSettingSfxChanged;
        public static Action onSettingMusicChanged;
        public static Action onSettingVibrateChanged;
        public static Action onSettingCloudSaveChanged;

        #region Gift spaghetti code.
        public static void SetGiftcodeUsed(string giftCode)
        {
            string jsonData = PlayerPrefs.GetString("GiftCode", string.Empty);
            List<string> listGiftCode = StringToList(jsonData);
            if (listGiftCode.Contains(giftCode)) return;
            listGiftCode.Add(giftCode);
            PlayerPrefs.SetString("GiftCode", ListToString(listGiftCode));
        }
        public static List<string> GetListGiftCode()
        {
            string jsonData = PlayerPrefs.GetString("GiftCode", string.Empty);
            return StringToList(jsonData);
        }
        private static List<string> StringToList(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? new List<string>() : new List<string>(json.Split(','));
        }
        private static string ListToString(List<string> listData)
        {
            string result = listData[0];
            if (listData.Count > 1)
                for (int i = 1; i < listData.Count; i++)
                    result += "," + listData[i];
            return result;
        }
        #endregion

        public static bool EnableSFX
        {
            set
            {
                PlayerPrefs.SetInt("EnableSFX", value ? 1 : 0);
                onSettingSfxChanged?.Invoke();
            }
            get { return PlayerPrefs.GetInt("EnableSFX", 1) == 1; }
        }
        public static bool EnableMusic
        {
            set
            {
                PlayerPrefs.SetInt("EnableMusic", value ? 1 : 0);
                onSettingMusicChanged?.Invoke();
            }
            get { return PlayerPrefs.GetInt("EnableMusic", 1) == 1; }
        }
        public static bool EnableVibrate
        {
            set
            {
                PlayerPrefs.SetInt("EnableVibrate", value ? 1 : 0);
                onSettingVibrateChanged?.Invoke();
            }
            get { return PlayerPrefs.GetInt("EnableVibrate", 1) == 1; }
        }
        public static float SFXVolume
        {
            get { return PlayerPrefs.GetFloat("SFXVolume", 1); }
            set
            {
                PlayerPrefs.SetFloat("SFXVolume", value);
                onSettingSfxChanged?.Invoke();
            }
        }
        public static float MusicVolume
        {
            get { return PlayerPrefs.GetFloat("MusicVolume", 1); }
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
                onSettingMusicChanged?.Invoke();
            }
        }
        public static bool EnableCloudSave
        {
            set
            {
                PlayerPrefs.SetInt("EnableSFX", value ? 1 : 0);
                onSettingCloudSaveChanged?.Invoke();
            }
            get { return PlayerPrefs.GetInt("EnableSFX", 1) == 1; }
        }
        public static int FullReviveNotification
        {
            get { return PlayerPrefs.GetInt("FullReviveNotification"); }
            set { PlayerPrefs.SetInt("FullReviveNotification", value); }
        }
        public static int NewDayNotification
        {
            get { return PlayerPrefs.GetInt("NewDayNotification"); }
            set { PlayerPrefs.SetInt("NewDayNotification", value); }
        }
        public static DateTime LastTimeSendNotification
        {
            get
            {
                string val = PlayerPrefs.GetString("LastTimeSendNotification");
                DateTime time = DateTime.MinValue;
                DateTime.TryParse(val, out time);
                return time;
            }
            set { PlayerPrefs.SetString("LastTimeSendNotification", value.ToString()); }
        }
        public static int DailyNotification1
        {
            get { return PlayerPrefs.GetInt("DailyNotification1"); }
            set { PlayerPrefs.SetInt("DailyNotification1", value); }
        }
        public static int DailyNotification2
        {
            get { return PlayerPrefs.GetInt("DailyNotification2"); }
            set { PlayerPrefs.SetInt("DailyNotification2", value); }
        }
        public static bool TrackedNoLifeLeft
        {
            set { PlayerPrefs.SetInt("TrackedNoLifeLeft", value ? 1 : 0); }
            get { return PlayerPrefs.GetInt("TrackedNoLifeLeft", 1) == 1; }
        }
        public static int UpgradeBuildingNotification
        {
            get { return PlayerPrefs.GetInt("UpgradeBuildingNotification"); }
            set { PlayerPrefs.SetInt("UpgradeBuildingNotification", value); }
        }
    }
}