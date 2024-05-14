using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lam
{
    
    [Serializable]
    public class BuildingDefinition
    {
        public int id;
        public float price;
        public string name;
        public string des;
        public int id_Shop;
        public int is_interact;
    }
    [Serializable]
    public class ShopDefinition 
    {
        public int id;
        public int type;
        public List<string> name_item;
        public List<int> amount_item;
        public List<int> price_item;
        public List<int> profit;
        public int shop_Level;
        public string des;
        public string name;
        public List<int> perfect_recipe_youth;
        public List<int> perfect_recipe_elder;
        public List<int> perfect_profit_youth;
        public List<int> perfect_profit_elder;
    }

    [Serializable]
    public class UpgradesDefinition
    {
        public int id;
        public int type_upgrade;
        public List<int> list_upgrade_1;
        public string name_upgrade_1;
        public List<int> amount_upgrade_1;
        public float cost_upgrade_1;
        public List<int> list_upgrade_2;
        public string name_upgrade_2;
        public List<int> amount_upgrade_2;
        public float cost_upgrade_2; 
    }

    [Serializable]
    public class BonusBuildingDefinition
    {
        public int id;
        public float Satisfaction;
        public List<int> Villagers;
        public int Beauty;
        public int Shop_Level;
    }
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
        public List<BuildingDefinition> ListBuildingDefinition = new List<BuildingDefinition>();
        public List<ShopDefinition> listShopDefinitions = new List<ShopDefinition>();
        public List<UpgradesDefinition> listUpgradesDefinition = new List<UpgradesDefinition>();
        public List<BonusBuildingDefinition> listBonusBuildingDefinition = new List<BonusBuildingDefinition>();
        public void Init()
        {
            LoadData();
        }

        void LoadData()
        {
            ListBuildingDefinition = JsonHelper.GetJsonList<BuildingDefinition>(File.ReadAllText(PATHFILEDATA + "Building.txt"));
            listShopDefinitions = JsonHelper.GetJsonList<ShopDefinition>(File.ReadAllText(PATHFILEDATA + "Shop.txt"));
            listUpgradesDefinition = JsonHelper.GetJsonList<UpgradesDefinition>(File.ReadAllText(PATHFILEDATA + "Upgrades.txt"));
            listBonusBuildingDefinition = JsonHelper.GetJsonList<BonusBuildingDefinition>(File.ReadAllText(PATHFILEDATA + "BonusBuilding.txt"));
        }

        public ShopDefinition GetShopDefById(int id)
        {
            for (int i = 0; i < listShopDefinitions.Count; i++)
            {
                if (listShopDefinitions[i].id == id)
                {
                    return listShopDefinitions[i];
                }
            }
            return null;
        }

        public BuildingDefinition GetBuildingById(int id)
        {
            for (int i = 0; i < ListBuildingDefinition.Count; i++)
            {
                if (ListBuildingDefinition[i].id == id)
                {
                    return ListBuildingDefinition[i];
                }
            }
            return null;
        }

        public UpgradesDefinition GetUpgradeById(int id)
        {
            for (int i = 0; i < listUpgradesDefinition.Count; i++)
            {
                if (listUpgradesDefinition[i].id == id)
                {
                    return listUpgradesDefinition[i];
                }
            }
            return null;
        }

        public BonusBuildingDefinition GetBonusById(int id)
        {
            for (int i = 0; i < listBonusBuildingDefinition.Count; i++)
            {
                if (listBonusBuildingDefinition[i].id == id)
                {
                    return listBonusBuildingDefinition[i];
                }
            }
            return null;
        }
    }
}
