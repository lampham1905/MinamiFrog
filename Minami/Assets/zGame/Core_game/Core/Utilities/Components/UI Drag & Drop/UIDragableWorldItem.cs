using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lam.zGame.Core_game.Core.Utilities.Components.UI_Drag___Drop
{
    /// <summary>
    /// Drag an item from UI-Canvas to world
    /// </summary>
    public class UIDragableWorldItem : UIDragableItem
    {
        public GameObject worldObjPrefab;
        protected GameObject mWorldObj;

        protected override void Awake()
        {
            base.Awake();

            GetDragHandler().dontShowItem = true;
        }

        public override void BeginDrag(PointerEventData eventData)
        {
            base.BeginDrag(eventData);

            GetWorldItem().SetActive(true);
            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        public override void Drag(PointerEventData eventData)
        {
            base.Drag(eventData);

            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        public override void EndDrag(PointerEventData eventData)
        {
            base.EndDrag(eventData);

            GetWorldItem().SetActive(false);
            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        protected GameObject GetWorldItem()
        {
            if (mWorldObj == null)
                mWorldObj = Instantiate(worldObjPrefab);

            return mWorldObj;
        }
    }
}