using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using ntDev;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lam
{
    public class UIGamePlay : PanelController
    {
        [Separator("Header")]
        [SerializeField] private JustButton m_btnAddBuilding;
        
        [Separator("Panels")]
        [SerializeField] private UIBuildBuilding m_UIBuildBuilding;
        [SerializeField] private UIUpgrade m_UIUpgrade;
        [SerializeField] private UIShop1 m_uiShop1;
        [SerializeField] private UIShop2 m_uiShop2;

        [Separator("Bottom")] 
        [SerializeField] private JustButton m_btnStartDay;
        //[SerializeField] private 
        public static UIGamePlay m_Instance;
        public static UIGamePlay Instance => m_Instance;
        
        [FormerlySerializedAs("_mUiUpgradeCustomize")] [SerializeField] private UIUpgradeShop mUIUpgradeShop;
        public UIUpgradeShop UIUpgradeShop => mUIUpgradeShop;
        
        protected override void Awake()
        {
            base.Awake();

            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
            ManagerEvent.RegEvent(EventCMD.SHOW_BTN, ShowBtn);
            ManagerEvent.RegEvent(EventCMD.HIDE_BTN, HideBtn);
        }
       
        private void Start()
        {
            
        }
        internal override void Init()
        {
            base.Init();
            StartCoroutine(PostInit());
        }
        private IEnumerator PostInit()
        {
            mUIUpgradeShop.Init();
            m_btnAddBuilding.onClick.AddListener(ShowAddBuildingPanel);
            m_btnStartDay.onClick.AddListener(StartDay);

            yield return null;
            yield return null;
            ManagerEvent.RegEvent(EventCMD.SHOWSHOP, ShowShop);
            ManagerEvent.RegEvent(EventCMD.SHOWUPGRADE,ShowUpgrade);
            //m_HorizontalSnapScrollView.MoveToItem(Tab.Campain.GetHashCode(), true);
            //m_CurTap = Tab.Campain;

            //EventDispatcher.AddListener<PlayerLevelChangedEvent>(OnPlayerLevelChanged);
            //EventDispatcher.AddListener<PlayerExpChangedEvent>(OnPlayerExpChanged);
            //EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
            
            //HybirdAudioManager.Instance.PlayMusicById(IDs.MUSIC_MAIN_MENU, true, 6f);
            //m_TabEvent.GetComponent<Tutorial_EventButton>().Refresh();
            //Tutorial_Manager.Instance.Init();
        }
        private void ShowAddBuildingPanel()
        {
            PushPanelToTop(ref m_UIBuildBuilding);
            m_UIBuildBuilding.Init();
            m_UIBuildBuilding.ShowAddBuild(0);
        }

        private void StartDay()
        {
            ManagerEvent.RaiseEvent(EventCMD.START_DAY, DataInGame.Instance.BuildingGroup.GetAmountBuilding());
            ManagerEvent.RaiseEvent(EventCMD.SPAWN_FROG);
            HideBtn();
            
        }
        private void ShowUpgrade(object e)
        {
            BuildingData buildingData = (BuildingData)e;
            UpgradesDefinition upgradesDefinition = BuildData.Instance.GetUpgradeById(buildingData.idBulding);
            PushPanelToTop(ref m_UIUpgrade);
            if (upgradesDefinition.type_upgrade == 2)
            {
                m_UIUpgrade.ShowPopUpforever(upgradesDefinition);
            }
            else
            {
                m_UIUpgrade.ShowPopUpNormal(upgradesDefinition);
            }
            HideBtn();
        }
        

        private void ShowShop(object e)
        {
            BuildingData buildingData = (BuildingData)e;
            ShopDefinition shopDefinition = BuildData.Instance.GetShopDefById(buildingData.idShop);
            if (shopDefinition.type == 1)
            {
                PushPanelToTop(ref m_uiShop1);
                m_uiShop1.ShowShopPopUp(buildingData);
            }
            else if(shopDefinition.type == 2)
            {
                PushPanelToTop(ref m_uiShop2);
                m_uiShop2.ShowShopPopUp(buildingData);
            }

            HideBtn();
        }

        void HideBtn(object e = null)
        {
            m_btnAddBuilding.gameObject.SetActive(false);
            m_btnStartDay.gameObject.SetActive(false);
        }

        void ShowBtn(object e = null)
        {
            m_btnAddBuilding.gameObject.SetActive(true);
            m_btnStartDay.gameObject.SetActive(true);
        }
    }
}
