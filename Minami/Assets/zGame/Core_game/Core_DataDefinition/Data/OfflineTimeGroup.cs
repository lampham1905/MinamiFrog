using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Data
{
    public class OfflineTimeGroup : DataGroup
    {
        private FloatData m_LastLocalSecondsUpdated;
        private FloatData m_LastServerSecondUpdated;
        private float m_SecondsElapsed;
        private float m_OfflineSeconds;

        public OfflineTimeGroup(int pId) : base(pId)
        {
            m_LastLocalSecondsUpdated = AddData(new FloatData(0));
            m_LastServerSecondUpdated = AddData(new FloatData(1));
            m_OfflineSeconds = 0;
        }

        public override void PostLoad()
        {
            base.PostLoad();

            m_OfflineSeconds += CalcOfflineSeconds();
        }

        public override void OnApplicationPaused(bool pPaused)
        {
            RefreshOfflineSeconds();

            Debug.Log($"Offline: {m_OfflineSeconds} Seconds");

            base.OnApplicationPaused(pPaused);
        }

        public float GetOfflineSeconds()
        {
            return m_OfflineSeconds;
        }

        public void ResetTime()
        {
            m_OfflineSeconds = 0;
        }

        public void RefreshOfflineSeconds()
        {
            m_OfflineSeconds += CalcOfflineSeconds();
        }

        private float CalcOfflineSeconds()
        {
            var timeManager = TimerTaskManager.instance;
            float offlineSeconds = 0;
            // Check local
            var t1 = m_LastLocalSecondsUpdated.Value;
            if (t1 > 0)
            {
                var t2 = timeManager.GetMillisSinceBoot() / TimerTaskManager.MILLISECONDS_PER_SECOND;
                var dt = t2 > t1 ? t2 - t1 : t2;
                offlineSeconds = dt;
                m_LastLocalSecondsUpdated.Value = t2;
            }
            //// Check Server
            float serverDeltaSeconds = 0;
            var lastServerSeconds = m_LastServerSecondUpdated.Value;
            if (lastServerSeconds > 0)
            {
                var currentServerSecond = timeManager.GetCurrentServerSeconds();
                if (currentServerSecond > 0)
                {
                    serverDeltaSeconds = currentServerSecond - lastServerSeconds;
                    m_LastServerSecondUpdated.Value = currentServerSecond;
                    offlineSeconds = serverDeltaSeconds;
                }
            }
            //Debug.Log(" 333333333333333333333    :" + offlineSeconds);
            return offlineSeconds;
        }

        public void CountTime(float pElapsedTime)
        {
            m_SecondsElapsed += pElapsedTime;
            if (m_SecondsElapsed > 5)
            {
                m_SecondsElapsed = 0;

                var timeManager = TimerTaskManager.instance;
                m_LastLocalSecondsUpdated.Value = timeManager.GetMillisSinceBoot() / TimerTaskManager.MILLISECONDS_PER_SECOND;
                var currentServerSeconds = timeManager.GetCurrentServerSeconds();
                if (currentServerSeconds > 0)
                    m_LastServerSecondUpdated.Value = currentServerSeconds;
            }
        }
    }
}
