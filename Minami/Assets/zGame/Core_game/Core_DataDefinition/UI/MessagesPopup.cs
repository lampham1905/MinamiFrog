using Lam.zGame.Core_game.Core_DataDefinition.Data;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Components.Audio;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Lam.zGame.Core_game.Core_DataDefinition.UI
{
    public class MessagesPopup : PanelController
    {
        public enum Choice
        {
            Ignore,
            Yes,
            No,
        }

        public class Message
        {
            public string title;
            public string content;
            public UnityAction yesAction;
            public UnityAction noAction;
            public UnityAction additionalAction;
            /// <summary>
            /// Text on yest button
            /// </summary>
            public string yesActionLabel;
            /// <summary>
            /// Text on No button
            /// </summary>
            public string noActionLabel;
            /// <summary>
            /// Text on anditional choice button
            /// </summary>
            public string additionalActionLabel;
            /// <summary>
            /// Size of popup
            /// </summary>
            public Vector2 popupSize;
            /// <summary>
            /// Can ignore popup by quit by X button
            /// </summary>
            public bool allowIgnore;
            /// <summary>
            /// Do not allow any external interfere which close popup during show
            /// </summary>
            public bool lockPopup;
            public TextAlignmentOptions contentAignment = TextAlignmentOptions.Center;

            public Message() { }

            public Message(string title, string content, UnityAction pYesAction, UnityAction pNoAction)
            {
                yesAction = pYesAction;
                noAction = pNoAction;
            }

            public Message(string title, UnityAction pYesAction, UnityAction pNoAction)
            {
                yesAction = pYesAction;
                noAction = pNoAction;
            }

            public Message(UnityAction pYesAction, UnityAction pNoAction)
            {
                yesAction = pYesAction;
                noAction = pNoAction;
            }
        }

        #region Members

#pragma warning disable 0649
        [SerializeField] private RectTransform mMainRect;
        [SerializeField] private TextMeshProUGUI mTxtTitle;
        [SerializeField] private TextMeshProUGUI mTxtContent;
        [SerializeField] private SimpleTMPButton mBtnYes;
        [SerializeField] private SimpleTMPButton mBtnNo;
        [SerializeField] private SimpleTMPButton mBtnAdditionalDo;
        [SerializeField] private Vector2 mDefaultSize;
#pragma warning restore 0649

        private Choice mCurrentChoice;
        private Message mMessage;

        public static Message LockFeatureMessage
        {
            get
            {
                return new Message()
                {
                    yesAction = () => { },
                    noAction = null,
                    additionalAction = null,
                    yesActionLabel = "OK",
                    allowIgnore = true,
                    title = "MESSAGE",
                    content = "THIS FEATURE IS NOT UNLOCKED!",
                    popupSize = new Vector2(800, 400),
                    contentAignment = TextAlignmentOptions.Center
                };
            }
        }

        #endregion

        //=============================================

        #region MonoBehaviour

        private void OnEnable()
        {
            mCurrentChoice = Choice.Ignore;

            HybirdAudioManager.Instance?.SetMusicVolume(0.1f, 0.5f);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            HybirdAudioManager.Instance?.SetMusicVolume(UnmanagedData.MusicVolume, 1.5f);
        }

        #endregion

        //=============================================

        #region Public

        public void InitMessage(Message pMessage)
        {
            mMessage = pMessage;
            if (!string.IsNullOrEmpty(mMessage.title)) mTxtTitle.text = mMessage.title;
            if (!string.IsNullOrEmpty(mMessage.content)) mTxtContent.text = mMessage.content;
            mTxtContent.alignment = pMessage.contentAignment;
            if (mBtnYes != null)
            {
                mBtnYes.onClick.RemoveAllListeners();
                mBtnYes.SetActive(mMessage.yesAction != null);
                if (mMessage.yesAction != null)
                {
                    mBtnYes.onClick.AddListener(OnBtnYes_Pressed);
                    mBtnYes.labelTMP.text = string.IsNullOrEmpty(mMessage.yesActionLabel) ? "YES" : mMessage.yesActionLabel;
                }
            }
            if (mBtnNo != null)
            {
                mBtnNo.onClick.RemoveAllListeners();
                mBtnNo.SetActive(mMessage.noAction != null);
                if (mMessage.noAction != null)
                {
                    mBtnNo.onClick.AddListener(OnBtnNo_Pressed);
                    mBtnNo.labelTMP.text = string.IsNullOrEmpty(mMessage.noActionLabel) ? "NO" : mMessage.noActionLabel;
                }
            }
            if (mBtnAdditionalDo != null)
            {
                mBtnAdditionalDo.onClick.RemoveAllListeners();
                mBtnAdditionalDo.SetActive(mMessage.additionalAction != null);
                if (mMessage.additionalAction != null)
                {
                    mBtnAdditionalDo.onClick.AddListener(OnBtnAdditionalDo_Pressed);
                    mBtnAdditionalDo.labelTMP.text = string.IsNullOrEmpty(mMessage.additionalActionLabel) ? "DO SOMETHING" : mMessage.additionalActionLabel;
                }
            }
            if (mMessage.popupSize != Vector2.zero)
                mMainRect.sizeDelta = mMessage.popupSize;
            else
                mMainRect.sizeDelta = mDefaultSize;
            if (btnBack != null)
                btnBack.SetActive(mMessage.allowIgnore);

            Lock(mMessage.lockPopup);
        }

        #endregion

        //==============================================

        #region Private

        protected override void AfterHiding()
        {
            base.AfterHiding();

            if (mCurrentChoice == Choice.Yes)
                mMessage.yesAction.Raise();
            else if (mCurrentChoice == Choice.No)
                mMessage.noAction.Raise();

            mMessage = new Message();
        }

        private void OnBtnNo_Pressed()
        {
            mCurrentChoice = Choice.No;
            Lock(false);

            base.Back();
        }

        private void OnBtnYes_Pressed()
        {
            mCurrentChoice = Choice.Yes;
            Lock(false);

            base.Back();
        }

        internal override void Back()
        {
            if (mMessage.allowIgnore)
            {
                mCurrentChoice = Choice.Ignore;
                Lock(false);

                base.Back();
            }
        }

        private void OnBtnAdditionalDo_Pressed()
        {
            mMessage.additionalAction();
        }

        #endregion
    }
}
