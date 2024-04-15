using System;
using UnityEngine;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] public EasyButton[] btnClose;

        bool init = false;
        public virtual void Init()
        {
            if (!init)
            {
                init = true;
                foreach (EasyButton btn in btnClose)
                    btn.OnClick(Hide);
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
        }
    }
}