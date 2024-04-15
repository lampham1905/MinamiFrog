using System;
using System.Collections;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Components.FX;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.UI
{
    public class MainMenuPanel : PanelController
    {
        public enum Tab
        {
            None = -1,
            Campain = 2,
            Shop = 0,
            Equipment = 1,
            Talent = 3,
            Event = 4
        }

        #region Members

        public static MainMenuPanel m_Instance;
        public static MainMenuPanel Instance => m_Instance;

        public Action<PanelController> onAnyChildHide;
        public Action<PanelController> onAnyChildShow;
        
        private Action delayAction;

        [SerializeField] private GameObject panelBlockAll;

        [Separator("Header")]
        [SerializeField] private JustButton btnAddBuilding;
        
        [Separator("Main Menu Screens")]
        [SerializeField] private HorizontalSnapScrollView m_HorizontalSnapScrollView;
        //[SerializeField] private CampaignPanel m_CampainScreen;
        //[SerializeField] private EquipmentPanel m_EquipmentScreen;
        //[SerializeField] private ShopPanel m_ShopScreen;
        //[SerializeField] private TalentPanel m_TalentScreen;
        //[SerializeField] private EventPanel m_EventPanel;

        [Separator("Panels")]
        [SerializeField] private PanelController m_Container;
        //[SerializeField] private HomeDebugPanel m_HomeDebugPanel;
        [SerializeField] private MessagesPopup m_MessagePopup;
        [SerializeField] private MessagesManager m_MessagesManager;
        [SerializeField] private UIBuildBuilding m_UIBuildBuilding;
    
        [Separator("Footer")]
        [SerializeField] private CustomToggleTab m_TabShop;
        [SerializeField] private CustomToggleTab m_TabEquipment;
        [SerializeField] private CustomToggleTab m_TabCampain;
        [SerializeField] private CustomToggleTab m_TabTalent;
        [SerializeField] private CustomToggleTab m_TabEvent;
        [SerializeField] private GameObject m_BuzzShop;
        [SerializeField] private GameObject m_BuzzEquipment;
        [SerializeField] private GameObject m_BuzzTalent;
        [SerializeField] private GameObject m_BuzzEvent;
        [SerializeField] private GameObject objBg1;
        [SerializeField] private GameObject objBg2;

        private Tab m_CurTap;
        private bool m_DisplayCooldownTimer;

        private Data.GameData GameData => Data.GameData.Instance;
        
        #endregion

        //=====================================

        #region MonoBehaviour

        private void PreInit()
        {
            //StartCoroutine(IECooldownStamina());
            //StartCoroutine(IECheckin());
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        //private IEnumerator Start()
        private void Start()
        {
            Init();
        }

        private IEnumerator PostInit()
        {
            
            btnAddBuilding.onClick.AddListener(ShowAddBuildingPanel);
            //m_HorizontalSnapScrollView.onIndexChanged += OnScrollIndex_Changed;
            //m_HorizontalSnapScrollView.onScrollEnd += OnScrollEnd;
            //m_HorizontalSnapScrollView.onScrollStart += OnScrollStart;

            yield return null;
            yield return null;
            //m_HorizontalSnapScrollView.MoveToItem(Tab.Campain.GetHashCode(), true);
            //m_CurTap = Tab.Campain;

            //EventDispatcher.AddListener<PlayerLevelChangedEvent>(OnPlayerLevelChanged);
            //EventDispatcher.AddListener<PlayerExpChangedEvent>(OnPlayerExpChanged);
            //EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
            
            //HybirdAudioManager.Instance.PlayMusicById(IDs.MUSIC_MAIN_MENU, true, 6f);
            //m_TabEvent.GetComponent<Tutorial_EventButton>().Refresh();
            //Tutorial_Manager.Instance.Init();
        }

        private void OnDestroy()
        {
            //m_HorizontalSnapScrollView.onIndexChanged -= OnScrollIndex_Changed;
            //m_HorizontalSnapScrollView.onScrollEnd -= OnScrollEnd;
            //m_HorizontalSnapScrollView.onScrollStart -= OnScrollStart;

            //EventDispatcher.RemoveListener<PlayerLevelChangedEvent>(OnPlayerLevelChanged);
            //EventDispatcher.RemoveListener<PlayerExpChangedEvent>(OnPlayerExpChanged);
            //EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            btnBack.SetActive(false);
        }
        private void Update()
        {
            // bool enableTabs = !m_HorizontalSnapScrollView.IsDragging && !m_HorizontalSnapScrollView.IsSnapping;
            // m_TabCampain.interactable = enableTabs;
            // m_TabEquipment.interactable = enableTabs;
            // m_TabShop.interactable = enableTabs;
            // m_TabTalent.interactable = enableTabs;
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (TopPanel == m_Container && m_Container.StackCount == 0 && m_Container.TopPanel == null)
                {
                    if (m_CurTap == Tab.Campain)
                    {
                        //Show quit popup
                        //ShowQuitPopup();
                    }
                    else {
                        m_TabCampain.isOn = true;
                    }
                }
                else
                {
                    //if (TopPanel is RewardsPopup)
                    //{
                    //    ((RewardsPopup)TopPanel).BtnConfirm_Pressed();
                    //}
                    //else
                    //{
                    //    Back();
                    //}
                }
            }
            return;
#endif
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    if (TopPanel == m_Container && m_Container.StackCount == 0 && m_Container.TopPanel == null)
                    {
                        if (m_CurTap == Tab.Campain)
                        {
                            //Show quit popup
                            //ShowQuitPopup();
                        }
                        else
                        {
                            //MainMenuPanel.Instance.SwitchTab(MainMenuPanel.Tab.Campain);
                            m_TabCampain.isOn = true;
                        }
                    }
                    else
                    {
                        //if (TopPanel is RewardsPopup)
                        //{
                        //    ((RewardsPopup)TopPanel).BtnConfirm_Pressed();
                        //}
                        //else
                        //{
                        //    Back();
                        //}
                    }
                }
            }

        }
        //-----------------
        #endregion

        //=====================================

        #region Public

        internal override void Init()
        {
            base.Init();

            PreInit();

            if (TopPanel == null)
                PushPanelToTop(m_Container);

            //m_CampainScreen.Init();
            //m_EquipmentScreen.Init();
            //m_ShopScreen.Init();
            //m_TalentScreen.Init();
            //m_EventPanel.Init();

            //m_GemView.Init(IDs.CURRENCY_GEM);
            //m_GoldView.Init(IDs.CURRENCY_GOLD);
            //m_StaminaView.Init(IDs.CURRENCY_STAMINA);

            //m_TxtLevel.text = $"{GameData.MiscGroup.PlayerLevel}";
            //m_ExpBar.FillAmount = GameData.MiscGroup.GetNextLevelProgress();

            StartCoroutine(PostInit());
        }

        public void CurrencyFall(int id, int value, Action callback)
        {
            panelBlockAll.SetActive(true);
            switch (id)
            {
                //case IDs.CURRENCY_GEM: m_GemView.FallValue(value); break;
                //case IDs.CURRENCY_STAMINA:m_StaminaView.FallValue(value);break;
            }
            delayAction = callback;
            Invoke("DelayAction", 1);
        }    

        private void DelayAction()
        {
            panelBlockAll.SetActive(false);
            delayAction?.Invoke();
            delayAction = null;
        }    

        public void SwitchTabShortcut(Tab pTab)
        {
            switch (pTab)
            {
                case Tab.Campain:
                    m_TabCampain.isOn = true;
                    break;
                case Tab.Shop:
                    m_TabShop.isOn = true;
                    break;
                case Tab.Equipment:
                    m_TabEquipment.isOn = true;
                    break;
                case Tab.Talent:
                    m_TabTalent.isOn = true;
                    break;
                case Tab.Event:
                    m_TabEvent.isOn = true;
                    break;
            }
        }    

        public void SwitchTab(Tab pTab, bool pForce = false)
        {
            if (m_CurTap == pTab && !pForce)
                return;
            //HybirdAudioManager.Instance.PlaySFXById(IDs.SFX_SCREEN_SWITCH);
            m_CurTap = pTab;

            switch (pTab)
            {
                case Tab.Campain:
                    ShowCampainPanel();
                    objBg1.SetActive(false);
                    objBg2.SetActive(true);
                    break;

                case Tab.Talent:
                    ShowTalentPanel();
                    objBg1.SetActive(true);
                    objBg2.SetActive(false);
                    break;

                case Tab.Equipment:
                    ShowEquipmentPanel();
                    objBg1.SetActive(true);
                    objBg2.SetActive(false);
                    Data.GameData.Instance.TutorialGroup.SetPassTutorialSelectEquipment();
                    break;

                case Tab.Shop:
                    ShowShopPanel();
                    objBg1.SetActive(true);
                    objBg2.SetActive(false);
                    Data.GameData.Instance.TutorialGroup.SetPassTutorialSelectShop();
                    break;

                case Tab.Event:
                    //ShowEventPanel();
                    objBg1.SetActive(true);
                    objBg2.SetActive(false);
                    break;
            }
            //CheckNotifications();
        }

        public MessagesPopup ShowMessagePopup(MessagesPopup.Message pMessage)
        {
            PushPanelToTop(ref m_MessagePopup);
            m_MessagePopup. InitMessage(pMessage);
            return m_MessagePopup;
        }

        public Transform ShowMessageBubble(RectTransform pTarget, MessagesManager.Bubble pBubble)
        {
            if (m_MessagesManager.gameObject.IsPrefab())
                m_MessagesManager = Instantiate(m_MessagesManager, transform);

            m_MessagesManager.SetActive(true);
            var tran = m_MessagesManager.ShowMessageBubble(pTarget, pBubble);
            m_MessagesManager.transform.SetAsLastSibling();
            SimpleLeanFX.instance.Bubble(tran, Vector3.one, 0.25f);
            return tran;
        }

        public Transform ShowMessageBubble(RectTransform pTarget, string pMessage, Vector2 pSize, bool pBlockInput = false)
        {
            return ShowMessageBubble(pTarget, new MessagesManager.Bubble()
            {
                message = pMessage,
                size = pSize,
                blockInput = pBlockInput,
            });
        }

        public Transform ShowWarningBubble(RectTransform pTarget, MessagesManager.Bubble pBubble)
        {
            if (m_MessagesManager.gameObject.IsPrefab())
                m_MessagesManager = Instantiate(m_MessagesManager, transform);

            m_MessagesManager.SetActive(true);
            var tran = m_MessagesManager.ShowWarningBubble(pTarget, pBubble);
            m_MessagesManager.transform.SetAsLastSibling();
            SimpleLeanFX.instance.Bubble(tran, Vector3.one, 0.25f);
            return tran;
        }

        public Transform ShowWarningBubble(RectTransform pTarget, string pMessage, Vector2 pSize, bool pBlockInput = false)
        {
            return ShowWarningBubble(pTarget, new MessagesManager.Bubble()
            {
                message = pMessage,
                size = pSize,
                blockInput = pBlockInput,
            });
        }

        public void ShowToastMessage(string pMessage)
        {
            m_MessagesManager.SetActive(true);
            m_MessagesManager.transform.SetAsLastSibling();
            m_MessagesManager.ShowToastMessage(pMessage, 1f);
        }

        public MessageWithPointer ShowNotificationBoard(RectTransform pTarget, string pMessage, PointerAlignment pAlign, Vector2 pSize)
        {
            return ShowNotificationBoard(new MessagesManager.Notification(0)
            {
                target = pTarget,
                alignment = pAlign,
                message = pMessage,
                size = pSize
            });
        }

        public MessageWithPointer ShowNotificationBoard(MessagesManager.Notification pNotification)
        {
            if (m_MessagesManager.gameObject.IsPrefab())
                m_MessagesManager = Instantiate(m_MessagesManager, transform);
            m_MessagesManager.SetActive(true);
            m_MessagesManager.transform.SetAsLastSibling();
            var notiBoard = m_MessagesManager.ShowNotificationBoard(pNotification);
            SimpleLeanFX.instance.Bubble(notiBoard.transform, Vector3.one, 0.25f);
            return notiBoard;
        }

        public void HideNotificationBoard(int pId)
        {
            m_MessagesManager.HideNotificationBoard(pId);
        }


        //public void ShowDebugPanel()
        //{
        //    PushPanelToTop(ref m_HomeDebugPanel);
        //    m_HomeDebugPanel.Init();
        //}

        //public void ShowOfflineRewards(List<RewardInfo> pRewards)
        //{
        //    PushPanelToTop(ref m_OfflineRewardPopup);
        //    m_OfflineRewardPopup.Init(pRewards);
        //}

        //public void ShowOfflineRewardsOnlyBonus()
        //{
        //    PushPanelToTop(ref m_OfflineRewardPopup);
        //    m_OfflineRewardPopup.ShowOnlyBonusRewards();
        //}

        //public void ShowLevelUpBonusPopup(List<RewardInfo> pRewards)
        //{
        //    PushPanelToTop(ref m_LevelUpBonusPopup);
        //    m_LevelUpBonusPopup.Init(pRewards);
        //}
        //public void ShowDailyLoginRewardPopup(List<RewardInfo> pRewards)
        //{
        //    PushPanelToTop(ref m_DailyLoginRewardPopup);
        //    m_DailyLoginRewardPopup.Init(pRewards);
        //}
        #endregion

        //=====================================

        #region Private


        private void ShowCampainPanel()
        {
            m_HorizontalSnapScrollView.MoveToItem(Tab.Campain.GetHashCode(), false);
        }

        private void ShowTalentPanel()
        {
            m_HorizontalSnapScrollView.MoveToItem(Tab.Talent.GetHashCode(), false);
        }

        private void ShowEquipmentPanel()
        {
            m_HorizontalSnapScrollView.MoveToItem(Tab.Equipment.GetHashCode(), false);
        }

        private void ShowShopPanel()
        {
            m_HorizontalSnapScrollView.MoveToItem(Tab.Shop.GetHashCode(), false);
        }

        private void ShowAddBuildingPanel()
        {
            PushPanelToTop(ref m_UIBuildBuilding);
            m_UIBuildBuilding.Init();
            m_UIBuildBuilding.ShowAddBuild(0);
        }

        protected override void OnAnyChildHide(PanelController pPanel)
        {
            base.OnAnyChildHide(pPanel);

            if (TopPanel == null)
                PushPanelToTop(m_Container);

            if (TopPanel != null && TopPanel != m_Container)
            {
                btnBack.SetActive(true);
                btnBack.transform.SetParent(transform);
                btnBack.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }
            else
            {
                btnBack.SetActive(false);
                btnBack.transform.SetSiblingIndex(0);
            }

            onAnyChildHide?.Invoke(pPanel);
            //CheckNotifications();
        }

        protected override void OnAnyChildShow(PanelController pPanel)
        {
            base.OnAnyChildShow(pPanel);

            if (TopPanel != null && TopPanel != m_Container)
            {
                btnBack.SetActive(true);
                btnBack.transform.SetParent(transform);
                btnBack.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }
            else
            {
                btnBack.SetActive(false);
                btnBack.transform.SetSiblingIndex(0);
            }

            onAnyChildShow?.Invoke(pPanel);
            //CheckNotifications();
        }
        internal override void PushPanelToTop(PanelController panel)
        {
            base.PushPanelToTop(panel);
            //HybirdAudioManager.Instance.PlaySFXById(IDs.SFX_SCREEN_SWITCH);
        }
        #endregion
    }
}