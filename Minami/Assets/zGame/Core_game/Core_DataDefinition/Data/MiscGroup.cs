using System;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.Misc;
using Lam.zGame.Core_game.Core_DataDefinition.MiscView;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using Debug = Lam.zGame.Core_game.Core.Utilities.Common.Debug.Debug;

    namespace Lam.zGame.Core_game.Core_DataDefinition.Data
    {
        public class MiscGroup : DataGroup
        {
            private const int SECONDS_PER_DAY = 24 * 60 * 60;
            private const int FORCE_ADS_COOLDOWN = 120;

            public const int DAILY_REWARD_GEM = 100;

            private IntegerData m_PlayerLevel;
            private IntegerData m_Exp;
            private BoolData m_Buzzed;
            private TimerTask m_DailyTimer;
            private IntegerData m_CheckinCount;
            private DateTimeData m_LastTimeAds;
            private IntegerData m_CountWachedAds;
            private IntegerData m_GemBonusPerLevel;
            private BoolData m_ActiveGetDailyReward;
            private IntegerData m_chestGemOpened;

            public bool newDayBuzz = true;
            public long offlineSeconds;
            public int PlayerLevel => m_PlayerLevel.Value;
            public long RemainingTimeOfDay => m_DailyTimer.RemainSeconds;
            public int GemBonusPerLevel => m_GemBonusPerLevel.Value;
            public bool ActiveGetDailyReward => m_ActiveGetDailyReward.Value;
            public int ChestGemOpened => m_chestGemOpened.Value;

            public MiscGroup(int pId) : base(pId)
            {
                m_PlayerLevel = AddData(new IntegerData(1, 1));
                m_Exp = AddData(new IntegerData(2));
                m_Buzzed = AddData(new BoolData(3));
                m_DailyTimer = AddData(new TimerTask(4, false));
                m_CheckinCount = AddData(new IntegerData(5));
                m_LastTimeAds = AddData(new DateTimeData(6));
                m_CountWachedAds = AddData(new IntegerData(7));
                m_GemBonusPerLevel = AddData(new IntegerData(8));
                m_ActiveGetDailyReward = AddData(new BoolData(9));
                m_chestGemOpened = AddData(new IntegerData(10, -1));
            }

            public bool IsMaxLevel()
            {
                return false;
                //return m_PlayerLevel.Value >= BuiltinData.Instance.playerLevels.Count;
            }

            public int GetExpRequiredToLevelUp()
            {
                //var levels = BuiltinData.Instance.playerLevels;
                //if (m_PlayerLevel.Value >= BuiltinData.Instance.playerLevels.Count)
                //    return 0;

                //int totalExp = 0;
                //for (int i = 0; i < levels.Count; i++)
                //{
                //    totalExp += levels[i].exp;
                //    if (totalExp > m_Exp.Value)
                //        return totalExp - m_Exp.Value;
                //}
                return 0;
            }

            public void AddExp(int pExp)
            {
                if (pExp <= 0)
                    return;

                //var levels = BuiltinData.Instance.playerLevels;
                //if (m_PlayerLevel.Value >= BuiltinData.Instance.playerLevels.Count)
                //    return;

                //m_Exp.Value += pExp;

                //int totalExp = 0;
                //int reachedLevel = 1;
                //for (int lvl = 1; lvl <= levels.Count; lvl++)
                //{
                //    totalExp += levels[lvl - 1].exp;
                //    if (totalExp <= m_Exp.Value)
                //        reachedLevel = lvl + 1;
                //    else
                //        break;
                //}

                EventDispatcher.Raise(new PlayerExpChangedEvent());

                //if (reachedLevel != m_PlayerLevel.Value)
                //{
                //    m_GemBonusPerLevel.Value += 1 * (reachedLevel - m_PlayerLevel.Value);
                //    //m_GemBonusPerLevel.Value += Constants.GEM_PER_LEVEL * (reachedLevel - m_PlayerLevel.Value);
                //    m_PlayerLevel.Value = reachedLevel;
                //    m_Buzzed.Value = true;

                //    EventDispatcher.Raise(new PlayerLevelChangedEvent());
                //}

                GameData.Instance.Save();
            }

            public float GetNextLevelProgress()
            {
                return 0;
                //var levels = BuiltinData.Instance.playerLevels;
                //if (m_PlayerLevel.Value >= BuiltinData.Instance.playerLevels.Count)
                //    return 0;

                //int totalExpToCurLevel = 0;
                //int totalExpToNextLevel = 0;
                //for (int lvl = 1; lvl <= levels.Count; lvl++)
                //{
                //    if (lvl > 1)
                //    {
                //        if (lvl <= m_PlayerLevel.Value)
                //            totalExpToCurLevel += levels[lvl - 2].exp;
                //    }
                //    if (lvl <= m_PlayerLevel.Value)
                //        totalExpToNextLevel += levels[lvl - 1].exp;

                //    if (lvl == m_PlayerLevel.Value)
                //        break;
                //}

                ////Debug.Log($" ---process : {m_Exp.Value}   {totalExpToCurLevel}   {totalExpToNextLevel}");
                //return (m_Exp.Value - totalExpToCurLevel) * 1f / (totalExpToNextLevel - totalExpToCurLevel);
            }

            public void ResetData()
            {
                m_chestGemOpened.Value = -1;
            }
            public void UpdateChestGemOpened()
            {
                m_chestGemOpened.Value += 1;
            }
            public bool CanOpenChestGem()
            {
                return m_chestGemOpened.Value < 5;
            }

            /// <summary>
            /// Call this function when game already start
            /// </summary>
            public void Checkin()
            {
                if (!m_DailyTimer.IsRunning)
                    StartNewDayClock(SECONDS_PER_DAY);

                m_DailyTimer.SetOnComplete((task, remainSeconds) =>
                {
                    long modSeconds = 0;
                    int passedSteps = 1 + TimeHelper.CalcTimeStepsPassed(-remainSeconds, SECONDS_PER_DAY, out modSeconds);
                    StartNewDayClock(SECONDS_PER_DAY - modSeconds);

                    offlineSeconds = remainSeconds;
                });

                UnityEngine.Debug.Log("The next day is comming in " + m_DailyTimer.RemainSeconds + " seconds");
            }

            public void StartNewDayClock(long pRemainSeconds)
            {
                m_ActiveGetDailyReward.Value = true;
                m_CheckinCount.Value += 1;
                newDayBuzz = true;
                m_DailyTimer.Start(pRemainSeconds);
                EventDispatcher.Raise(new NewDayCheckinEvent());
                Debug.Log("Start a new day");
            }
            public List<RewardInfo> ClaimDailyReward()
            {
                var r = GeneralAPI.ClaimReward(new RewardInfo(
                    0,0,
                    //IDs.REWARD_CURRENCY, IDs.CURRENCY_GEM,
                    DAILY_REWARD_GEM));
                m_ActiveGetDailyReward.Value = false;
                return r;
            }
            public void EndDayIn10s()
            {
                m_DailyTimer.Start(10);
            }

            public void CountWatchedAds()
            {
                m_CountWachedAds.Value += 1;
            }

            public void SetLastTimeAds(DateTime pTime)
            {
                m_LastTimeAds.Value = pTime;
            }

            public bool ShouldShowForceAd()
            {
                var lastTimeForceAd = m_LastTimeAds.Value;
                if (lastTimeForceAd != null && lastTimeForceAd.Value.AddSeconds(FORCE_ADS_COOLDOWN) > DateTime.Now)
                    return false;

                return false;
            }

            public List<RewardInfo> ClaimBonusGems()
            {
                var r = GeneralAPI.ClaimReward(new RewardInfo(
                    0,0,
                    //IDs.REWARD_CURRENCY, IDs.CURRENCY_GEM,
                    m_GemBonusPerLevel.Value));
                m_GemBonusPerLevel.Value = 0;
                return r;
            }
        }
    }
