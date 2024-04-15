#if USE_DOTWEEN
using DG.Tweening;
#endif
using Lam.zGame.Core_game.Core_DataDefinition.Misc;
using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lam.zGame.Core_game.Core_DataDefinition.MiscView
{
    public class CurrencyView : MonoBehaviour
    {
        #region Members

#pragma warning disable 0649
        [SerializeField] private int mCurrencyId;
        [SerializeField] private Image mImgIcon;
        [SerializeField] private TextMeshProUGUI mTxtValue;
        [SerializeField] private TextMeshProUGUI mFallValue;
        [SerializeField] private JustButton mBtnShortcut;
        [SerializeField] private Animation mAnimFall;
#pragma warning restore 0649

        public Image ImgIcon => mImgIcon;


        private bool mRegisteredEvents;

        #endregion

        //=====================================

        #region MonoBehaviour

        private void Start()
        {
            if (mBtnShortcut != null)
                mBtnShortcut.onClick.AddListener(BtnShortCut_Pressed);
        }

        private void OnEnable()
        {
            if (Data.GameData.Instance == null || Data.GameData.Instance.CurrenciesGroup == null)
                return;

            RegisterEvents();
            Refresh();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (mImgIcon == null)
                mImgIcon = GetComponentInChildren<Image>();
            if (mImgIcon != null && mCurrencyId > 0)
                mImgIcon.sprite = GeneralAssets.Instance.currencyIcons[mCurrencyId - 1];
            if (mTxtValue == null)
                mTxtValue = GetComponentInChildren<TextMeshProUGUI>();
            if (mBtnShortcut == null)
                mBtnShortcut = GetComponentInChildren<JustButton>();
        }
#endif

        #endregion

        //=====================================

        #region Public

        public void Init(int pId)
        {
            mCurrencyId = pId;
            mImgIcon.sprite = GeneralAssets.Instance.currencyIcons[pId - 1];

            Refresh();
            RegisterEvents();
        }

        public void SetValue(int pValue)
        {
            mTxtValue.text = pValue.ToString();
        }

        public void SetValue(int pFrom, int pTo, float pDuration)
        {
            int val = pFrom;
            mTxtValue.text = val.ToString();
#if USE_DOTWEEN
            DOTween.To(() => val, x => val = x, pTo, pDuration)
                .OnUpdate(() =>
                {
                    mTxtValue.text = val.ToString();
                })
                .OnComplete(() =>
                {
                    mTxtValue.text = pTo.ToString();
                });
#else
            mTxtValue.text = pTo.ToString();
#endif
        }

        public void FallValue(int value)
        {
            mFallValue.text = string.Format("-{0}", value);
            if (mAnimFall != null) mAnimFall.Play();   
        }    

        #endregion

        //=====================================

        #region Private

        private void RegisterEvents()
        {
            if (mRegisteredEvents || mCurrencyId == -1)
                return;

            mRegisteredEvents = true;

            EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void UnregisterEvents()
        {
            if (!mRegisteredEvents)
                return;

            mRegisteredEvents = false;

            EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void Refresh()
        {
            var currenciesGroup = Data.GameData.Instance.CurrenciesGroup;
            switch (mCurrencyId)
            {
                //case IDs.CURRENCY_GEM:
                //    mTxtValue.text = currenciesGroup.TotalGem().ToString();
                //    break;
                //case IDs.CURRENCY_GOLD:
                //    mTxtValue.text = currenciesGroup.TotalGold().ToString();
                //    break;
                //case IDs.CURRENCY_STAMINA:
                //    mTxtValue.text = $"{currenciesGroup.TotalStamina()}/{Constants.MAX_STAMINA}";
                //    break;
            }
        }

        private void BtnShortCut_Pressed()
        {
            //Open shop or do something
            //if (MainMenuPanel.Instance != null)
            //{
            //    MainMenuPanel.Instance.SwitchTabShortcut(MainMenuPanel.Tab.Shop);
            //    //MainMenuPanel.Instance.SwitchTab(MainMenuPanel.Tab.Shop);
            //}
        }

        private void OnCurrencyChanged(CurrencyChangedEvent e)
        {
            if (e.id == mCurrencyId)
                Refresh();
        }

        #endregion
    }
}
