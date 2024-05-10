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
    public class UIShop2 : PanelController
    {
        [Header("Info")]
        [SerializeField] private TMP_InputField m_inputShopName;
        [SerializeField] private GameObject m_imgShopLevelPrefab;
        [SerializeField] private Transform m_shopLevelParent;
        [SerializeField] private Image m_imgShop;
        [Header("Main")]
        [SerializeField] private TextMeshProUGUI m_txtdes;
        [SerializeField] private List<TextMeshProUGUI> m_listTxtNameItem = new List<TextMeshProUGUI>();
        [SerializeField] private List<JustButton> m_listBtnSelect = new List<JustButton>();
        [SerializeField] private List<TextMeshProUGUI> m_listTxtBtnSelect = new List<TextMeshProUGUI>();
        [SerializeField] private List<GameObject> m_listItemSelect = new List<GameObject>();
        [SerializeField] private List<GameObject> m_listImgNotSelect = new List<GameObject>();
        [SerializeField] private List<TextMeshProUGUI> m_listTxtNameItemSelect = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> m_listTxtCost = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> m_listTxtPrice = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> m_listTxtProfit = new List<TextMeshProUGUI>();
        [SerializeField] private List<JustButton> m_listBtnAdd = new List<JustButton>();
        [SerializeField] private List<JustButton> m_listBtnSub = new List<JustButton>();
        private BuildingData m_buildingData;
        public List<int> m_listIdItemSelect = new List<int>();

        private void Start()
        {
            foreach (var btn in m_listBtnSelect)
            {
                btn.onClick.AddListener(() =>
                {
                    SelectItem(m_listBtnSelect.IndexOf(btn));
                });
            }

            foreach (var btn in m_listBtnAdd)
            {
                btn.onClick.AddListener(() =>
                {
                    ChangeValueItem(TypeChangeValueItem.add, m_listBtnAdd.IndexOf(btn));
                });
            }
            foreach (var btn in m_listBtnSub)
            {
                btn.onClick.AddListener(() =>
                {
                    ChangeValueItem(TypeChangeValueItem.sub, m_listBtnSub.IndexOf(btn));
                });
            }
        }
        public void ShowShopPopUp(BuildingData buildingData)
        {
            m_buildingData = buildingData;
            m_listIdItemSelect = buildingData.shopData.item_selected;
            ShopDefinition shopDefinition = BuildData.Instance.GetShopDefById(buildingData.idShop);
            // info
            m_inputShopName.text = buildingData.shopData.name;
            SetLevelShop(shopDefinition.shop_Level);
            //main
            m_txtdes.text = shopDefinition.des;
            for(int i = 0 ; i < m_listTxtNameItem.Count; i++)
            {
                m_listTxtNameItem[i].text = shopDefinition.name_item[i];
                if (m_listIdItemSelect.Contains(i))
                {
                    m_listTxtBtnSelect[i].text = "-";
                }
                else
                {
                    m_listTxtBtnSelect[i].text = "+";
                }
            }
            
          // Load Item Select
          LoadItemSelect();

        }

        void LoadItemSelect()
        {
            ShopDefinition shopDefinition = BuildData.Instance.GetShopDefById(m_buildingData.idShop);
            for (int i = 0; i < m_listItemSelect.Count; i++)
            {
                m_listItemSelect[i].SetActive(false);
                m_listImgNotSelect[i].SetActive(true);
            }
            for (int i = 0; i < m_listIdItemSelect.Count; i++)
            {
                m_listItemSelect[i].SetActive(true);
                m_listImgNotSelect[i].SetActive(false);
                m_listTxtNameItemSelect[i].text = shopDefinition.name_item[m_listIdItemSelect[i]];
                float cost = m_buildingData.shopData.price_item[m_listIdItemSelect[i]];
                float profit = m_buildingData.shopData.profit[m_listIdItemSelect[i]];
                m_listTxtCost[i].text = cost.ToString();
                m_listTxtProfit[i].text = profit.ToString();
                m_listTxtPrice[i].text = (cost + profit).ToString();
            }
        }
        void CheckBtnSelect(int id)
        {
            for (int i = 0; i < m_listTxtNameItem.Count; i++)
            {
                if (m_listIdItemSelect.Contains(id))
                {
                    m_listTxtBtnSelect[id].text = "-";
                }
                else
                {
                    m_listTxtBtnSelect[id].text = "+";
                }
            }
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

        void SelectItem(int index)
        {
            Debug.Log("11111111");
            if (m_listIdItemSelect.Contains(index))
            {
                m_listTxtBtnSelect[index].text = "+";
                m_listIdItemSelect.Remove(index);
                DataInGame.Instance.BuildingGroup.SaveDataBulding();
                LoadItemSelect();
            }
            else
            {
                Debug.Log("222222" + m_listTxtBtnSelect.Count);
                if (m_listIdItemSelect.Count < 3)
                {
                    Debug.Log("loadddddddd");
                    m_listTxtBtnSelect[index].text = "-";
                    m_listIdItemSelect.Add(index);
                    DataInGame.Instance.BuildingGroup.SaveDataBulding();
                    LoadItemSelect();
                }
            }
        }

        void ChangeValueItem(TypeChangeValueItem type, int index)
        {
            List<int> listProfit = m_buildingData.shopData.profit;
            List<int> listPrice = m_buildingData.shopData.price_item;
            if (type == TypeChangeValueItem.add)
            {
                if (listProfit[m_listIdItemSelect[index]] < 19)
                {
                    listProfit[m_listIdItemSelect[index]] += 1;
                }
               
            }
            else
            {
                if (listProfit[m_listIdItemSelect[index]] > 0)
                {
                    listProfit[m_listIdItemSelect[index]] -= 1;
                }
            }
            DataInGame.Instance.BuildingGroup.SaveDataBulding();
            m_listTxtProfit[index].text = listProfit[m_listIdItemSelect[index]].ToString();
            m_listTxtPrice[index].text = (listPrice[m_listIdItemSelect[index]] +  listProfit[m_listIdItemSelect[index]]).ToString();
        }
        internal override void Back()
        {
            base.Back();
            ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
        }
    }
}
