using System.Collections;
using System.Collections.Generic;
using Lam.Scripts.GameManager;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using UnityEngine;

namespace Lam
{
    public class UIUpgradeShop : PoolManager
    {
        [SerializeField] private GameObject uiPiece;
        private List<UIUpgradeShopPiece> _mListUIUpgradeShopPieces = new List<UIUpgradeShopPiece>();
        public List<UIUpgradeShopPiece> ListUIUpgradeShopPieces => _mListUIUpgradeShopPieces;
        [SerializeField] private Camera _camera;
        public void Init()
        {
            SetUpInit(uiPiece, 10,this.transform.parent);
        }

        public void GetUIPiece(IBuilding building)
        {
            GameObject g = getPollObject(uiPiece);
            if (g.TryGetComponent<UIUpgradeShopPiece>(out UIUpgradeShopPiece ui))
            {
                ui.building = building;
                ui.uiUpgradeShop = this;
                ui.SetInit(building.isUpgrade, building.isShop);
            }
            _mListUIUpgradeShopPieces.Add(ui);
            g.transform.position = (building.transform.position) + new Vector3(-0.5f ,2f, -1.5f);
            g.gameObject.SetActive(true);
        }
    }
}
