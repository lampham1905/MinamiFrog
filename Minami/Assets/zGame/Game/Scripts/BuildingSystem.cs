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
public class BuildingRef
{
   public int id;
   public IBuilding Building;
}
public class BuildingSystem : MonoBehaviour
{

   

   public List<IBuilding> listBuilding = new List<IBuilding>();
   public List<BuildingRef> listBuildingRef = new List<BuildingRef>();
   public static IBuilding buildingAddCurrent ;
   public GameObject gbbuildingAddCurrent;
   public float posXCurrent = 0f;
   private List<UIUpgradeShopPiece> _mListUIUpgradeCustomize = new List<UIUpgradeShopPiece>();
   private void Awake()
   {
      ManagerEvent.RegEvent(EventCMD.SHOWADDBUILDING, ShowAddBuilding);
      ManagerEvent.RegEvent(EventCMD.SHOWBUILDING, ShowBuilding);
      ManagerEvent.RegEvent(EventCMD.BUILDBUILDING, BuildBuilding);
      ManagerEvent.RegEvent(EventCMD.HIDEADDBUILDING, HideAddBuilding);
   }
   private void Start()
   {
     
      
   }

  
   void ShowBuilding(object e)
   {
      int AmountBrick = 0;
     ListData<BuildingData> data = e as ListData<BuildingData>;
      for (int i = 0; i < data.Count; i++)
      {
         GameObject g = Instantiate(GetBuildingRefById(data[i].idBulding).gameObject);
         if (g.TryGetComponent<IBuilding>(out IBuilding building))
         {
            building.SetData(data[i]);
            listBuilding.Add(building);
            g.transform.position = new Vector3(posXCurrent - building.size, 0, 0);
            posXCurrent -= building.size;
            MoveCam();
            UIGamePlay.Instance.UIUpgradeShop.GetUIPiece(building);
            AmountBrick += building.size;
         }
      }
      ManagerEvent.RaiseEvent(EventCMD.INITPATH, listBuilding);
   }
   
   void MoveCam()
   {
      if (posXCurrent == 0)
      {
         ManagerEvent.RaiseEvent(EventCMD.MOVECAMERATOTARGET, new Vector3(-4, 5, -5));
      }
      else
      {
         ManagerEvent.RaiseEvent(EventCMD.MOVECAMERATOTARGET, new Vector3(-4 + posXCurrent, 5, -5));
      }
   }
   private IBuilding GetBuildingRefById(int id)
   {
      for (int i = 0; i < listBuildingRef.Count; i++)
      {
         if (listBuildingRef[i].id == id)
         {
            return listBuildingRef[i].Building;
         }
      }
      return null;
   }
   void ShowAddBuilding(object e)
   {
      HideAddBuilding();
      int idBuilding = (int)e;
      gbbuildingAddCurrent = Instantiate(listBuildingRef[idBuilding].Building.gameObject);
      if (gbbuildingAddCurrent.TryGetComponent<IBuilding>(out IBuilding building))
      {
         gbbuildingAddCurrent.transform.position = new Vector3(posXCurrent - building.size, 0, 0);
         MoveCam();
      }
   }

   
   void HideAddBuilding(object e = null)
   {
      if (gbbuildingAddCurrent != null)
      {
         Destroy(gbbuildingAddCurrent);
      }
   }
   void BuildBuilding(object e)
   {
      BuildingData data = (BuildingData)e;
      if (gbbuildingAddCurrent.TryGetComponent<IBuilding>(out IBuilding building))
      {
         building.SetData(data);
         listBuilding.Add(building);
         posXCurrent -= building.size;
         UIGamePlay.Instance.UIUpgradeShop.GetUIPiece(building);
         DataInGame.Instance.BuildingGroup.AddBonusStat(data);
         ManagerEvent.RaiseEvent(EventCMD.ADDPATH,building.size);
      }
      gbbuildingAddCurrent = null;
   }

   public List<IBuilding> GetListBuildingService()
   {
      List<IBuilding> res = new List<IBuilding>();
      for (int i = 0; i < listBuilding.Count; i++)
      {
         if (BuildData.Instance.GetBuildingById(listBuilding[i].idBuilding).is_interact == 1)
         {
            res.Add(listBuilding[i]);
         }
      }
      return res;
   }

   // public List<IBuilding> GetListBuildingShop()
   // {
   //    
   // }

  
  
  
}
