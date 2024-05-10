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
    }
}
