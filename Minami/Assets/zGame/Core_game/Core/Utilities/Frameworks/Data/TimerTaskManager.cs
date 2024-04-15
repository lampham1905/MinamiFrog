/**
 *  Based on TimeManager of hnim.
 *  Copyright (c) 2017 RedAntz. All rights reserved.
 */

using System;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using UnityEngine;
using UnityEngine.Networking;

namespace Lam.zGame.Core_game.Core.Utilities.Frameworks.Data
{
    public class TimerTaskManager : IUpdate
    {
        #region Constants

        public const int MONTHS_PER_YEAR = 12;
        public const int DAYS_PER_WEEK = 7;
        public const int DAYS_PER_MONTH = 30;
        public const int HOURS_PER_DAY = 24;
        public const int MINUTES_PER_HOUR = 60;

        public const int MILLISECONDS_PER_SECOND = 1000;
        public const int MICROSECONDS_PER_SECOND = 1000 * 1000;
        public const long NANOSECONDS_PER_SECOND = 1000 * 1000 * 1000;

        public const long MICROSECONDS_PER_MILLISECOND = MICROSECONDS_PER_SECOND / MILLISECONDS_PER_SECOND;

        public const long NANOSECONDS_PER_MICROSECOND = NANOSECONDS_PER_SECOND / MICROSECONDS_PER_SECOND;
        public const long NANOSECONDS_PER_MILLISECOND = NANOSECONDS_PER_SECOND / MILLISECONDS_PER_SECOND;

        public const float SECONDS_PER_NANOSECOND = 1f / NANOSECONDS_PER_SECOND;
        public const float MICROSECONDS_PER_NANOSECOND = 1f / NANOSECONDS_PER_MICROSECOND;
        public const float MILLISECONDS_PER_NANOSECOND = 1f / NANOSECONDS_PER_MILLISECOND;

        public const float SECONDS_PER_MICROSECOND = 1f / MICROSECONDS_PER_SECOND;
        public const float MILLISECONDS_PER_MICROSECOND = 1f / MICROSECONDS_PER_MILLISECOND;

        public const float SECONDS_PER_MILLISECOND = 1f / MILLISECONDS_PER_SECOND;

        public const int SECONDS_PER_MINUTE = 60;
        public const int SECONDS_PER_HOUR = SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
        public const int SECONDS_PER_DAY = SECONDS_PER_HOUR * HOURS_PER_DAY;
        public const int SECONDS_PER_WEEK = SECONDS_PER_DAY * DAYS_PER_WEEK;
        public const int SECONDS_PER_MONTH = SECONDS_PER_DAY * DAYS_PER_MONTH;
        public const int SECONDS_PER_YEAR = SECONDS_PER_MONTH * MONTHS_PER_YEAR;

        #endregion

        //=============================================================

        #region Members

