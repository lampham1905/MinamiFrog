using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

    namespace Lam.zGame.Core_game.Core_DataDefinition.Manager
    {
        public static class LogicAPI
        {
            public static List<int> CalcRandomWithChancesNotDuplicate(List<int> chances, int amount)
            {
                List<int> listIndexChances = null;
                List<int> listGet = null;
                int length = chances.Count;
                if (length > 0)
                {
                    if (amount >= length)
                    {
                        //no la ca cai list do luon
                        listGet = new List<int>();
                        for (int i = 0; i < length; i++)
                        {
                            listGet.Add(i);
                        }
                    }
                    else
                    {
                        listGet = new List<int>();
                        listIndexChances = new List<int>();
                        int totalRatios = 0;
                        for (int j = 0; j < length; j++)
                        {
                            totalRatios += chances[j];
                            listIndexChances.Add(j);
                        }
                        for (int i = 0; i < amount; i++)
                        {
                            int index = 0;
                            float random = Random.Range(0, totalRatios);
                            float temp2 = 0;
                            length = listIndexChances.Count;
                            for (int j = 0; j < length; j++)
                            {
                                temp2 += chances[listIndexChances[j]];
                                if (temp2 > random)
                                {
                                    index = j;
                                    break;
                                }
                            }
                            listGet.Add(listIndexChances[index]);

                            totalRatios -= chances[listIndexChances[index]];
                            listIndexChances.RemoveAt(index);
                        }
                    }

                }
                return listGet;
            }
            public static int CalcRandomWithChances(List<int> chances)
            {
                int index = 0;
                float totalRatios = 0;
                for (int i = 0; i < chances.Count; i++)
                    totalRatios += chances[i];
                if (totalRatios > 0)
                {
                    float random = Random.Range(0, totalRatios);
                    float temp2 = 0;
                    for (int i = 0; i < chances.Count; i++)
                    {
                        temp2 += chances[i];
                        if (temp2 > random)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                else
                {
                    index = -1;
                }
                return index;
            }
            public static int CalcRandomWithChances(List<float> chances)
            {
                int index = 0;
                float totalRatios = 0;
                for (int i = 0; i < chances.Count; i++)
                    totalRatios += chances[i];
                if (totalRatios > 0)
                {
                    float random = Random.Range(0, totalRatios);
                    float temp2 = 0;
                    for (int i = 0; i < chances.Count; i++)
                    {
                        temp2 += chances[i];
                        if (temp2 > random)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                else
                {
                    index = -1;
                }
                return index;
            }
            public static int CalcRandomWithChances(float[] chances)
            {
                int index = 0;
                float totalRatios = 0;
                for (int i = 0; i < chances.Length; i++)
                    totalRatios += chances[i];
                if (totalRatios > 0)
                {
                    float random = Random.Range(0, totalRatios);
                    float temp2 = 0;
                    for (int i = 0; i < chances.Length; i++)
                    {
                        temp2 += chances[i];
                        if (temp2 > random)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                else
                {
                    index = -1;
                }
                return index;
            }
            //enemy
            public static int GetEnemyHP(int baseHP)
            {
                float bonusHpEnemy =0.2f;
                return Mathf.FloorToInt((float)((bonusHpEnemy) * baseHP));
            }

            public static int GetEnemyAtk(int baseAtk)
            {
                float bonusAtkEnemy = 0.2f;
                return Mathf.FloorToInt((float)((bonusAtkEnemy) * baseAtk));
            }
            public static float GetHeroHP(float baseHP, int level, int rank)
            {
                return baseHP + (level - 1) * (0.05f * baseHP) + (rank - 1) * (GetHeroLevelMax(rank) - 1) * (0.05f * baseHP);
            }

            public static float GetHeroAtk(float baseAtk, int level, int rank)
            {
                return baseAtk + (level - 1) * (0.05f * baseAtk) + (rank - 1) * (GetHeroLevelMax(rank) - 1) * (0.05f * baseAtk);
            }
            public static float GetHeroATKSpeed(float baseAtkSpeed, int level, int rank)
            {
                return baseAtkSpeed;
            }
            public const int HERO_LEVEL_MAX = 10;
            public static int GetHeroLevelMax(int rank) //đề phòng sau này thay đổi limit level theo rank
            {
                return HERO_LEVEL_MAX;
            }
            public static int GetEnemyHPModeBoss(int baseHP, int level)
            {
                int indexHs = level - 1;
                float add = 0.025f * indexHs;
                return Mathf.FloorToInt(baseHP * add);
            }

            public static int GetEnemyAtkModeBoss(int baseAtk, int level)
            {
                int indexHs = level - 1;
                float add = 0.025f * indexHs;
                return Mathf.FloorToInt(baseAtk * add);
            }
            public static int GetAtkBonusSurfaceArea(int baseAtk)
            {
                return baseAtk;
            }
        }
    }
