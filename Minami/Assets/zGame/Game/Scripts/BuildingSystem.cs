using System;
using System.Collections;
using System.Collections.Generic;
using Lam;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using ntDev;
using UnityEngine;

public enum TypeBuilding
{
   house, ramen, park
}



[Serializable]
public class BuildingInfo
{
   public int id;
   public float price;
   public string name;
   public string des;
}

[Serializable]
public class BuildingRef
{
   public string name;
   public IBuilding Building;
}
public class BuildingSystem : MonoBehaviour
{
   public static BuildingSystem ins;

   private void Awake()
   {
      ins = this;
   }

   public List<IBuilding> listBuilding = new List<IBuilding>();
   public List<BuildingRef> listBuildingRef = new List<BuildingRef>();
   private Dictionary<string, IBuilding> dicBuildingRef = new Dictionary<string, IBuilding>();
   public static BuildingInfo buildingAddCurrent ;
   private int indexCurrent = 0;
   private float posXCurrent = -1f;
   private void Start()
   {
      IniBuildingRef();
      ManagerEvent.RegEvent(EventCMD.SHOWADDBUILDING, ShowAddBuilding);
      ManagerEvent.RegEvent(EventCMD.SHOWBUILDING, ShowBuilding);
   }

   void IniBuildingRef()
   {
      for (int i = 0; i < listBuildingRef.Count; i++)
      {
         dicBuildingRef.Add(listBuildingRef[i].name, listBuildingRef[i].Building);
         
      }

      foreach (var VARIABLE in dicBuildingRef)
      {
         Debug.Log(VARIABLE.Key);
      }
   }
   void ShowBuilding(object e = null)
   {
      ListData<BuildingData> data = DataInGame.Instance.BuildingGroup.listBuildingData;
      Debug.Log(DataInGame.Instance.BuildingGroup.listBuildingData.Count);
      for (int i = 0; i < data.Count; i++)
      {
         // GameObject g = Instantiate(dicBuildingRef[data[i].name].gameObject);
         //  if (g.TryGetComponent<IBuilding>(out IBuilding building))
         //  {
         //     g.transform.position = new Vector3(posXCurrent + 1, 0, 0);
         //     posXCurrent += building.size;
         //  }
      }
     

   }
   void ShowAddBuilding(object e)
   {
      int index = (int)e;
      
   }

   void HideAddBuilding()
   {
      
   }
   void BuildBuilding()
   {
      
   }
   
  
}
