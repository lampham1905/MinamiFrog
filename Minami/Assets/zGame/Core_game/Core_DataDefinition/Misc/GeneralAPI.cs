using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.MiscView;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    public class GeneralAPI : MonoBehaviour
    {
        public static Data.GameData GameData => Data.GameData.Instance;

        public static List<RewardInfo> ClaimReward(RewardInfo pReward)
        {
            var list = new List<RewardInfo>();
            switch (pReward.type)
            {
                //case IDs.REWARD_CURRENCY:
                //    //----
                //    if (pReward.id == IDs.MATERIAL_RANDOM)
                //    {
                //        //int[] arrSLPart = ConfigMethod.GetListChiaRd(pReward.value, 3);//FIXME tam thoi bo drone
                //        int[] arrSLPart = ConfigMethod.GetListChiaDeuRd(pReward.value, 3);
                //        for (int i = 0; i < 3; i++)
                //        {
                //            if (arrSLPart[i] > 0)
                //            {
                //                switch (i)
                //                {
                //                    case 0:
                //                        RewardInfo rewardPart = new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_ARMOR_PART, arrSLPart[i]);
                //                        GameData.CurrenciesGroup.Add(rewardPart.id, rewardPart.value);
                //                        list.Add(rewardPart);
                //                        break;
                //                    case 1:
                //                        RewardInfo rewardPart1 = new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_ARMOR, arrSLPart[i]);
                //                        GameData.CurrenciesGroup.Add(rewardPart1.id, rewardPart1.value);
                //                        list.Add(rewardPart1);
                //                        break;
                //                    case 2:
                //                        RewardInfo rewardPart2 = new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_WEAPON, arrSLPart[i]);
                //                        GameData.CurrenciesGroup.Add(rewardPart2.id, rewardPart2.value);
                //                        list.Add(rewardPart2);
                //                        break;
                //                    case 3:
                //                        RewardInfo rewardPart3 = new RewardInfo(IDs.REWARD_CURRENCY, IDs.MATERIAL_HERO, arrSLPart[i]);
                //                        GameData.CurrenciesGroup.Add(rewardPart3.id, rewardPart3.value);
                //                        list.Add(rewardPart3);
                //                        break;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //---
                //        GameData.CurrenciesGroup.Add(pReward.id, pReward.value);
                //        list.Add(pReward);
                //    }
                //    break;

                //case IDs.REWARD_EQUIPMENT:
                //    if (pReward.id <= 0)
                //    {
                //        int rarity = pReward.rarity;
                //        int typeExotic = IDs.TYPE_GET_EXOTIC_NOT;
                //        switch (rarity)
                //        {
                //            case IDs.RARITY_EPIC:
                //            case IDs.RARITY_LEGENDARY:
                //            case IDs.RARITY_RELIC:
                //                int indexRd = Random.Range(0,100);
                //                if (indexRd < Constants.RATE_EPIC_EXOTIC_RANDOM) {
                //                    typeExotic = IDs.TYPE_GET_EXOTIC_ONLY;
                //                }
                //                break;
                //        }
                //        var randomEQ = BuiltinData.Instance.GetEquipment(0, typeExotic);
                //        GameData.EquipmentsGroup.CreateNewEquipmentItemAndAddToInventory(randomEQ, rarity, 1);
                //        pReward.id = randomEQ.id;
                //    }
                //    else
                //    {
                //        int rarity = pReward.rarity;
                //        GameData.EquipmentsGroup.CreateNewEquipmentItemAndAddToInventory(pReward.id, rarity, 1);
                //    }
                //    list.Add(pReward);
                //    break;
                //case IDs.REWARD_EQUIPMENT_EXOTIC:
                //    if (pReward.id <= 0)
                //    {
                //        var randomEQ = BuiltinData.Instance.GetEquipment(0, IDs.TYPE_GET_EXOTIC_ONLY);
                //        int rarity = pReward.rarity;
                //        GameData.EquipmentsGroup.CreateNewEquipmentItemAndAddToInventory(randomEQ, rarity, 1);
                //        pReward.id = randomEQ.id;
                //    }
                //    else
                //    {
                //        int rarity = pReward.rarity;
                //        GameData.EquipmentsGroup.CreateNewEquipmentItemAndAddToInventory(pReward.id, rarity, 1);
                //    }
                //    pReward.type = IDs.REWARD_EQUIPMENT;
                //    list.Add(pReward);
                //    break;

                //case IDs.REWARD_EXP:
                //    GameData.MiscGroup.AddExp(pReward.value);
                //    list.Add(pReward);
                //    break;

                //case IDs.REWARD_UNLOCK_FEATURE:
                //    list.Add(pReward);
                //    break;

                //case IDs.REWARD_UNLOCK_MAP:
                //    list.Add(pReward);
                //    break;

                //case IDs.REWARD_CHEST_KEY:
                //    var chest = GameData.ShopItemsGroup.GetChest(pReward.id);
                //    if (chest != null)
                //        chest.AddToStock(pReward.value);
                //    list.Add(pReward);
                //    break;

                //case IDs.REWARD_OFFLINE_SECONDS:
                //    var rewards = ClaimReward(ConvertOfflineRewardToReward(pReward));
                //    list.AddRange(rewards);
                //    break;
            }
            return list;
        }

        public static List<RewardInfo> ClaimRewards(List<RewardInfo> pRewards)
        {
            var list = new List<RewardInfo>();
            foreach (var r in pRewards)
                list.AddRange(ClaimReward(r));
            return list;
        }

        public static RewardInfo ConvertOfflineRewardToReward(RewardInfo pReward)
        {
            //var mods = GameData.TalentsGroup.GetMods();
            //var stats = new TotalStats();
            //stats.AddMods(mods);

            //if (stats.coinOffline == 0)
            //    stats.coinOffline = Constants.OFFLINE_COIN_TEMP;

            //switch (pReward.id)
            //{
            //    case IDs.CURRENCY_GOLD:
            //        pReward.type = IDs.REWARD_CURRENCY;
            //        pReward.value = Mathf.RoundToInt(pReward.value / Constants.OFFLINE_STEP_SECONDS * stats.coinOffline);
            //        break;

            //    case IDs.MATERIAL_ARMOR:
            //    case IDs.MATERIAL_ARMOR_PART:
            //    case IDs.MATERIAL_DRONE:
            //    case IDs.MATERIAL_WEAPON:
            //        pReward.type = IDs.REWARD_CURRENCY;
            //        pReward.value = Mathf.RoundToInt(pReward.value / Constants.OFFLINE_STEP_SECONDS * stats.coinOffline / 520f);
            //        break;
            //}
            return pReward;
        }
    }
}
