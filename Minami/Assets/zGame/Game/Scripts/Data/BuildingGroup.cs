using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lam
{
    [Serializable]
    public class BuildingData
    {
        public int id;
        public int idBulding;
        public bool isUpgrade;
        public int typeUpgrade;
        public int idShop;
        public ShopData shopData;
        public void SetInfo(BuildingDefinition building)
        {
            this.idBulding = building.id;
            this.isUpgrade = false;
            this.typeUpgrade = 0;
            this.idShop = building.id_Shop;
            if (building.id_Shop != 1)
            {
                shopData = new ShopData();
                this.shopData.SetInfo(BuildData.Instance.GetShopDefById(building.id_Shop));
            }
        }
    }
    [Serializable]
    public class ShopData 
    {
        public List<int> amount_item;
        public List<int> price_item;
        public List<int> profit;
        public List<int> item_selected;
        public List<int> perfect_recipe_youth;
        public List<int> perfect_recipe_elder;
        public List<int> perfect_profit_youth;
        public List<int> perfect_profit_elder;
        public int shopLevel;
        public string name;
        public void SetInfo(ShopDefinition shop)
        {
            this.amount_item = shop.amount_item;
            this.price_item = shop.price_item;
            this.profit = shop.profit;
            this.shopLevel = shop.shop_Level;
            this.name = shop.name;
            this.item_selected = new List<int>();
            if (shop.type == 1)
            {
                // random recipe
                this.perfect_recipe_youth = LogicAPI.GetRandomPerfectRecipeShop1(shop.amount_item, shop.perfect_recipe_youth);
                this.perfect_recipe_elder = LogicAPI.GetRandomPerfectRecipeShop1(shop.amount_item, shop.perfect_recipe_elder);
                // random profit
                this.perfect_profit_youth = LogicAPI.GetRandomPerfectProfitShop1(shop.perfect_profit_youth);
                this.perfect_profit_elder = LogicAPI.GetRandomPerfectProfitShop1(shop.perfect_profit_elder);
            }
            else
            {
                // random recipe
                this.perfect_recipe_youth = LogicAPI.GetRandomPerfectRecipeShop2();
                this.perfect_recipe_elder = LogicAPI.GetRandomPerfectRecipeShop2();
                // random profit
                this.perfect_profit_youth = LogicAPI.GetRandomPerfectProfitShop2(this.perfect_recipe_youth, shop.perfect_profit_youth);
                this.perfect_profit_elder = LogicAPI.GetRandomPerfectProfitShop2(this.perfect_recipe_elder, shop.perfect_profit_elder);
            }
        }
        
    }

   
    public class BuildingGroup : DataGroup
    {
        protected ListData<BuildingData> m_listBuildingData;
        protected ListData<int> mDeletedIds;
        public ListData<BuildingData> listBuildingData => m_listBuildingData;

        public IntegerData a;
        public BuildingGroup(int pId) : base(pId)
        {
            m_listBuildingData = AddData(new ListData<BuildingData>(0));
            mDeletedIds = AddData(new ListData<int>(1));
            
        }

        public void  AddDataBuilding(BuildingDefinition building)
        {   
            int index = m_listBuildingData.Count;
            if (mDeletedIds.Count > 0)
            {
                index = mDeletedIds[0];
                mDeletedIds.RemoveAt(0);
            }
            BuildingData data = new BuildingData();
            data.id = index;
            data.SetInfo(building);
            m_listBuildingData.Add(data);
            DataInGame.Instance.Save();
        }

        public BuildingData GetBulidngById(int id)
        {
            for (int i = 0; i < listBuildingData.Count; i++)
            {
                if (listBuildingData[i].id == id)
                {
                    return listBuildingData[i];
                }
            }
            return null;
        }
        
        public BuildingData GetLastBuilding()
        {
            return listBuildingData[listBuildingData.Count - 1];
        }

        public void ChangeDataShop(BuildingData buildingData)
        {
            GetBulidngById(buildingData.id).shopData = buildingData.shopData;
            DataInGame.Instance.Save();
        }

        public void SaveDataBulding()
        {
            listBuildingData.MarkChange();
            DataInGame.Instance.Save();
        }

        public int GetAmountBuilding()
        {
            return listBuildingData.Count;
        }
        public void AddBonusStat(BuildingData data)
        {
            BonusBuildingDefinition bonusBuildingDefinition = BuildData.Instance.GetBonusById(data.idBulding);
            if (bonusBuildingDefinition.Beauty != -1)
            {
                StatGame.BEAUTY += bonusBuildingDefinition.Beauty;
            }
            if (bonusBuildingDefinition.Satisfaction != -1)
            {
                StatGame.SATISFACTION += bonusBuildingDefinition.Satisfaction;
            }

            int amountVillager = 0;
            for (int i = 0; i < bonusBuildingDefinition.Villagers.Count; i++)
            {
                amountVillager += bonusBuildingDefinition.Villagers[i];
            }
            StatGame.VILLAGERS += amountVillager;
            if (bonusBuildingDefinition.Villagers[0] != 0)
            {
                StatGame.YOUTH += bonusBuildingDefinition.Villagers[0];
            }
            if (bonusBuildingDefinition.Villagers[1] != 0)
            {
                StatGame.ELDER += bonusBuildingDefinition.Villagers[1];
            }
            if (bonusBuildingDefinition.Villagers[2] != 0)
            {
                StatGame.GOLBIN += bonusBuildingDefinition.Villagers[2];
            }
        }
    }
}
