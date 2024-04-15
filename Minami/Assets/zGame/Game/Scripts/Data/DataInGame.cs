using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.AntiCheat;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data.Base;
using ntDev;
using UnityEngine;

namespace Lam
{
    public class DataInGame : DataManager
    {
        public const bool ENCRYPT_FILE = false;
        public const bool ENCRYPT_SAVER = true;
        public static bool USE_CLOUD_SAVE = true;
        public static readonly Encryption FILE_ENRYPTION = new Encryption();
        public static DataInGame mInstance;
        public static DataInGame Instance { get { return mInstance; } }
        private bool mInitialized;
        public bool Initialized => mInitialized;

        private BuildingGroup m_BuildingGroup;
        public BuildingGroup BuildingGroup => m_BuildingGroup;
        private DataSaver m_DataSaver;
        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }
        public static string LoadFile(string pPath, bool pEncrypt = ENCRYPT_FILE)
        {
            if (pEncrypt)
                return LoadFile(pPath, FILE_ENRYPTION);
            else
                return LoadFile(pPath, null);
        }
        
        public void Init()
        {
            if (mInitialized) return;
            mInitialized = true;
            m_DataSaver = DataSaverContainer.CreateSaver("main", ENCRYPT_SAVER ? FILE_ENRYPTION : null); 
            BuildData.Instance.Init();
            //------------------------------------------------------------------
            m_BuildingGroup = AddMainDataGroup(new BuildingGroup(1), m_DataSaver);

            // for (int i = 0; i < dataDefinition.coumt; i++)
            // {
            //     
            // }
            
            
            //------------------------
            ManagerEvent.RaiseEvent(EventCMD.SHOWBUILDING);
            Debug.Log("cc");
        }
        
    }
}
