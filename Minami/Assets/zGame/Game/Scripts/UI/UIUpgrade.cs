using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using ntDev;
using TMPro;
using UnityEngine;

namespace Lam
{
    public class UIUpgrade : PanelController
    {
        [SerializeField] private GameObject m_popUpNormal;
        [SerializeField] private GameObject m_popUpForever;
        
        [SerializeField] private List<TextMeshProUGUI> m_listTxtCostNormal = new List<TextMeshProUGUI>();
        [SerializeField] private TextMeshProUGUI m_txtCostForever;

        [SerializeField] private List<TextMeshProUGUI> m_listTxtAmountUpgradeNormal = new List<TextMeshProUGUI>();
        [SerializeField] private TextMeshProUGUI m_txtAmountUpgradeForever;

        [SerializeField] private List<TextMeshProUGUI> m_listTxtNameUpgradeNormal = new List<TextMeshProUGUI>();
        [SerializeField] private TextMeshProUGUI m_txtNameUpgradeForever;
        private UpgradesDefinition m_upgradesDefinition;
        public void ShowPopUpNormal(UpgradesDefinition upgradesDefinition)
        {
            this.m_upgradesDefinition = upgradesDefinition;
            m_popUpForever.gameObject.SetActive(false);
            m_popUpNormal.gameObject.SetActive(true);
            m_listTxtCostNormal[0].text = upgradesDefinition.cost_upgrade_1.ToString() + "$";
            m_listTxtCostNormal[1].text = upgradesDefinition.cost_upgrade_2.ToString() + "$";
            m_listTxtNameUpgradeNormal[0].text = upgradesDefinition.name_upgrade_1.ToString();
            m_listTxtNameUpgradeNormal[1].text = upgradesDefinition.name_upgrade_2.ToString();
            if (upgradesDefinition.type_upgrade == 1)
            {
                int amountUpgrade1 = 0;
                for (int i = 0; i < upgradesDefinition.amount_upgrade_1.Count; i++)
                {
                    amountUpgrade1 += upgradesDefinition.amount_upgrade_1[i];
                }
                m_listTxtAmountUpgradeNormal[0].text = "+" + amountUpgrade1.ToString();
                int amountUpgrade2 = 0;
                for (int i = 0; i < upgradesDefinition.amount_upgrade_2.Count; i++)
                {
                    amountUpgrade2 += upgradesDefinition.amount_upgrade_2[i];
                }
                m_listTxtAmountUpgradeNormal[1].text = "+" + amountUpgrade2.ToString();
            }
            else
            {
                int amountBuilding = DataInGame.Instance.BuildingGroup.listBuildingData.Count;
                m_listTxtAmountUpgradeNormal[0].text = "+" + amountBuilding.ToString();
                m_listTxtAmountUpgradeNormal[1].text = "+" + amountBuilding.ToString();
            }
        }

        public void ShowPopUpforever(UpgradesDefinition upgradesDefinition)
        {
            this.m_upgradesDefinition = upgradesDefinition;
            m_popUpForever.gameObject.SetActive(true);
            m_popUpNormal.gameObject.SetActive(false);
            m_txtCostForever.text = upgradesDefinition.cost_upgrade_1.ToString() + "$";
            m_txtNameUpgradeForever.text = upgradesDefinition.name_upgrade_1.ToString();
            m_txtAmountUpgradeForever.text = "+" + upgradesDefinition.amount_upgrade_1[0].ToString();
        }
        internal override void Back()
        {
            base.Back();
            ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
        }
    }
}
