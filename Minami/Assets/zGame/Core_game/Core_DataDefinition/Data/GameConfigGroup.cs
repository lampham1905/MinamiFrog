using System;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;

namespace Lam.zGame.Core_game.Core_DataDefinition.Data
{
    public enum UserStates
    {
        Init = -1,
        NewUser = 0,
        OldUser = 1
    }

    public class GameConfigGroup : DataGroup
    {
        #region Members
        private IntegerData mLastDay;
        private IntegerData mCountFreeWatchMount;
        private BoolData mIsFirstBattle;
        private BoolData mIsFirstRequestAttLog;
        private IntegerData mCountAdsStamina;
        private BoolData mIsFirstPurchase;
        private IntegerData mCountBuySolLunaPack;
        private IntegerData mLastDayNotification;
        private IntegerData mCampaignSession;

        private IntegerData mRatingState;
        private DateTimeData mLastTimeLater;
        private IntegerData mUserState;

        private StringData mUserId;
        private FloatData mLocalPlayTime;

        protected TimerTask m_SessionCountdownTimer;
        //----
        public int CountFreeWatchMount => mCountFreeWatchMount.Value;
        public bool IsFirstBattle => mIsFirstBattle.Value;
        public bool IsFirstRequestAttLog => mIsFirstRequestAttLog.Value;
        public int CountAdsStamina => mCountAdsStamina.Value;
        public bool IsFirstPurchase => mIsFirstPurchase.Value;
        public int CountBuySolLunaPack => mCountBuySolLunaPack.Value;
        public int CampaignSession
        {
            get => mCampaignSession.Value;
            set => mCampaignSession.Value = value;
        }

        public string UserId
        {
            get => mUserId.Value;
            set => mUserId.Value = value;
        }
        public float LocalPlayTime
        {
            get => mLocalPlayTime.Value;
            set => mLocalPlayTime.Value = value;
        }

        // 0-unrate, 1-nothank, 2-later, 3-sure.
        public int RatingState => mRatingState.Value;
        public DateTime? LastTimeLater => mLastTimeLater.Value;
        public UserStates UserStates => (UserStates)mUserState.Value;
        public bool isNewDay { get; private set; }
        #endregion

        //==============================================

        #region Public

        public GameConfigGroup(int pId) : base(pId)
        {
            mLastDay = AddData(new IntegerData(1));
            mCountFreeWatchMount = AddData(new IntegerData(2));
            mIsFirstBattle = AddData(new BoolData(3, false));
            mCountAdsStamina = AddData(new IntegerData(4));
            mIsFirstPurchase = AddData(new BoolData(5, false));
            mLastDayNotification = AddData(new IntegerData(6));
            mCountBuySolLunaPack = AddData(new IntegerData(7));
            mIsFirstRequestAttLog = AddData(new BoolData(8, false));
            m_SessionCountdownTimer = AddData(new TimerTask(9, false));

            mRatingState = AddData(new IntegerData(10, 0));
            mLastTimeLater = AddData(new DateTimeData(11));
            mCampaignSession = AddData(new IntegerData(12, -1));
            mUserState = AddData(new IntegerData(13, (int)UserStates.Init));

            mUserId = AddData(new StringData(14));
            mLocalPlayTime = AddData(new FloatData(15));
        }

        public override void PostLoad()
        {
            base.PostLoad();
            if (mIsFirstPurchase.Value)
            {
                if (!m_SessionCountdownTimer.IsRunning)
                {
                    ResetSession();
                }
                else
                {
                    m_SessionCountdownTimer.SetOnComplete((task, secondsRemain) =>
                    {
                        ResetSession();
                    });
                }
            }
            CheckNewDay();
        }
        public void ResetSession()
        {
            mIsFirstPurchase.Value = false;

            //m_SessionCountdownTimer.Start(7 * 24 * 60 * 60);
            //m_SessionCountdownTimer.SetOnComplete((task, secondsRemain) =>
            //{
            //    ResetSession();
            //});
        }

        public bool CheckCanWhatchFreeMount()
        {
            if (mCountFreeWatchMount.Value > 0)
            {
                return false;
            }
            return true;
        }
        public void WatchFreeMount()
        {
            mCountFreeWatchMount.Value += 1;
        }
        public void SetActiveIsFirstPurchase()
        {
            mIsFirstPurchase.Value = true;

            m_SessionCountdownTimer.Start(7 * 24 * 60 * 60);
            //m_SessionCountdownTimer.Start(1* 60);
            m_SessionCountdownTimer.SetOnComplete((task, secondsRemain) =>
            {
                ResetSession();
            });

        }
        public void BuySolLunaPack()
        {
            mCountBuySolLunaPack.Value += 1;
        }
        public void SetActiveFirstBattle()
        {
            mIsFirstBattle.Value = true;
        }
        public void SetActiveFirstRequestLog()
        {
            mIsFirstRequestAttLog.Value = true;
        }
        public bool CheckCanWhatchStamina()
        {
            if (mCountAdsStamina.Value > 3)
            {
                return false;
            }
            return true;
        }
        public void WatchStamina()
        {
            mCountAdsStamina.Value += 1;
        }

        public bool CanShowRating()
        {
            if(RatingState != 2)
            {
                return RatingState == 0;
            }    
            else
            {
                return LastTimeLater.Value.Subtract(DateTime.Now).TotalDays >= 2;
            }    
        }
        public void SetRatingState(int state)
        {
            mRatingState.Value = state;
        }
        public void SetLastTimeLater()
        {
            mLastTimeLater.Value = DateTime.Now;
        }
        public void ChangeUserState()
        {
            if(mUserState.Value == (int)UserStates.Init)
                mUserState.Value = (int)UserStates.NewUser;
            else if(mUserState.Value == (int)UserStates.NewUser)
                mUserState.Value = (int)UserStates.OldUser;
        }
        #endregion

        //=============================================

        #region Private

        private void CheckNewDay()
        {
            int day = DateTime.Now.DayOfYear;
            if (mLastDay.Value != day)
            {
                mLastDay.Value = (day);
                mCountFreeWatchMount.Value = 0;
                mCountAdsStamina.Value = 0;
                //Config.isNewDayAdManager = true;
                isNewDay = true;
            }
            if (mLastDayNotification.Value != day)
            {
                mLastDayNotification.Value = day;
                //---notification---
                UnityMainThreadDispatcher.Instance().Enqueue(CheckNotificationNewDay);
                //CheckNotificationNewDay();
            }
        }
        void CheckNotificationNewDay()
        {
            //double seconds = 0;
            //DateTime now = DateTime.Now;
            //now = TimerTaskManager.instance.GetNow();
            //seconds = TimeHelper.GetSecondsTillMindNight(now);
            SendNewDayNotification(DateTime.Now.AddDays(1));
        }
        void SendNewDayNotification(DateTime pTime)
        {
            //if (UnmanagedData.NewDayNotification != -1)
            //{
            //    LocalNotificationHelper.Instance.CancelNotification(UnmanagedData.NewDayNotification);
            //    UnmanagedData.NewDayNotification = -1;
            //}
            //UnmanagedData.NewDayNotification = LocalNotificationHelper.Instance.SendNotification("Wild Gunner: Claim your free daily Chest!", "Ready your weapons and prepare for the monster Stampede!", pTime);
            //UnmanagedData.LastTimeSendNotification = pTime;
        }
        #endregion
    }
}