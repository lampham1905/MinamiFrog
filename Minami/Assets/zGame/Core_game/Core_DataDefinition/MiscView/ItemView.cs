using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lam.zGame.Core_game.Core_DataDefinition.MiscView
{
    public class RewardInfo
    {
        public int type;
        public int id;
        public int value;
        public int rarity;

        public RewardInfo(int type, int id, int value = 1
            //, int rarity = IDs.RARITY_COMMON
        )
        {
            this.type = type;
            this.id = id;
            this.value = value;
            this.rarity = rarity;

            //if (type == IDs.REWARD_CURRENCY)
            //{
            //    if (id == IDs.CURRENCY_GOLD)
            //        this.rarity = IDs.RARITY_LEGENDARY;
            //    else if (id == IDs.CURRENCY_GEM)
            //        this.rarity = IDs.RARITY_EPIC;
            //    else if (id == IDs.CURRENCY_STAMINA)
            //        this.rarity = IDs.RARITY_GREAT;
            //}
            //else if (type == IDs.REWARD_OFFLINE_SECONDS)
            //{
            //    if (id == IDs.CURRENCY_GOLD)
            //        this.rarity = IDs.RARITY_LEGENDARY;
            //}
            //else if (type == IDs.REWARD_CHEST_KEY)
            //{
            //    if (id == IDs.CHEST_NORMAL)
            //        this.rarity = IDs.RARITY_GREAT;
            //    else if (id == IDs.CHEST_PREMIUM || id == IDs.CHEST_PREMIUM_X10)
            //        this.rarity = IDs.RARITY_RELIC;

            //}
        }

        public Sprite Icon
        {
            get
            {
                Sprite icon = null;
                //switch (type)
                //{
                //    case IDs.REWARD_CURRENCY:
                //        icon = GeneralAssets.Instance.currencyIcons[id - 1];
                //        break;
                //    case IDs.REWARD_EQUIPMENT:
                //        if (id < 0)
                //        {
                //            icon = GeneralAssets.Instance.commonIcons.GetAsset("random_epic");
                //        }
                //        else
                //        {
                //            icon = GeneralAssets.Instance.equipmentIcons[id - 1];
                //        }
                //        break;
                //    case IDs.REWARD_EQUIPMENT_EXOTIC:
                //        if (id < 0)
                //        {
                //            icon = GeneralAssets.Instance.commonIcons.GetAsset("random_exotic");
                //        }
                //        else
                //        {
                //            icon = GeneralAssets.Instance.equipmentIcons[id - 1];
                //        }
                //        break;
                //    case IDs.REWARD_EXP:
                //        icon = GeneralAssets.Instance.commonIcons.GetAsset("xp_icon");
                //        break;
                //    case IDs.REWARD_UNLOCK_FEATURE:
                //        break;
                //    case IDs.REWARD_UNLOCK_MAP:
                //        icon = GeneralAssets.Instance.mapIcons[id - 1];
                //        break;
                //    case IDs.REWARD_CHEST_KEY:
                //        icon = GeneralAssets.Instance.chestIcons[id - 1];
                //        break;
                //    case IDs.REWARD_OFFLINE_SECONDS:
                //        break;
                //}
                return icon;
            }
        }
    }

    public class ItemView : MonoBehaviour
    {
        public int id;
        public Image imgBg;
        public bool isChangeImgBg = true;
        public Image imgIcon;
        public ImageWithText iwtValue;
        public TextMeshProUGUI txtValue;
        public Vector2 maxBgSize;
        public float iconFixedRatio = 0.9f;

        public virtual void Init(RewardInfo pRewardInfo, int pId = 0, bool disnableValue = false)
        {
            id = pId;
            if (isChangeImgBg)
            {
                if (pRewardInfo.rarity > 0)
                {
                    imgBg.sprite = GeneralAssets.Instance.raritySlots[pRewardInfo.rarity - 1];
                }
                else
                {
                    //imgBg.sprite = GeneralAssets.Instance.raritySlots[IDs.RARITY_COMMON - 1];
                }
            }
            imgIcon.sprite = pRewardInfo.Icon;
            if (imgBg.rectTransform.sizeDelta != Vector2.zero)
            {
                imgIcon.SetNativeSize(imgBg.rectTransform.sizeDelta * iconFixedRatio);
            }
            else { 
                imgIcon.SetNativeSize(maxBgSize * iconFixedRatio);
            }
            if (iwtValue != null)
            {
                iwtValue.text = "" + pRewardInfo.value;
                if (pRewardInfo.rarity > 0)
                {
                    iwtValue.label.color = GeneralAssets.Instance.rarityDarkColors[pRewardInfo.rarity - 1];
                    iwtValue.image.color = GeneralAssets.Instance.rarityLightColors[pRewardInfo.rarity - 1];
                }
                else
                {
                    //iwtValue.label.color = GeneralAssets.Instance.rarityDarkColors[IDs.RARITY_COMMON - 1];
                    //iwtValue.image.color = GeneralAssets.Instance.rarityLightColors[IDs.RARITY_COMMON - 1];
                }
                iwtValue.gameObject.SetActive(!disnableValue);
            }
            if (txtValue != null)
            {
                txtValue.text = "" + pRewardInfo.value.ToString();
            }
        }

        public virtual void Init(Sprite pIcon, int pValue,
            //int pRarity = IDs.RARITY_COMMON,
            bool disnableValue = false)
        {
            if (isChangeImgBg)
            {
                //imgBg.sprite = GeneralAssets.Instance.raritySlots[pRarity - 1];
            }
            imgIcon.sprite = pIcon;
            if (imgBg.rectTransform.sizeDelta != Vector2.zero)
                imgIcon.SetNativeSize(imgBg.rectTransform.sizeDelta * iconFixedRatio);
            else
                imgIcon.SetNativeSize(maxBgSize * iconFixedRatio);
            if (iwtValue != null)
            {
                iwtValue.text = pValue.ToString();
                //iwtValue.label.color = GeneralAssets.Instance.rarityDarkColors[pRarity - 1];
                //iwtValue.image.color = GeneralAssets.Instance.rarityLightColors[pRarity - 1];
                iwtValue.gameObject.SetActive(!disnableValue);
            }
            if (txtValue != null)
            {
                txtValue.text = pValue.ToString();
            }
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (imgIcon == null || imgBg == null)
            {
                var imgs = gameObject.GetComponentsInChildren<Image>();
                if (imgs.Length >= 2)
                {
                    imgBg = imgs[0];
                    imgIcon = imgs[1];
                }
                else if (imgs.Length == 1)
                {
                    imgIcon = imgs[0];
                }
            }
            if (iwtValue == null)
                iwtValue = imgBg.GetComponentInChildren<ImageWithText>();

            if (imgBg.rectTransform.sizeDelta != Vector2.zero)
                maxBgSize = imgBg.rectTransform.sizeDelta;
            if (maxBgSize == Vector2.zero)
                maxBgSize = imgBg.sprite.NativeSize();
            if (imgIcon != null && maxBgSize != Vector2.zero)
                imgIcon.SetNativeSize(maxBgSize * iconFixedRatio);
        }
#endif
    }
}