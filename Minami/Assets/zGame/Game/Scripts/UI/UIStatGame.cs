using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using TMPro;
using UnityEngine;

namespace Lam
{
    public class UIStatGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_txtSatisfaction;
        [SerializeField] private TextMeshProUGUI m_txtMoney;
        [SerializeField] private TextMeshProUGUI m_txtVillagers;
        [SerializeField] private TextMeshProUGUI m_txtBeauty;
        [SerializeField] private TextMeshProUGUI m_txtYouths;
        [SerializeField] private TextMeshProUGUI m_txtElders;
        [SerializeField] private TextMeshProUGUI m_txtGolibins;
        private void Awake()
        {
            ManagerEvent.RegEvent(EventCMD.UPDATE_SATISFACTION, UpdateSatisfaction);
            ManagerEvent.RegEvent(EventCMD.UPDATE_MONEY, UpdateMoney);
            ManagerEvent.RegEvent(EventCMD.UPDATE_VILLAGERS, UpdateVillagers);
            ManagerEvent.RegEvent(EventCMD.UPDATE_BEAUTY, UpdateBeauty);
            ManagerEvent.RegEvent(EventCMD.UPDATE_YOUTH, UpdateYouth);
            ManagerEvent.RegEvent(EventCMD.UPDATE_ELDER, UpdateElder);
            ManagerEvent.RegEvent(EventCMD.UPDATE_GOLBIN, UpdateGolbin);

        }

        void UpdateSatisfaction(object e)
        {
            float val = (float)e;
            m_txtSatisfaction.text = val.ToString();
        }
        void UpdateMoney(object e)
        {
            float val = (float)e;
            m_txtMoney.text = val.ToString();
        }
        void UpdateVillagers(object e)
        {
            int val = (int)e;
            m_txtVillagers.text = val.ToString();
        }
        void UpdateBeauty(object e)
        {
            float val = (float)e;
            m_txtBeauty.text = val.ToString();
        }
        void UpdateYouth(object e)
        {
            float val = (float)e;
            m_txtYouths.text = val.ToString();
        }
        void UpdateElder(object e)
        {
            float val = (float)e;
            m_txtElders.text = val.ToString();
        }
        void UpdateGolbin(object e)
        {
            float val = (float)e;
            m_txtGolibins.text = val.ToString();
        }
    }
}
