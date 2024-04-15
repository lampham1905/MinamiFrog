using System.Collections;
using Lam.zGame.Core_game.Core_DataDefinition.Misc;
using Lam.zGame.Core_game.Core.Config;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lam.zGame.Core_game.Core_DataDefinition.UI
{
    public class SystemPanel : PanelController
    {
        #region Members

        private static SystemPanel mInstance;
        public static SystemPanel Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<SystemPanel>();
                return mInstance;
            }
        }

        [SerializeField] private MessagesPopup mCrashPopup;
        [SerializeField] private TextMeshProUGUI mTxtFPS;
        [SerializeField] private Image mImgDarkLayer;

        #endregion

        //=============================================

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        private IEnumerator Start()
        {
            mTxtFPS.transform.parent.SetActive(DevSetting.Instance.showFPS);
            yield return new WaitForSeconds(1f);
            Application.logMessageReceived += OnLogMessageReceived;
            DevSetting.Instance.onSettingsChanged += OnSettingsChanged;
            mTxtFPS.transform.parent.SetActive(DevSetting.Instance.showFPS);
            enabled = DevSetting.Instance.showFPS;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            DevSetting.Instance.onSettingsChanged -= OnSettingsChanged;
        }

        private void Update()
        {
            if (FPS.Instance!=null&&FPS.Instance.updated)
                mTxtFPS.text = $"FPS:{FPS.Instance.fps}";
        }

        #endregion

        //=============================================

        #region Public

        public void ShowCrashPopup(MessagesPopup.Message pMessage)
        {
            PushPanelToTop(ref mCrashPopup);
            mCrashPopup.InitMessage(pMessage);
        }

        #endregion

        //==============================================

        #region Private

        protected override void OnAnyChildHide(PanelController pPanel)
        {
            base.OnAnyChildHide(pPanel);

            if (TopPanel != null)
            {
                mImgDarkLayer.SetActive(true);
                mImgDarkLayer.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }
            else
            {
                mImgDarkLayer.SetActive(false);
                mImgDarkLayer.transform.SetAsFirstSibling();
            }
        }

        protected override void OnAnyChildShow(PanelController pPanel)
        {
            base.OnAnyChildShow(pPanel);

            if (TopPanel != null)
            {
                mImgDarkLayer.SetActive(true);
                mImgDarkLayer.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }
            else
            {
                mImgDarkLayer.SetActive(false);
                mImgDarkLayer.transform.SetAsFirstSibling();
            }
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Exception)
                return;

            string crashReport = string.Format("LOG ENTRY: {0}\n{1}", condition, stackTrace);

            ShowCrashPopup(new MessagesPopup.Message()
            {
                title = "FATAL ERROR",
                content = crashReport,
                yesAction = () =>
                {
                    Application.Quit();
                },
                noAction = null,
                additionalAction = () =>
                {
                    //string userId = GameInitializer.Instance.LoggedUserId;
                    string userId = "";
                    string subject = EmailHelper.CreateEmailReportSubject();
                    string content = EmailHelper.CreateEmailReportContent(stackTrace, userId);
                    EmailHelper.SendEmailByDefaultApp(subject, content, DevSetting.Instance.crashEmail, null);
                },
                yesActionLabel = "QUIT",
                noActionLabel = "IGNORE",
                additionalActionLabel = "EMAIL US",
                allowIgnore = false,
                lockPopup = true,
                popupSize = new Vector2(500, 600),
                contentAignment = TextAlignmentOptions.TopLeft
            });
        }

        private void OnSettingsChanged()
        {
            mTxtFPS.transform.parent.SetActive(DevSetting.Instance.showFPS);
            if (DevSetting.Instance.showFPS)
                enabled = true;
            else
                enabled = false;
        }

        #endregion
    }
}
