using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using UnityEngine;

namespace Lam
{
    public class BuildingData : DataGroup
    {
        public IntegerData id;
        public StringData name;

        public BuildingData(int pId) : base(pId)
        {
            
        }
    }
    public class BuildingGroup : DataGroup
    {
        private ListData<BuildingData> m_listBuildingData;


        public ListData<BuildingData> listBuildingData => m_listBuildingData;

        public BuildingGroup(int pId) : base(pId)
        {
            Debug.Log("BuildData.Instance.listBuildingInit : "+BuildData.Instance.listBuildingInit.Count);
            m_listBuildingData = AddData(new ListData<BuildingData>(1));
            foreach (BuildingData v in BuildData.Instance.listBuildingInit)
            {
                m_listBuildingData.Add(v);
            }
            Debug.Log("BuildData.Instance.listBuildingInit : "+m_listBuildingData.Count);
        }
    }
}
