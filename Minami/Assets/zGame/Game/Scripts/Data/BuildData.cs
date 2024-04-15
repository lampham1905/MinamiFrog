using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEngine;

namespace Lam
{
    public class BuildData : MonoBehaviour
    {
        private static BuildData m_Instance;
        public static BuildData Instance { get { return m_Instance; } }
         private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        private const string PATHFILEDATA = "Assets/Resources/Data/";
        public List<BuildingInfo> ListBuildingInfos = new List<BuildingInfo>();
        public List<BuildingData> listBuildingInit = new List<BuildingData>();
        public void Init()
        {
            LoadData();
        }

        void LoadData()
        {
            ListBuildingInfos = JsonHelper.GetJsonList<BuildingInfo>(File.ReadAllText(PATHFILEDATA + "DataBuiding.txt"));
            listBuildingInit = JsonHelper.GetJsonList<BuildingData>(File.ReadAllText(PATHFILEDATA + "DataBuildingInit1.txt"));
        }
    }
}
