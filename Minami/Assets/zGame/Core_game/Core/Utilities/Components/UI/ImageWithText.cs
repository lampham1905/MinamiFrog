

#pragma warning disable 0649

using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lam.zGame.Core_game.Core.Utilities.Components.UI
{
    [AddComponentMenu("Utitlies/UI/ImageWithText")]
    public class ImageWithText : MonoBehaviour
    {
        [SerializeField] protected Image mImg;
        [SerializeField] protected TextMeshProUGUI mTxt;

        [Separator("Custom")]
        [SerializeField] protected bool mAutoReize;
        [SerializeField] protected Vector2 mFixedSize;

        public Image image
        {
            get
            {
                if (mImg == null)
                    mImg = GetComponentInChildren<Image>();
                return mImg;
            }
        }
        public TextMeshProUGUI label
        {
            get
            {
                if (mTxt == null)
                    mTxt = GetComponentInChildren<TextMeshProUGUI>();
                return mTxt;
            }
        }
        public RectTransform rectTransform
        {
            get { return transform as RectTransform; }
        }
        public Sprite sprite
        {
            get { return image.sprite; }
            set
            {
                if (mImg.sprite != value)
                    SetSprite(value);
            }
        }
        public string text
        {
            get { return label.text; }
            set { label.text = value; }
        }

        public void SetSprite(Sprite pSprite)
        {
            image.sprite = pSprite;

            if (mAutoReize)
            {
                if (pSprite == null)
                    return;

                if (mFixedSize.x > 0 && mFixedSize.y > 0)
                {
                    image.SetNativeSize(mFixedSize);
                }
                else if (mFixedSize.x > 0)
                {
                    image.SketchByWidth(mFixedSize.x);
                }
                else if (mFixedSize.y > 0)
                {
                    image.SketchByWidth(mFixedSize.y);
                }
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        private void OnValidate()
        {
            Validate();
        }

        protected virtual void Validate()
        {
            if (mImg == null)
                mImg = GetComponentInChildren<Image>();
            if (mTxt == null)
                mTxt = GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ImageWithText), true)]
    public class ImageWithTextEditor : Editor
    {
        private ImageWithText mScript;

        private void OnEnable()
        {
            mScript = (ImageWithText)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorHelper.Button("Auto Reize"))
                mScript.SetSprite(mScript.image.sprite);
        }
    }
#endif
}
