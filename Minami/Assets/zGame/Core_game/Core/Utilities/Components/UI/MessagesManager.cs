
//#define NEW_INPUT

#if UNITY_EDITOR
#endif
#if NEW_INPUT
using UnityEngine.InputSystem;
#endif
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lam.zGame.Core_game.Core.Utilities.Components.UI
{
    /// <summary>
    /// A simple messages manager
    /// </summary>
    public class MessagesManager : MonoBehaviour
    {
        #region Internal class

        public class Message
        {
            public string message;
            public Vector2 size;
        }

        public class Bubble : Message
        {
            public bool blockInput = false;
            public float yOffset;
            public Bubble() { }
            public Bubble(string pMessage, Vector2 pSize, bool pBlockInput = false, float pYOffset = 0)
            {
                message = pMessage;
                size = pSize;
                blockInput = pBlockInput;
                yOffset = pYOffset;
            }
        }

        public class Notification : Message
        {
            public Notification(int pId) { id = pId; }
            public int id;
            public RectTransform target;
            public PointerAlignment alignment;
            public bool displayArrow = true;
            public bool displayMessage => !string.IsNullOrEmpty(message);
        }

        #endregion

        //=====================================

        #region Members

        [SerializeField] private bool mAutoDeactive = true;
        [SerializeField] private Image mImgBlock;
        /// <summary>
        /// Bubble message, it disaper when player tap outside message frame
        /// Display only one message in one moment
        /// </summary>
        [Header("Bubble message")]
        [SerializeField] private ImageWithText mIwtMessage;
        [SerializeField] private Color mWarningColor;
        /// <summary>
        /// This type of message used for notification, which mean it should not auto disapear when player tap any elsewhere
        /// Can display multiply messages in one momment 
        /// </summary>
        [Separator("Notifications")]
        [SerializeField] protected List<MessageWithPointer> mNotifications;
        [Separator("Quick Message")]
        [SerializeField] protected ImageWithText mIwtToastMessage;

        protected bool mInitialized;
        protected Camera mCamera;
#if NEW_INPUT
        protected CommonInputs mCommonInputs;
#endif
        private Vector2 mMousePosition;

        #endregion

        //=====================================

        #region MonoBehaviour

        private void OnEnable()
        {
#if NEW_INPUT
            if (mCommonInputs == null)
            {
                mCommonInputs = new CommonInputs();
                mCommonInputs.Inputs.GetScreenPoint.performed += OnGetScreenPoint;
            }
            mCommonInputs.Enable();
#endif
        }

        private void OnDisable()
        {
#if NEW_INPUT
            if (mCommonInputs != null)
                mCommonInputs.Disable();
#endif
        }

        protected void Start()
        {
            Init();
        }

#if !NEW_INPUT
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mMousePosition = Input.mousePosition;
                if (mIwtMessage.gameObject.activeSelf)
                    CheckClickOutBoard(mIwtMessage.rectTransform);
            }

            if (mAutoDeactive && !mIwtMessage.gameObject.activeSelf && !mIwtToastMessage.gameObject.activeSelf)
                enabled = false;
        }
