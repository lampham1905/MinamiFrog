using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using UnityEngine;

namespace Lam
{
    public class UIGamePlay : PanelController
    {
        [Separator("Header")]
        [SerializeField] private JustButton btnAddBuilding;
        
        [Separator("Panels")]
        [SerializeField] private UIBuildBuilding m_UIBuildBuilding;
        public static UIGamePlay m_Instance;
        public static UIGamePlay Instance => m_Instance;
        protected override void Awake()
        {
            base.Awake();

            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }
       
        private void Start()
        {
            Init();
        }
        internal override void Init()
        {
            base.Init();
            StartCoroutine(PostInit());
        }
        private IEnumerator PostInit()
        {
            
            btnAddBuilding.onClick.AddListener(ShowAddBuildingPanel);
      

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
        private void ShowAddBuildingPanel()
        {
            PushPanelToTop(ref m_UIBuildBuilding);
            m_UIBuildBuilding.Init();
            m_UIBuildBuilding.ShowAddBuild(0);
        }
    }
}
