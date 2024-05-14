using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using TMPro;
using UnityEngine;

namespace Lam
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private UITime m_UITime;
        public float m_TimeCurrent;
        private float m_TotalTime;
        public bool isStartDay = false;
        public float timeScale;
        public static float SALE_TIME = 1;
        private float m_hour;
        private float m_minute;
        private float m_timePer5Minutes;
        private float m_timePer5MinutesCurrent;

        private void Awake()
        {
            ManagerEvent.RegEvent(EventCMD.START_DAY,StartDay);
        }

        private void Update()
        {
            if (isStartDay)
            {
                m_TimeCurrent += Time.deltaTime * SALE_TIME;
                m_timePer5MinutesCurrent += Time.deltaTime * SALE_TIME;
                if (m_timePer5MinutesCurrent >= m_timePer5Minutes)
                {
                    m_timePer5MinutesCurrent -= m_timePer5Minutes;
                    m_minute += 5;
                    if (m_minute == 60)
                    {
                        m_minute = 0;
                        m_hour += 1;
                       
                    }
                    UpdateUI();
                }

                if (m_TimeCurrent >= m_TotalTime)
                {
                    EndDay();
                }
            }
            
            
        }

        void StartDay(object e)
        {
          
            int amountAbuilding = (int)e;
            Debug.Log(amountAbuilding);
            m_TimeCurrent = 0;
            isStartDay = true;
            m_TotalTime = LogicAPI.GetTimeADay(amountAbuilding);
            m_timePer5Minutes = LogicAPI.GetTimePer5Minute(amountAbuilding);
            m_hour = 6;
            m_minute = 0;
        }

        void EndDay()
        {
            isStartDay = false;
            m_minute = 0;
            m_hour = 20;
            UpdateUI();
            ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
            ManagerEvent.RaiseEvent(EventCMD.END_DAY);
            TimeManager.SALE_TIME = 1;
        }
        void UpdateUI()
        {
            m_UITime.UpdateTime((m_hour <= 12 ? m_hour.ToString() :(m_hour - 12).ToString()) + " : " +  m_minute.ToString("00") + (m_hour <= 12 ?  " am":  " pm"));
        }
    }
    
}