#endif

        #endregion

        //=====================================

        #region Public

        public void Init()
        {
            mNotifications.Free();
            mIwtMessage.SetActive(false);
            mImgBlock.SetActive(false);
            mIwtToastMessage.SetActive(false);

            mInitialized = true;
            mCamera = Camera.main;
        }

        //=== BUBBLE

        public Transform ShowMessageBubble(RectTransform pTarget, Bubble pBubble)
        {
            if (!mInitialized) Init();
            mIwtMessage.SetActive(true);

            mIwtMessage.rectTransform.sizeDelta = pBubble.size;
            if (pTarget == null)
                mIwtMessage.rectTransform.localPosition = Vector3.zero;
            else
            {
                mIwtMessage.rectTransform.pivot = pTarget.pivot;
                mIwtMessage.rectTransform.position = pTarget.position;
                var anchoredPos = mIwtMessage.rectTransform.anchoredPosition;
                anchoredPos.y += mIwtMessage.rectTransform.rect.height / 2f - pBubble.yOffset;
                mIwtMessage.rectTransform.anchoredPosition = anchoredPos;

                //Check if message board is out of view
                var topRight = mIwtMessage.rectTransform.TopRight();
                var botLeft = mIwtMessage.rectTransform.BotLeft();
                float top = topRight.y;
                float bot = botLeft.y;
                float right = topRight.x;
                float left = botLeft.x;

                var mainRect = transform as RectTransform;
                topRight = mainRect.TopRight();
                botLeft = mainRect.BotLeft();
                float mainTop = topRight.y;
                float mainBot = botLeft.y;
                float mainRight = topRight.x;
                float mainLeft = botLeft.x;

                float xOffset = 0;
                float yOffset = 0;
                if (top > mainTop)
                    yOffset = top - mainTop;
                if (bot < mainBot)
                    yOffset = bot - mainBot;
                if (left < mainLeft)
                    xOffset = left - mainLeft;
                if (right > mainRight)
                    xOffset = right - mainRight;

                anchoredPos = mIwtMessage.rectTransform.anchoredPosition;
                anchoredPos.x -= xOffset;
                anchoredPos.y -= yOffset;
                mIwtMessage.rectTransform.anchoredPosition = anchoredPos;
            }

            mIwtMessage.label.text = pBubble.message;
            mIwtMessage.label.color = Color.white;
            mImgBlock.SetActive(pBubble.blockInput);
            enabled = true;

            mIwtMessage.transform.localScale = Vector3.one;
            return mIwtMessage.transform;
        }

        public Transform ShowWarningBubble(RectTransform pTarget, Bubble pBubble)
        {
            ShowMessageBubble(pTarget, pBubble);
            mIwtMessage.label.color = mWarningColor;
            return mIwtMessage.transform;
        }

        //=== NOTIFICATIONS

        public MessageWithPointer ShowNotificationBoard(Notification pNotification)
        {
            if (!mInitialized)
                Init();

            MessageWithPointer board = null;
            board = GetActiveNotification(pNotification.id);
            if (board == null)
                board = mNotifications.Obtain(transform);
            board.RectPointer.SetActive(pNotification.displayArrow);
            board.RectMessage.SetActive(pNotification.displayMessage);
            if (pNotification.displayArrow)
                board.PointToTarget(pNotification.target, pNotification.alignment);
            if (pNotification.displayMessage)
                board.MessageToTarget(pNotification.target, pNotification.message, pNotification.alignment, pNotification.size);
            board.id = pNotification.id;
            board.SetActive(true);
            board.transform.SetParent(pNotification.target);

            enabled = true;
            return board;
        }

        public void HideNotificationBoard(int pId)
        {
            var board = GetActiveNotification(pId);
            if (board != null)
                board.SetActive(false);
        }

        //=== QUICK MESSAGE

        public void ShowToastMessage(string pMessage, float pLifeTime)
        {
            mIwtToastMessage.label.text = pMessage;
            mIwtToastMessage.SetActive(true);
            mIwtToastMessage.gameObject.TryGetComponent(out Animation anim);
            if (anim != null)
            {
                anim.Stop();
                anim.Play();
                pLifeTime = anim.clip.length;
            }

            enabled = true;
            WaitUtil.RemoveCountdownEvent(mIwtMessage.GetInstanceID());
            WaitUtil.Start(pLifeTime, (s) =>
            {
                try
                {
                    mIwtToastMessage.SetActive(false);
                }
                catch { }
            }).SetId(mIwtMessage.GetInstanceID());
        }

        #endregion

        //=====================================

        #region Private

        protected MessageWithPointer GetActiveNotification(int pId)
        {
            for (int i = 0; i < mNotifications.Count; i++)
            {
                if (mNotifications[i].gameObject.activeSelf && mNotifications[i].id == pId)
                    return mNotifications[i];
            }
            return null;
        }

        protected void CheckClickOutBoard(params RectTransform[] boards)
        {
            bool valid = false;
            for (int i = 0; i < boards.Length; i++)
            {
                var screenPoint = mCamera.ScreenToWorldPoint(mMousePosition);
                var inBoard = RectTransformUtility.RectangleContainsScreenPoint(boards[i], screenPoint);
                if (inBoard && !valid)
                    valid = true;
            }
            if (!valid)
                for (int i = 0; i < boards.Length; i++)
                    boards[i].SetActive(false);
        }

#if NEW_INPUT
        private void OnGetScreenPoint(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                mMousePosition = mCommonInputs.Inputs.ScreenPoint.ReadValue<Vector2>();
                if (mIwtMessage.gameObject.activeSelf)
                    CheckClickOutBoard(mIwtMessage.rectTransform);
            }
        }
#endif

        #endregion

        //=====================================

        #region Editor

#if UNITY_EDITOR

        [CustomEditor(typeof(MessagesManager))]
        private class MessagesManagerEditor : Editor
        {
            private MessagesManager mScript;
            private RectTransform mTestTarget;

            private void OnEnable()
            {
                mScript = target as MessagesManager;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                mTestTarget = (RectTransform)EditorHelper.ObjectField<RectTransform>(mTestTarget, "Test target", 100);

                if (GUILayout.Button("Show Bubble Message"))
                    if (mTestTarget != null)
                        mScript.ShowMessageBubble(mTestTarget, new Bubble()
                        {
                            message = "This is a test messsage",
                            size = new Vector2(600, 200)
                        });

                if (GUILayout.Button("Show Notification Message"))
                    mScript.ShowNotificationBoard(new Notification(0)
                    {
                        target = mTestTarget,
                        message = "This is a test messsage",
                        alignment = PointerAlignment.TopRight,
                        size = new Vector2(600, 200)
                    });

                if (GUILayout.Button("Show Toast Message"))
                    mScript.ShowToastMessage("Test Message", 1f);
            }
        }

#endif

        #endregion
    }
}