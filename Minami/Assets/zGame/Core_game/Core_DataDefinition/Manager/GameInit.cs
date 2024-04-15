using System;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.Data;
using Lam.zGame.Core_game.Core.Utilities.Components.Audio;
using Lam.zGame.Core_game.Core.Utilities.Services.Firebase;
using UnityEngine;
using Lam;

namespace Lam.zGame.Core_game.Core_DataDefinition.Manager
{
    public class GameInit : MonoBehaviour
    {
        #region Members

        public Action<bool> firebaseInitEvent;

        private static GameInit m_Instance;
        public static GameInit Instance => m_Instance;

        private bool m_Initialized;

        [SerializeField] private GameObject m_SystemPanel;

        #endregion

        //=====================================

        #region MonoBehaviour

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            m_SystemPanel.SetActive(true);
        }

        #endregion

        //=====================================

        #region Public
        public void AddCallendars()
        {
            new System.Globalization.GregorianCalendar();
            new System.Globalization.PersianCalendar();
            new System.Globalization.UmAlQuraCalendar();
            new System.Globalization.ThaiBuddhistCalendar();
        }
        public void Init()
        {
            AddCallendars();
            if (m_Initialized)
                return;

            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            //1. Init data
            Lam.DataInGame.Instance.Init();
            //2. Init services
            RFirebaseManager.Init(FirebaseInitEvent);
#if ACTIVE_FACEBOOK
            //Init FB
            Facebook.Unity.FB.Init();
#endif
            //3. Init IAP
            var skus = new List<string>();
            //foreach (var item in BuiltinData.Instance.shopItems)
            //    if (item.IsIAP)
            //        skus.Add(item.sku);
            //foreach (var item in BuiltinData.Instance.chests)
            //    if (item.IsIAP)
            //        skus.Add(item.sku);
            //foreach (var item in BuiltinData.Instance.heroes)
            //    if (item.IsIAP)
            //        skus.Add(item.sku);


            //PurchaserManager.Instance.InitProducts(skus);
            //4. Init Sound
            HybirdAudioCollection soundManager = HybirdAudioCollection.Instance;
            //5: Init Audio Manager
            //HybirdAudioManager.Instance.EnableSFX(UnmanagedData.EnableSFX);
            //HybirdAudioManager.Instance.SetSFXVolume(UnmanagedData.SFXVolume);
            //HybirdAudioManager.Instance.EnableMusic(UnmanagedData.EnableMusic);
            //HybirdAudioManager.Instance.SetMusicVolume(UnmanagedData.MusicVolume);
            //UnmanagedData.onSettingSfxChanged += OnSettingSfxChanged;
            //UnmanagedData.onSettingMusicChanged += OnSettingMusicChanged;

            //---FIXME tat am thanh----
            //HybirdAudioManager.Instance.EnableSFX(false);
            //HybirdAudioManager.Instance.EnableMusic(false);
        }
        public void Init(Action<bool> firebaseInitEvent)
        {
            this.firebaseInitEvent = firebaseInitEvent;
            Init();
        }    
        #endregion

        //=====================================

        #region Private
        private void FirebaseInitEvent(bool status)
        {
            firebaseInitEvent?.Invoke(status);
            firebaseInitEvent = null;
        }    

        private void OnSettingMusicChanged()
        {
            HybirdAudioManager.Instance.EnableMusic(UnmanagedData.EnableMusic);
            HybirdAudioManager.Instance.SetMusicVolume(UnmanagedData.MusicVolume);
        }

        private void OnSettingSfxChanged()
        {
            HybirdAudioManager.Instance.EnableSFX(UnmanagedData.EnableSFX);
            HybirdAudioManager.Instance.SetSFXVolume(UnmanagedData.SFXVolume);
        }
        #endregion
    }
}
