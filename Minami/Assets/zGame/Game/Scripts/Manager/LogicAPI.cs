using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lam
{
    public class LogicAPI 
    {
        public static float  GetTimeMove(float s, float v, float tJump, float tIdle)
        {
            float t = (s / v) + ((s / (v * tJump)-1) * tIdle);
            return t;
        }

        public static float GetTimePer5Minute(int amountBulidng)
        {
            float t = (amountBulidng * GameConfig.TIME_A_DAY_PER_BUILDING) / 168;
            return t;
        }

        public static float GetTimeADay(int amountBulidng)
        {
            float t = amountBulidng * GameConfig.TIME_A_DAY_PER_BUILDING;
            return t;
        }

        public static List<int> GetRandomPerfectRecipeShop1(List<int> recipeInit, List<int> listRandom)
        {
            List<int> listRecipePefect = new List<int>();
            for (int i = 0; i < recipeInit.Count; i++)
            {
                listRecipePefect.Add(recipeInit[i]);
                Debug.Log("add" + recipeInit[i]);
            }
            List<int> listInt = new List<int>() { 0, 1, 2 };
            int random1 = Random.Range(0, 3);
            listRecipePefect[listInt[random1]] += 1;
            listInt.Remove(random1);
            int random2 = Random.Range(0, 2);
            listRecipePefect[listInt[random2]] += 2;
            int random3 = Random.Range(0, listRandom.Count);
            listRecipePefect[3] = listRandom[random3];
            return listRecipePefect;
        }

        public static List<int> GetRandomPerfectProfitShop1(List<int> list)
        {
            int random = Random.Range(0, list.Count);
            Debug.Log(list.Count);
            List<int> res = new List<int>();
            res.Add(list[random]);
            return res;
        }

        public static List<int> GetRandomPerfectRecipeShop2()
        {
            List<int> res = new List<int>();
            List<int> listTemp = new List<int>() {0, 1, 2, 3, 4, 5, 6};
            for (int i = 0; i < 3; i++)
            {
                int random = Random.Range(0, listTemp.Count);
                res.Add(listTemp[random]);
                listTemp.Remove(random);
            }
            return res;
        }

        public static List<int> GetRandomPerfectProfitShop2(List<int> listRecipe, List<int> listRandom)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < listRecipe.Count; i++)
            {
                int random = Random.Range(0, 2);
                res.Add(listRandom[listRecipe[i] + random]);
            }
            return res;
        }
    }
}
