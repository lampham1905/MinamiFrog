using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lam
{
    public enum TypeChangeValueItem
    {
        add, sub
    }
    public class UIShop1 : PanelController
    {
        [Header("Info")]
        [SerializeField] private TMP_InputField m_inputShopName;
        [SerializeField] private GameObject m_imgShopLevelPrefab;
        [SerializeField] private Transform m_shopLevelParent;
        [SerializeField] private Image m_imgShop;
        [Header("Main")] 
        [SerializeField] private TextMeshProUGUI m_txtdes;
        [SerializeField] private Image m_imgProduct;
        [SerializeField] private List<TextMeshProUGUI> m_listNameItem;
        [SerializeField] private List<TextMeshProUGUI> m_listAmountItem;
        [SerializeField] private List<TextMeshProUGUI> m_listPriceItem;
        [SerializeField] private TextMeshProUGUI m_txtProfit;
        [SerializeField] private List<JustButton> m_listBtnAdd = new List<JustButton>();
        [SerializeField] private List<JustButton> m_listBtnSub = new List<JustButton>();
        [SerializeField] private TextMeshProUGUI m_txtTotalCost;
        [SerializeField] private TextMeshProUGUI m_txtSellingPrice;
        [SerializeField] private JustButton m_btnAddPrfit;
        [SerializeField] private JustButton m_btnSubProfit;
        private BuildingData m_buildingData;

        private void Start()
        {
            foreach (JustButton  btn in m_listBtnAdd)
            {
                btn.onClick.AddListener(() =>
                {
                    ChangeValueItem(TypeChangeValueItem.add, m_listBtnAdd.IndexOf(btn));
                });
            }
            foreach (JustButton  btn in m_listBtnSub)
            {
                btn.onClick.AddListener(() =>
                {
                    ChangeValueItem(TypeChangeValueItem.sub, m_listBtnSub.IndexOf(btn));
                });
            }
            m_btnAddPrfit.onClick.AddListener(() =>
            {
                ChangeProfit(TypeChangeValueItem.add);
            });
            m_btnSubProfit.onClick.AddListener(() =>
            {
                ChangeProfit(TypeChangeValueItem.sub);
            });
        }

        public void ShowShopPopUp(BuildingData buildingData)
        {
            m_buildingData = buildingData;
            ShopDefinition shopDefinition = BuildData.Instance.GetShopDefById(buildingData.idShop);
            // info
            m_inputShopName.text = buildingData.shopData.name;
            SetLevelShop(shopDefinition.shop_Level);
            // main
            m_txtdes.text = shopDefinition.des;
            for (int i = 0; i < m_listNameItem.Count; i++)
            {
                m_listNameItem[i].text = shopDefinition.name_item[i];
                m_listAmountItem[i].text = buildingData.shopData.amount_item[i].ToString();
                m_listPriceItem[i].text = "$" + buildingData.shopData.price_item[i].ToString();
            }
            m_txtProfit.text = "$" + buildingData.shopData.profit[0].ToString();
            SetPriceTotal();
        }
        
        void SetLevelShop(int levelShop)
        {
            if (m_shopLevelParent.transform.childCount > 0)
            {
                for (int i = 0; i < m_shopLevelParent.transform.childCount; i++)
                {
                    Destroy(m_shopLevelParent.transform.GetChild(i).gameObject);
                }    
            }

            for (int i = 0; i < levelShop; i++)
            {
                Instantiate(m_imgShopLevelPrefab, m_shopLevelParent);
            }
        }

        void SetPriceTotal()
        {
            ShopData data = m_buildingData.shopData;
            float totalCost = 0;
            for (int i = 0; i < data.amount_item.Count; i++)
            {
                totalCost += data.amount_item[i] * data.price_item[i];
            }

            m_txtTotalCost.text = "$" + totalCost.ToString();
            m_txtSellingPrice.text = "$" + (totalCost + data.profit[0]).ToString();
        }

        void ChangeValueItem(TypeChangeValueItem type, int index)
        {
            Debug.Log(index);
            if (type == TypeChangeValueItem.add)
            {
                m_buildingData.shopData.amount_item[index] += 1;
            }
            else
            {
                if (m_buildingData.shopData.amount_item[index] > 0)
                {
                    m_buildingData.shopData.amount_item[index] -= 1;
                }
            }
            m_listAmountItem[index].text = m_buildingData.shopData.amount_item[index].ToString();
            SetPriceTotal();
            DataInGame.Instance.BuildingGroup.listBuildingData.MarkChange();
            DataInGame.Instance.Save(); }

        void ChangeProfit(TypeChangeValueItem type)
        {
            if (type == TypeChangeValueItem.add)
            {
                if (m_buildingData.shopData.profit[0] < 19)
                {
                    m_buildingData.shopData.profit[0] += 1;
                }
            }
            else
            {
                if (m_buildingData.shopData.profit[0] > 0)
                {
                    m_buildingData.shopData.profit[0] -= 1;
                    
                }
            }
            m_txtProfit.text = "$" + m_buildingData.shopData.profit[0].ToString();
            SetPriceTotal();
            DataInGame.Instance.BuildingGroup.listBuildingData.MarkChange();
            DataInGame.Instance.Save();
        }

        internal override void Back()
        {
            base.Back();
            ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
        }
    }
    
}
