#if UNITY_EDITOR
#endif
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.MiscView;
using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;
using Lam.zGame.Core_game.Core.Utilities.AntiCheat;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data.Base;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Data
    {
        public class GameData : DataManager
        {
            public const bool ENCRYPT_FILE = false;
            public const bool ENCRYPT_SAVER = true;
            public static bool USE_CLOUD_SAVE = true;
            public static readonly Encryption FILE_ENRYPTION = new Encryption();

            public static GameData mInstance;
            public static GameData Instance { get { return mInstance; } }

            private CurrenciesGroup m_CurrenciesGroup;
            private MiscGroup m_MiscGroup;
            private GameConfigGroup m_GameConfigGroup;
            private OfflineTimeGroup m_OfflineTimeGroup;
            private TutorialGroup m_TutorialGroup;
            //-----------------
            public GameConfigGroup GameConfigGroup => m_GameConfigGroup;
            public CurrenciesGroup CurrenciesGroup => m_CurrenciesGroup;
            public OfflineTimeGroup OfflineTimeGroup => m_OfflineTimeGroup;
            public TutorialGroup TutorialGroup => m_TutorialGroup;

            private DataSaver m_DataSaver;
            private bool mInitialized;
            public bool Initialized => mInitialized;
            private List<RewardInfo> m_OfflineRewards;

            private void Awake()
            {
                if (mInstance == null)
                    mInstance = this;
                else if (mInstance != this)
                    Destroy(gameObject);
            }

            private void Update()
            {
                if (m_OfflineTimeGroup != null)
                {
                    m_OfflineTimeGroup.CountTime(Time.deltaTime);
                }
                if(m_GameConfigGroup != null)
                {
                    m_GameConfigGroup.LocalPlayTime += Time.unscaledDeltaTime;
                }
            }

            public override void Init()
            {
                if (mInitialized) return;

                BuiltinData.Instance.Init();

                mInitialized = true;
                m_DataSaver = DataSaverContainer.CreateSaver("main", ENCRYPT_SAVER ? FILE_ENRYPTION : null);

                m_CurrenciesGroup = AddMainDataGroup(new CurrenciesGroup(1), m_DataSaver);
                //m_MapsGroup = AddMainDataGroup(new MapsGroup(2), m_DataSaver);
                //m_TalentsGroup = AddMainDataGroup(new TalentsGroup(3), m_DataSaver);
                //m_ShopItemsGroup = AddMainDataGroup(new ShopItemsGroup(4), m_DataSaver);
                //m_EquipmentsGroup = AddMainDataGroup(new EquipmentsGroup(5), m_DataSaver);
                m_MiscGroup = AddMainDataGroup(new MiscGroup(6), m_DataSaver);
                //m_HeroesGroup = AddMainDataGroup(new HeroesGroup(7), m_DataSaver);
                m_GameConfigGroup = AddMainDataGroup(new GameConfigGroup(8), m_DataSaver);
                m_OfflineTimeGroup = AddMainDataGroup(new OfflineTimeGroup(9), m_DataSaver);
                m_TutorialGroup = AddMainDataGroup(new TutorialGroup(10), m_DataSaver);
                //m_DailyMissionGroup = AddMainDataGroup(new DailyMissionGroup(11), m_DataSaver);
                //m_DailyDungeonGroup = AddMainDataGroup(new DailyDungeonGroup(12), m_DataSaver);

                BaseInit();
            }

            public void SaveDataGame()
            {
                Save();
            }

            public void BaseInit()
            {
                base.Init();
                m_GameConfigGroup.ChangeUserState();
                //m_DailyDungeonGroup.InitData();
                if(m_GameConfigGroup.isNewDay)
                {
                    m_MiscGroup.ResetData();
                    //m_DailyMissionGroup.ResetData();
                    //m_DailyDungeonGroup.ResetData();
                }
            }    

            public static string LoadFile(string pPath, bool pEncrypt = ENCRYPT_FILE)
            {
                if (pEncrypt)
                    return LoadFile(pPath, FILE_ENRYPTION);
                else
                    return LoadFile(pPath, null);
            }

            public void ResetOfflineTime()
            {
                m_OfflineTimeGroup.ResetTime();
                m_OfflineRewards = null;
            }

            public List<RewardInfo> CalcOfflineRewards()
            {
                if (m_OfflineRewards != null)
                    return m_OfflineRewards;

                m_OfflineRewards = new List<RewardInfo>();
                //var totalStats = new TotalStats();
                //var talentMods = m_TalentsGroup.GetMods();
                //totalStats.AddMods(talentMods);

                //float offlineSeconds = m_OfflineTimeGroup.GetOfflineSeconds();

                //if (offlineSeconds > Constants.OFFLINE_MAX_SECONDS)
                //    offlineSeconds = Constants.OFFLINE_MAX_SECONDS;
                //float offlineSteps = offlineSeconds / Constants.OFFLINE_STEP_SECONDS;
                ////Debug.Log(offlineSeconds + "  -------333333333333---------   " + offlineSteps + "  ------------  " + totalStats.coinOffline);
                //if (offlineSteps >= 1 && totalStats.coinOffline > 0)
                //{
                //    int coins = Mathf.RoundToInt(totalStats.coinOffline * offlineSteps);
                //    int parts = Mathf.RoundToInt(totalStats.coinOffline / 520f * offlineSteps);

                //    //if (coins >= 1)
                //    //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.CURRENCY_GOLD, coins));

                //    if (parts >= 1)
                //    {
                //        int armorMaterial = 0;
                //        int weaponMaterial = 0;
                //        int droneMaterial = 0;
                //        int armorPartMaterial = 0;
                //        int heroMaterial = 0;
                //        while (parts > 0)
                //        {
                //            int random = Random.Range(0, 100) % 4;
                //            switch (random)
                //            {
                //                case 0: armorMaterial++; break;
                //                case 1: weaponMaterial++; break;
                //                case 2: droneMaterial++; break;
                //                case 3: armorPartMaterial++; break;
                //                case 4: heroMaterial++; break;
                //            }
                //            parts--;
                //        }

                //        //if (armorMaterial > 0)
                //        //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_ARMOR, armorMaterial));
                //        //if (weaponMaterial > 0)
                //        //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_WEAPON, weaponMaterial));
                //        //if (droneMaterial > 0)
                //        //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_DRONE, droneMaterial));
                //        //if (armorPartMaterial > 0)
                //        //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_ARMOR_PART, armorPartMaterial));
                //        //if (heroMaterial > 0)
                //        //    m_OfflineRewards.Add(new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_HERO, heroMaterial));
                //    }
                //}
                return m_OfflineRewards;
            }

            public void ResetToDefaultData()
            {
                foreach(var item in mMainGroups)
                {
                    foreach(var g in item.Value)
                    {
                        g.Reset();
                        g.PostLoad();
                    }
                }     
            }


#if UNITY_EDITOR
            [CustomEditor(typeof(GameData))]
            private class GameDataEditor : DataManagerEditor
            {
                private GameData m_Script;
                private string[] m_WsNames;
                private string[] m_PerkNames;
                private string m_WsName;
                private string m_WsPerkName;
                //private EquipmentDefinition m_SelectedWP;
                //private WeaponPerkDefinition m_SelectedPerk;
                private string m_SelectedRarity;

                private void OnEnable()
                {
                    m_Script = target as GameData;

                    //var weaponPerks = BuiltinData.Instance.weaponPerks;
                    //var weapons = BuiltinData.Instance.equipments;
                    var wsNames = new List<string>();
                    var perkNames = new List<string>();

                    //for (int i = 0; i < weapons.Count; i++)
                    //{
                    //    var w = weapons[i];
                    //    if (w.slot == IDs.EQ_SLOT_WEAPON)
                    //        wsNames.Add(w.name);
                    //}

                    //for (int i = 0; i < weaponPerks.Count; i++)
                    //{
                    //    perkNames.Add(weaponPerks[i].id.ToString());
                    //}

                    m_PerkNames = perkNames.ToArray();
                    m_WsNames = wsNames.ToArray();
                }

                public override void OnInspectorGUI()
                {
                    base.OnInspectorGUI();

                    if (Application.isPlaying)
                    {
                        EditorHelper.BoxHorizontal(() =>
                        {
                            if (EditorHelper.Button("Add 1000 Gold"))
                                m_Script.m_CurrenciesGroup.AddGold(1000);
                            if (EditorHelper.ButtonColor("Remove Gold", Color.red))
                                m_Script.m_CurrenciesGroup.SetGold(0);
                        });
                        EditorHelper.BoxHorizontal(() =>
                        {
                            if (EditorHelper.Button("Add 100 Gem"))
                                m_Script.m_CurrenciesGroup.AddGem(100);
                            if (EditorHelper.ButtonColor("Remove Gem", Color.red))
                                m_Script.m_CurrenciesGroup.SetGem(0);
                        });
                        EditorHelper.BoxHorizontal(() =>
                        {
                            if (EditorHelper.Button("Add 5 Stamina"))
                                m_Script.m_CurrenciesGroup.AddStamina(5, true);
                            if (EditorHelper.ButtonColor("Remove Stamina", Color.red))
                                m_Script.m_CurrenciesGroup.SetStamina(0);
                        });
                        EditorHelper.Seperator();
                    }
                    else
                        EditorGUILayout.HelpBox("Click play to see how it work", MessageType.Info);

                    EditorHelper.BoxVertical(() =>
                    {
                        int rarity = 1;
                        if (m_WsNames != null)
                        {
                            var selected = EditorHelper.DropdownList(m_WsName, "Weapon", m_WsNames);
                            if (selected != m_WsName)
                            {
                                m_WsName = selected;

                                //foreach (var e in BuiltinData.Instance.equipments)
                                //    if (e.name == m_WsName)
                                //    {
                                //        m_SelectedWP = e;
                                //        break;
                                //    }
                            }
                            m_SelectedRarity = EditorHelper.DropdownList(m_SelectedRarity, "Rarity", BuiltinData.RARITY_NAMES);
                            for (int i = 0; i < BuiltinData.RARITY_NAMES.Length; i++)
                            {
                                if (BuiltinData.RARITY_NAMES[i] == m_SelectedRarity)
                                {
                                    rarity = i + 1;
                                    break;
                                }
                            }

                            //    if (m_SelectedWP != null)
                            //    {
                            //        selected = EditorHelper.DropdownList(m_WsPerkName, "Perks", m_PerkNames);
                            //        if (selected != m_WsPerkName)
                            //        {
                            //            m_WsPerkName = selected;

                            //            foreach (var e in BuiltinData.Instance.weaponPerks)
                            //            {
                            //                if (e.id.ToString() == m_WsPerkName)
                            //                {
                            //                    m_SelectedPerk = e;
                            //                    break;
                            //                }
                            //            }
                            //        }

                            //        if (Application.isPlaying && EditorHelper.Button("Create Weapon"))
                            //        {
                            //            var newItem = new EquipmentItem();
                            //            newItem.BaseId = m_SelectedWP.id;
                            //            newItem.Rarity = rarity;
                            //            newItem.Level = 1;
                            //            newItem.Quantity = 1;
                            //            newItem.perkIds = new int[1] { m_SelectedPerk.id };
                            //            m_Script.EquipmentsGroup.Inventory.Insert(newItem);
                            //            m_Script.Save();
                            //            EventDispatcher.Raise(new EquipmentCraftedEvent(newItem.Id));
                            //        }
                            //    }
                        }

                    }, Color.white, true);
                }
            }
#endif
        }
    }
