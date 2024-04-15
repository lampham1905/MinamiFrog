using System;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject
{
    [System.Serializable]
    public class ModDefinition
    {
        public int mod;
        public string description;
        public bool addSign;
    }

    [System.Serializable]
    public class PlayerLevelDefinition
    {
        public int level;
        public int exp;
    }

    [CreateAssetMenu(fileName = "BuiltinData", menuName = "OTS/Create Built-in Data")]
    public class BuiltinData : UnityEngine.ScriptableObject
    {
        #region Members

        public static readonly string[] SLOT_NAMES = new string[] { "Weapon", "Helmet", "Armor", "Glover", "Boot", "Drone 1", "Drone 2" };
        public static readonly string[] RARITY_NAMES = new string[] { "Common", "Great", "Rare", "Epic", "Legendary", "Relic" };
        public static readonly string[] CURRENCY_NAMES = new string[] { "Coin", "Gem", "Stamina", "Money", "Armor Scraps", "Scrap Metals", "Weapon Scraps", "Drone Scraps" };
        public static readonly int[] MAX_LEVELS = new int[] { 20, 25, 30, 40, 50, 60 };

        private static BuiltinData m_Instance;
        public static BuiltinData Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = Resources.Load<BuiltinData>($"BuiltinData");
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }

        public bool autoInit;
        public List<ModDefinition> mods;
        //public List<MapDefinition> maps;
        //public List<StageRewardDefinition> stageRewards;
        //public List<MapDefinition> hardMaps;
        //public List<StageRewardDefinition> hardStageRewards;
        //public List<EquipmentDefinition> equipments;
        //public List<EquipmentUpgradeDefinition> weaponStats;
        //public List<EquipmentUpgradeDefinition> armorStats;
        //public List<EquipmentUpgradeDefinition> armorPartStats;
        //public List<EquipmentUpgradeDefinition> droneStats;
        //public List<EnemyDefinition> enemies;
        //public List<HeroDefinition> heroes;
        //public List<WeaponPerkDefinition> weaponPerks;
        //public List<TalentDefinition> talents;
        //public List<TalentGroupDefinition> talentGroups;
        //public Dictionary<int, List<TalentDefinition>> dictOfTalents;
        //public List<ShopItemDefinition> shopItems;
        //public List<ChestDefinition> chests;
        //public List<PlayerLevelDefinition> playerLevels;
        //public List<WeaponComparion> weaponComparions;

        //public List<HeroUpgradeDefinition> heroUpgrades;
        //public List<MilestoneDefinition> milestones;
        //public List<DailyMissionDefinition> dailyMissions;
        //public List<DailyDungeonDefinition> dailyDungeons;

        [NonSerialized]
        private bool m_Loaded;

        #endregion

        //=====================================

        #region MonoBehaviour

        public void Init()
        {
            if (autoInit)
                LoadData();
        }

        #endregion

        //=====================================

        #region Public

        //===================================== MODS

        public ModDefinition GetMod(int pId)
        {
            if (pId > 0 && pId < 100)
                return mods[pId - 1];
            return null;
        }

        ////===================================== STAGE REWARDS

        //public List<StageRewardDefinition> GetStageRewards(int pMapId, int typeListMap)
        //{
        //    List<StageRewardDefinition> listStageReward = stageRewards;
        //    switch (typeListMap)
        //    {
        //        case IDs.TYPE_LIST_MAP_HARD:
        //            listStageReward = hardStageRewards;
        //            break;
        //    }

        //    if (pMapId == 0)
        //    {
        //        return listStageReward;
        //    }
        //    else
        //    {
        //        var list = new List<StageRewardDefinition>();
        //        for (int i = 0; i < listStageReward.Count; i++)
        //        {
        //            if (listStageReward[i].mapId == pMapId)
        //                list.Add(listStageReward[i]);
        //        }
        //        return list;
        //    }
        //}

        //public StageRewardDefinition GetStageReward(int pMapId, int pStage)
        //{
        //    for (int i = 0; i < stageRewards.Count; i++)
        //    {
        //        if (stageRewards[i].mapId == pMapId && stageRewards[i].stage == pStage)
        //            return stageRewards[i];
        //    }

        //    for (int i = 0; i < hardStageRewards.Count; i++)
        //    {
        //        if (hardStageRewards[i].mapId == pMapId && hardStageRewards[i].stage == pStage)
        //            return hardStageRewards[i];
        //    }
        //    return null;
        //}

        //public StageRewardDefinition GetNextStageReward(int pMapId, int pStage, int typeListMap)
        //{
        //    switch (typeListMap)
        //    {
        //        case IDs.TYPE_LIST_MAP_HARD:
        //            for (int i = 0; i < hardStageRewards.Count; i++)
        //            {
        //                if (hardStageRewards[i].mapId == pMapId && hardStageRewards[i].stage > pStage)
        //                    return hardStageRewards[i];
        //            }
        //            break;
        //        default:
        //            for (int i = 0; i < stageRewards.Count; i++)
        //            {
        //                if (stageRewards[i].mapId == pMapId && stageRewards[i].stage > pStage)
        //                    return stageRewards[i];
        //            }
        //            break;
        //    }
        //    return null;
        //}

        ////===================================== MAPS

        //public MapDefinition GetMap(int id, int typeListMap)
        //{
        //    //return maps[id - 1];
        //    MapDefinition mapDefinition = null;
        //    List<MapDefinition> listMap = maps;
        //    switch (typeListMap)
        //    {
        //        case IDs.TYPE_LIST_MAP_HARD:
        //            listMap = hardMaps;
        //            break;
        //    }
        //    int length = listMap.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        if (listMap[i].id == id)
        //        {
        //            mapDefinition = listMap[i];
        //            break;
        //        }
        //    }
        //    return mapDefinition;
        //}

        ////===================================== EQUIPMENTS
        //// typeGetExotic: 0-all,1-no exotic, 2-only exotic
        //public EquipmentDefinition GetEquipment(int pId = 0, int typeGetExotic = IDs.TYPE_GET_EXOTIC_NOT, int slot = 0, bool pOnlyVisible = true)
        //{
        //    if (pId > 0)
        //    {
        //        return equipments[pId - 1];
        //    }
        //    else
        //    {
        //        //Random Id = 0
        //        var list = new List<EquipmentDefinition>();
        //        foreach (var item in equipments)
        //        {
        //            if ((!pOnlyVisible || pOnlyVisible == item.visible)
        //                && (slot == 0 || item.slot == slot))
        //            {
        //                bool check = false;
        //                switch (typeGetExotic)
        //                {
        //                    case IDs.TYPE_GET_EXOTIC_NOT:
        //                        if (!item.isExotic)
        //                        {
        //                            check = true;
        //                        }
        //                        break;
        //                    case IDs.TYPE_GET_EXOTIC_ONLY:
        //                        if (item.isExotic)
        //                        {
        //                            check = true;
        //                        }
        //                        break;
        //                    default:
        //                        check = true;
        //                        break;
        //                }
        //                if (check)
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //        }
        //        if (list.Count > 0)
        //        {
        //            return list[UnityEngine.Random.Range(0, list.Count)];
        //        }
        //        return null;
        //    }
        //}

        //public EquipmentDefinition GetEquipmentRandomly(int slot, bool pOnlyVisible = true)
        //{
        //    var list = new List<EquipmentDefinition>();
        //    foreach (var item in equipments)
        //        if ((!pOnlyVisible || pOnlyVisible == item.visible)
        //            && (slot == 0 || item.slot == slot))
        //            list.Add(item);
        //    return list[Random.Range(0, list.Count)];
        //}

        //public EquipmentUpgradeDefinition GetEquipmentUpgrade(int pSlot, int pLevel)
        //{
        //    if (pSlot == IDs.EQ_SLOT_WEAPON)
        //    {
        //        if (pLevel > weaponStats.Count)
        //        {
        //            pLevel = weaponStats.Count;
        //        }
        //        return weaponStats[pLevel - 1];
        //    }
        //    if (pSlot == IDs.EQ_SLOT_DRONE_1 || pSlot == IDs.EQ_SLOT_DRONE_2)
        //    {
        //        if (pLevel > droneStats.Count)
        //        {
        //            pLevel = droneStats.Count;
        //        }
        //        return droneStats[pLevel - 1];
        //    }
        //    else if (pSlot == IDs.EQ_SLOT_ARMOR)
        //    {
        //        if (pLevel > armorStats.Count)
        //        {
        //            pLevel = armorStats.Count;
        //        }
        //        return armorStats[pLevel - 1];
        //    }
        //    else
        //    {
        //        if (pLevel > armorPartStats.Count)
        //        {
        //            pLevel = armorPartStats.Count;
        //        }
        //        return armorPartStats[pLevel - 1];
        //    }
        //}

        //public List<WeaponPerkDefinition> GetRandomPerks(int pNumber, int idEquipment, int pWeaponType, bool isExotic, List<int> pIgnoreGroups = null)
        //{
        //    var selectedPerks = new List<WeaponPerkDefinition>();
        //    var selectedGroups = new List<int>();
        //    if (pIgnoreGroups != null)
        //        selectedGroups.AddRange(pIgnoreGroups);
        //    for (int i = 0; i < pNumber; i++)
        //    {
        //        var perks = GetPerks(idEquipment, pWeaponType, isExotic, selectedGroups, true);
        //        if (perks.Count > 0)
        //        {
        //            var perk = perks[Random.Range(0, perks.Count)];
        //            selectedGroups.Add(perk.group);
        //            selectedPerks.Add(perk);
        //        }
        //    }
        //    return selectedPerks;
        //}

        //public WeaponPerkDefinition GetPerk(int id)
        //{
        //    return weaponPerks[id - 1];
        //}

        //public List<WeaponPerkDefinition> GetPerks(int idEquipment, int pWeaponType, bool isExotic, List<int> groups, bool outSideGroup, bool pOnlyVisible = true)
        //{
        //    var list = new List<WeaponPerkDefinition>();
        //    for (int i = 0; i < weaponPerks.Count; i++)
        //    {
        //        if (!pOnlyVisible || pOnlyVisible == weaponPerks[i].visible)
        //        {
        //            if (weaponPerks[i].weaponType == 0 || weaponPerks[i].weaponType == pWeaponType)
        //            {
        //                bool isCheck = false;
        //                if (weaponPerks[i].idEquipment == null || weaponPerks[i].idEquipment.Length == 0)
        //                {
        //                    isCheck = true;
        //                }
        //                else
        //                {
        //                    int lengthIdEquipmentOK = weaponPerks[i].idEquipment.Length;
        //                    for (int k = 0; k < lengthIdEquipmentOK; k++)
        //                    {
        //                        if (weaponPerks[i].idEquipment[k] == idEquipment)
        //                        {
        //                            isCheck = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (isCheck)
        //                {
        //                    //exotic perk 1 luon la` group Exotic
        //                    if (isExotic)
        //                    {
        //                        isCheck = false;
        //                        //kiem tra xem co chua
        //                        if (groups.Contains(IDs.P_EXOTIC))
        //                        {
        //                            isCheck = true;
        //                        }
        //                        else
        //                        {
        //                            if (weaponPerks[i].group == IDs.P_EXOTIC)
        //                            {
        //                                list.Add(weaponPerks[i]);
        //                            }
        //                        }
        //                    }
        //                }
        //                if (isCheck)
        //                {
        //                    if (groups.Contains(weaponPerks[i].group) && !outSideGroup)
        //                    {
        //                        list.Add(weaponPerks[i]);
        //                    }
        //                    else if (!groups.Contains(weaponPerks[i].group) && outSideGroup)
        //                    {
        //                        list.Add(weaponPerks[i]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}

        ////===================================== ENEMIES

        //public EnemyDefinition GetEnemy(int id)
        //{
        //    return enemies[id - 1];
        //}

        ////===================================== HEROES

        //public HeroDefinition GetHero(int id)
        //{
        //    return heroes[id - 1];
        //}

        //public KeyValuePair<int, int> GetHeroUpgradeRequire(int level)
        //{
        //    HeroUpgradeDefinition definition = heroUpgrades[level - 1];
        //    return new KeyValuePair<int, int>(definition.coin, definition.material);
        //}

        //===================================== TALENTS

        //public TalentGroupDefinition GetTalentGroup(int pGroup)
        //{
        //    //Debug.Log(pGroup+"    talent -----  "+ talentGroups.Count);
        //    return talentGroups[pGroup - 1];
        //}

        //public Dictionary<int, List<TalentDefinition>> GetTalents()
        //{
        //    if (dictOfTalents == null)
        //    {
        //        var dict = new Dictionary<int, List<TalentDefinition>>();
        //        for (int i = 0; i < talents.Count; i++)
        //        {
        //            int group = talents[i].group;
        //            if (!dict.ContainsKey(group))
        //                dict.Add(group, new List<TalentDefinition>());
        //            dict[group].Add(talents[i]);
        //        }
        //        dictOfTalents = dict;
        //        return dict;
        //    }
        //    return dictOfTalents;
        //}

        //public List<TalentDefinition> GetTalents(int group)
        //{
        //    if (dictOfTalents == null)
        //        GetTalents();
        //    return dictOfTalents[group];
        //}

        //public ShopItemDefinition GetShopItem(int pId)
        //{
        //    return shopItems[pId - 1];
        //}

        //public ChestDefinition GetChest(int pId)
        //{
        //    return chests[pId - 1];
        //}

        //public WeaponComparion GetWeaponComparion(int pWeaponId)
        //{
        //    for (int i = 0; i < weaponComparions.Count; i++)
        //    {
        //        if (weaponComparions[i].weaponId == pWeaponId)
        //            return weaponComparions[i];
        //    }
        //    return null;
        //}

        ////=================================== DAILY DUNGEON

        //public DailyDungeonDefinition GetDailyDungeon(int dungeonId)
        //{
        //    return dailyDungeons.Find(e => e.id == dungeonId);
        //}

        #endregion

        #region Private

        private void LoadData()
        {
            if (m_Loaded && Application.isPlaying)
                return;
       
            //maps = JsonHelper.GetJsonList<MapDefinition>(GameData.LoadFile("Data/Maps"));
            //stageRewards = JsonHelper.GetJsonList<StageRewardDefinition>(GameData.LoadFile("Data/StageRewards"));

            //hardMaps = JsonHelper.GetJsonList<MapDefinition>(GameData.LoadFile("Data/HardMaps"));
            //hardStageRewards = JsonHelper.GetJsonList<StageRewardDefinition>(GameData.LoadFile("Data/HardStageRewards"));

            //equipments = JsonHelper.GetJsonList<EquipmentDefinition>(GameData.LoadFile("Data/Equipments"));
            //weaponStats = JsonHelper.GetJsonList<EquipmentUpgradeDefinition>(GameData.LoadFile("Data/WeaponStats"));
            //armorStats = JsonHelper.GetJsonList<EquipmentUpgradeDefinition>(GameData.LoadFile("Data/ArmorStats"));
            //armorPartStats = JsonHelper.GetJsonList<EquipmentUpgradeDefinition>(GameData.LoadFile("Data/ArmorPartStats"));
            //droneStats = JsonHelper.GetJsonList<EquipmentUpgradeDefinition>(GameData.LoadFile("Data/DroneStats"));
            //enemies = JsonHelper.GetJsonList<EnemyDefinition>(GameData.LoadFile("Data/Enemies"));
            //heroes = JsonHelper.GetJsonList<HeroDefinition>(GameData.LoadFile("Data/Heroes"));
            //mods = JsonHelper.GetJsonList<ModDefinition>(GameData.LoadFile("Data/Mods"));
            //weaponPerks = JsonHelper.GetJsonList<WeaponPerkDefinition>(GameData.LoadFile("Data/WeaponPerks"));
            //talents = JsonHelper.GetJsonList<TalentDefinition>(GameData.LoadFile("Data/Talents"));
            //talentGroups = JsonHelper.GetJsonList<TalentGroupDefinition>(GameData.LoadFile("Data/TalentGroups"));
            //shopItems = JsonHelper.GetJsonList<ShopItemDefinition>(GameData.LoadFile("Data/Shop"));
            //chests = JsonHelper.GetJsonList<ChestDefinition>(GameData.LoadFile("Data/Chests"));
            //var chestRewards = JsonHelper.GetJsonList<ChestReward>(GameData.LoadFile("Data/ChestRewards"));
            //foreach (var chest in chests)
            //{
            //    chest.equipmentRewards = new List<ChestReward>();
            //    foreach (var reward in chestRewards)
            //        if (chest.id == reward.chestId)
            //            chest.equipmentRewards.Add(reward);
            //}
            //playerLevels = JsonHelper.GetJsonList<PlayerLevelDefinition>(GameData.LoadFile("Data/PlayerLevels"));
            //weaponComparions = JsonHelper.GetJsonList<WeaponComparion>(GameData.LoadFile("Data/WeaponComparion"));

            //heroUpgrades = JsonHelper.GetJsonList<HeroUpgradeDefinition>(GameData.LoadFile("Data/HeroUpgrade"));
            //milestones = JsonHelper.GetJsonList<MilestoneDefinition>(GameData.LoadFile("Data/Milestone"));
            //dailyMissions = JsonHelper.GetJsonList<DailyMissionDefinition>(GameData.LoadFile("Data/DailyMission"));
            //dailyDungeons = JsonHelper.GetJsonList<DailyDungeonDefinition>(GameData.LoadFile("Data/DailyDungeon"));

            m_Loaded = true;
        }

        #endregion

        //=====================================

#if UNITY_EDITOR
        [CustomEditor(typeof(BuiltinData))]
        private class BuiltinDataEditor : Editor
        {
            private BuiltinData m_Script;

            private void OnEnable()
            {
                m_Script = target as BuiltinData;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (EditorHelper.Button("Build Data"))
                {
                    m_Script.LoadData();
                    EditorUtility.SetDirty(m_Script);
                    AssetDatabase.SaveAssets();
                }
            }

            [MenuItem("OTS/Built-in Data")]
            private static void Open()
            {
                Selection.activeObject = BuiltinData.Instance;
            }
        }
#endif
    }
}