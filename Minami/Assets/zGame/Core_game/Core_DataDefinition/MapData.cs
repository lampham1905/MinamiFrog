//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Utilities.Pattern.Data;
//using System;
//using Utilities.Common;

//    [Serializable]
//    public class MapDefinition
//    {
//        public int id;
//        public int typeListMap;
//        public string name;
//        public string description;
//        public int totalStages;
//        public int totalGold;
//        public int totalParts;
//        public float heroParts;
//        public int totalEquipment;
//        public int expPlay;
//        public int expPerStage;
//    }

//    [Serializable]
//    public class StageRewardDefinition
//    {
//        public int mapId;
//        public int stage;
//        public int[] rewardTypes;
//        public int[] rewardIds;
//        public int[] rewardValues;
//        public int[] rewardRaritys;
//        public List<RewardInfo> GetRewards()
//        {
//            var list = new List<RewardInfo>();
//            for (int i = 0; i < rewardTypes.Length; i++)
//            {
//                int rarity = 0;
//                if (rewardRaritys != null && rewardRaritys.Length > 0)
//                {
//                    rarity = rewardRaritys[i];
//                }
//                list.Add(new RewardInfo(rewardTypes[i], rewardIds[i], rewardValues[i], rewardRaritys[i]));
//            }
//            return list;
//        }
//    }

//    public class MapData : DataGroup
//    {
//        private BoolData m_Unlocked;
//        private BoolData m_Buzzed;
//        private IntegerData m_HighestStage; //NOTE: stage here is not index, therefore it start from 1
//        private ListData<int> m_ClaimedStageRewards;

//        private int m_TotalStages;
//        private int m_TotalGold;
//        private float m_TotalPats;
//        private float m_heroParts;
//        private float m_TotalEquipment;
//        private int m_expPlay;
//        private int m_expPerStage;
//        private int m_typeListMap;
//        public bool Unlocked => m_Unlocked.Value;
//        public int HighestStage => m_HighestStage.Value;
//        public bool Completed => m_HighestStage.Value >= m_TotalStages;
//        public int TotalStages => m_TotalStages;
//        public int TotalGold => m_TotalGold;
//        public float TotalPats => m_TotalPats;
//        public float HeroParts => m_heroParts;
//        public float TotalEquipment => m_TotalEquipment;
//        public int expPlay => m_expPlay;
//        public int expPerStage => m_expPerStage;
//        public bool Buzzed
//        {
//            get { return m_Buzzed.Value; }
//            set { m_Buzzed.Value = value; }
//        }

//        public MapData(MapDefinition pBaseData) : base(pBaseData.id)
//        {
//            m_TotalStages = pBaseData.totalStages;
//            m_TotalGold = pBaseData.totalGold;
//            m_heroParts = pBaseData.heroParts;
//            m_TotalPats = pBaseData.totalParts;
//            m_TotalEquipment = pBaseData.totalEquipment;
//            m_expPlay = pBaseData.expPlay;
//            m_expPerStage = pBaseData.expPerStage;
//            m_typeListMap = pBaseData.typeListMap;

//            m_Unlocked = AddData(new BoolData(1, pBaseData.id == 1));
//            m_Buzzed = AddData(new BoolData(2));
//            m_HighestStage = AddData(new IntegerData(3));
//            m_ClaimedStageRewards = AddData(new ListData<int>(4));
//        }

//        public MapDefinition GetDefinition()
//        {
//            return BuiltinData.Instance.GetMap(Id, m_typeListMap);
//        }

//        public Sprite GetIcon()
//        {
//            return GeneralAssets.Instance.mapIcons[Id - 1];
//        }

//        public bool Unlock(bool pValue = true)
//        {
//            if (m_Unlocked.Value == pValue)
//            {
//                return false;
//            }
//            if (pValue)
//            {
//                m_Unlocked.Value = pValue;
//                m_Buzzed.Value = true;
//                EventDispatcher.Raise(new MapUnlockedEvent(Id));
//            }
//            else if (Id != 1)
//            {
//                m_Unlocked.Value = pValue;
//            }
//            return true;
//        }

//        public void SetHighestStage(int pValue)
//        {
//            //UnityEngine.Debug.Log($" SetHighestStage {pValue}");
//            //if (m_HighestStage.Value == pValue)
//            //    return;
//            if (pValue > m_HighestStage.Value)
//            {
//                m_HighestStage.Value = pValue;
//            }
//            if (m_HighestStage.Value >= m_TotalStages)
//            {
//                var nextMap = GameData.Instance.MapsGroup.GetMap(Id + 1);
//                if (nextMap != null)
//                {
//                    nextMap.Unlock();
//                    GameData.Instance.HeroesGroup.CheckCanUnlockHero();
//                }
//                EventDispatcher.Raise(new MapCompletedEvent(Id));
//            }
//        }

//        public bool IsClaimed(int stage)
//        {
//            return m_ClaimedStageRewards.Contain(stage);
//        }

//        public List<RewardInfo> ClaimStageRewards(int pStage)
//        {
//            if (!CanClaimStageReward(pStage))
//                return null;

//            var stageReward = BuiltinData.Instance.GetStageReward(Id, pStage);
//            return ClaimStageRewards(stageReward);
//        }

//        public List<RewardInfo> ClaimStageRewards(StageRewardDefinition stageReward)
//        {
//            if (!CanClaimStageReward(stageReward.stage))
//                return null;

//            var rewards = new List<RewardInfo>();
//            for (int i = 0; i < stageReward.rewardIds.Length; i++)
//            {
//                int type = stageReward.rewardTypes[i];
//                int id = stageReward.rewardIds[i];
//                int value = stageReward.rewardValues[i];
//                int rarity = 0;
//                if (stageReward.rewardRaritys != null && stageReward.rewardRaritys.Length > 0)
//                {
//                    rarity = stageReward.rewardRaritys[i];
//                }
//                rewards.Add(new RewardInfo(type, id, value, rarity));
//            }
//            rewards = GeneralAPI.ClaimRewards(rewards);
//            m_ClaimedStageRewards.Add(stageReward.stage);
//            GameData.Instance.Save();
//            return rewards;
//        }

//        public StageRewardDefinition GetNextStageReward()
//        {
//            if (!m_ClaimedStageRewards.Contain(m_HighestStage.Value))
//                return BuiltinData.Instance.GetNextStageReward(Id, m_HighestStage.Value, m_typeListMap);
//            else
//                return BuiltinData.Instance.GetNextStageReward(Id, m_HighestStage.Value + 1, m_typeListMap);
//        }

//        public bool HasClaimableStageReward()
//        {
//            var stageRewards = BuiltinData.Instance.GetStageRewards(Id, m_typeListMap);
//            for (int i = 0; i < stageRewards.Count; i++)
//            {
//                int stage = stageRewards[i].stage;
//                if (m_HighestStage.Value >= stage && !m_ClaimedStageRewards.Contain(stage))
//                    return true;
//            }
//            return false;
//        }

//        public bool CanClaimStageReward(int stage)
//        {
//            return m_HighestStage.Value >= stage && !m_ClaimedStageRewards.Contain(stage);
//        }
//    }