        private static TimerTaskManager mInstance;
        public static TimerTaskManager instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new TimerTaskManager();
                return mInstance;
            }
        }

        private DateTime mDayZero;
        private bool mLocalTimeSynced;
        private long mLocalTimeOffset;

        private long mServerTimeOffset;
        private bool mTimeServerFetched;
        private bool mFetchingTimeServer;
        private float mSecondsElapsed;

        private List<TimerTask> mTimerTasks;

        public int id { get; set; }

        #endregion

        //=============================================================

        #region Public

        public TimerTaskManager()
        {
            mTimerTasks = new List<TimerTask>();
            mLocalTimeSynced = false;
            mTimeServerFetched = false;
            mDayZero = new DateTime(2017, 1, 1);

            SyncTimers();

            WaitUtil.AddUpdate(this);
        }

        public long GetSecondsSinceBoot()
        {
            return GetMillisSinceBoot() / MILLISECONDS_PER_SECOND;
        }

        public long GetCurrentServerSeconds()
        {
            return 0;

            if (mTimeServerFetched == false)
            {
                SyncTimeServer();
                return 0;
            }
            else
            {
                return (GetLocalMillisSeconds() + mServerTimeOffset) / MILLISECONDS_PER_SECOND;
            }
        }

        public long GetMillisSinceBoot()
        {
            SyncTimeLocal();
            return GetLocalMillisSeconds() + mLocalTimeOffset;
        }

        public void OnApplicationFocus(bool pFocus)
        {
            if (pFocus)
            {
                SyncTimers();
            }
            else
            {
                mLocalTimeSynced = false;
                mTimeServerFetched = false;
            }
        }

        public DateTime GetNow()
        {
            //UnityEngine.Debug.LogError("mServerTimeOffset: " + mServerTimeOffset);
            return DateTime.Now.AddMilliseconds(mServerTimeOffset);
        }

        public void SyncTimers()
        {
            SyncTimeLocal();
            SyncTimeServer();
        }

        public void AddTimerTask(TimerTask pTimer)
        {
            mTimerTasks.Add(pTimer);
        }
        long currentLocalSeconds = 0;
        int countUpdateFrame = 0;
        bool isActiveCheckC = false;
        public void Update(float pUnscaledDetalTime)
        {
            int count = mTimerTasks.Count;
            if (count > 0)
            {
                mSecondsElapsed += pUnscaledDetalTime;
                //---Dai Them---
                if (mSecondsElapsed > 3600)
                {
                    // 1h ?
                    mSecondsElapsed = 1;
                }
                //-----------
                if (mSecondsElapsed >= 1.0f)
                {
                    mSecondsElapsed -= 1.0f;
                    long currentServerSeconds = GetCurrentServerSeconds();
                    //long currentLocalSeconds = GetSecondsSinceBoot();
                    long currentLocalSecondsNew = GetSecondsSinceBoot();
                    //currentLocalSeconds = currentLocalSecondsNew;
                    //if (GameData.Instance.Initialized)
                    //{
                    if (!isActiveCheckC)
                    {
                        countUpdateFrame++;
                        if (countUpdateFrame >= 10)
                        {
                            isActiveCheckC = true;
                        }
                        currentLocalSeconds = currentLocalSecondsNew;
                    }
                    else
                    {
                        //if (currentLocalSeconds > 0 && (currentLocalSecondsNew - currentLocalSeconds) > 360)
                        if (currentLocalSeconds > 0 && (currentLocalSecondsNew - currentLocalSeconds) > 3600)
                        {
                            //1h ?
                            return;
                        }
                        currentLocalSeconds = currentLocalSecondsNew;
                    }
                    //}

                    for (int i = count - 1; i >= 0; i--)
                    {
                        var task = mTimerTasks[i];
                        if (task != null)
                        {
                            if (task.IsRunning)
                                task.Update(currentServerSeconds, currentLocalSeconds, 1);
                            else
                                mTimerTasks.RemoveAt(i);
                        }
                    }
                }
            }
        }

        #endregion

        //================================================================

        #region Private

        private long GetLocalMillisSeconds()
        {
            return (long)(DateTime.Now - mDayZero).TotalMilliseconds;
        }

        private void SyncTimeLocal()
        {
            if (mLocalTimeSynced == false)
            {
                mLocalTimeSynced = true;
                mLocalTimeOffset = RNative.getMillisSinceBoot() - GetLocalMillisSeconds();
            }
        }

        private void SyncTimeServer()
        {
            return;

            if (!mTimeServerFetched && !mFetchingTimeServer)
            {
                string url = "http://divmob.com/api/zombieage/time.php";

                var form = new WWWForm();
                var request = UnityWebRequest.Post(url, form);
                request.SendWebRequest();

                mFetchingTimeServer = true;
                WaitUtil.Start(() => request.isDone,
                    () =>
                    {
                        mFetchingTimeServer = false;
                        if (request.isNetworkError)
                        {
                            //Error
                            mTimeServerFetched = false;
                            mServerTimeOffset = 0;
                        }
                        else
                        {
                            if (request.responseCode == 200)
                            {
                                mTimeServerFetched = true;
                                var text = request.downloadHandler.text;
                                var time = DateTime.MinValue;
                                if (TimeHelper.TryParse(text, out time))
                                    mServerTimeOffset = (long)(time - mDayZero).TotalMilliseconds - GetLocalMillisSeconds();
                            }
                            else
                            {
                                //Error
                                mTimeServerFetched = false;
                                mServerTimeOffset = 0;
                            }
                        }
                    });
            }
        }

        #endregion
    }
}