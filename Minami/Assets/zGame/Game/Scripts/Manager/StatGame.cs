using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

namespace Lam
{
    public class StatGame : MonoBehaviour
    {
        private static float m_satisfaction = -1;
        private static float m_money = -1;
        private static int m_villagers = -1;
        private static float m_beauty = -1;
        private static float m_youth = -1;
        private static float m_eleder = -1;
        private static float m_golbin = -1;
        public static float SATISFACTION
        {
            get
            {
                if (m_satisfaction == -1)
                {
                    m_satisfaction = 0;
                }
                return m_satisfaction;
            }
            set
            {
                if (m_satisfaction != value)
                {
                    m_satisfaction = value;
                    DataInGame.Instance.StatGroup.satisfaction.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_SATISFACTION, value);
                }
            }
        }
        public static float MONEY
        {
            get
            {
                if (m_money == -1)
                {
                    m_money = 0;
                }
                return m_money;
            }
            set
            {
                if (m_money != value)
                {
                    m_money = value;
                    DataInGame.Instance.StatGroup.money.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_MONEY, value);
                }
            }
        }
        public static int VILLAGERS
        {
            get
            {
                if (m_villagers == -1)
                {
                    m_villagers = 0;
                }
                return m_villagers;
            }
            set
            {
                if (m_villagers != value)
                {
                    m_villagers = value;
                    DataInGame.Instance.StatGroup.villagers.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_VILLAGERS, value);
                }
            }
        }
        public static float BEAUTY
        {
            get
            {
                if (m_beauty == -1)
                {
                    m_beauty = 0;
                }
                return m_beauty;
            }
            set
            {
                if (m_beauty != value)
                {
                    m_beauty = value;
                    DataInGame.Instance.StatGroup.beauty.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_BEAUTY, value);
                }
            }
        }
        public static float YOUTH
        {
            get
            {
                if (m_youth == -1)
                {
                    m_youth = 0;
                }
                return m_youth;
            }
            set
            {
                if (m_youth != value)
                {
                    m_youth = value;
                    DataInGame.Instance.StatGroup.youth.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_YOUTH, value);
                }
            }
        }
        public static float ELDER
        {
            get
            {
                if (m_eleder == -1)
                {
                    m_eleder = 0;
                }
                return m_eleder;
            }
            set
            {
                if (m_eleder != value)
                {
                    m_eleder = value;
                    DataInGame.Instance.StatGroup.elder.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_ELDER, value);
                }
            }
        }
        public static float GOLBIN
        {
            get
            {
                if (m_golbin == -1)
                {
                    m_golbin = 0;
                }
                return m_golbin;
            }
            set
            {
                if (m_golbin != value)
                {
                    m_golbin = value;
                    DataInGame.Instance.StatGroup.golbin.Value = value;
                    DataInGame.Instance.Save();
                    ManagerEvent.RaiseEvent(EventCMD.UPDATE_GOLBIN, value);
                }
            }
        }
    }
}
