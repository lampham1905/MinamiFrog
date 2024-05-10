using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using ntDev;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lam
{
    public class UIUpgradeShopPiece : PanelController
    {
        public IBuilding building;
        [FormerlySerializedAs("uiUpgradeCustomize")] public UIUpgradeShop uiUpgradeShop;
        [SerializeField] private JustButton btnUpgrade;
        [FormerlySerializedAs("btnCustomize")] [SerializeField] private JustButton btnShop;

        private void Start()
        {
            btnShop.onClick.AddListener(() =>
            {
                ManagerEvent.RaiseEvent(EventCMD.SHOWSHOP, building.buildingData);
            });
            btnUpgrade.onClick.AddListener(() =>
            {
                ManagerEvent.RaiseEvent(EventCMD.SHOWUPGRADE, building.buildingData);
            });
        }

        public void SetInit(bool isUpgrade, bool isCustomize)
        {
            btnUpgrade.gameObject.SetActive(!isUpgrade);
            btnShop.gameObject.SetActive(isCustomize);
        }
        
    }
}
