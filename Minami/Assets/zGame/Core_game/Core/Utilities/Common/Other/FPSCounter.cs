using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Common.Other
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class FPSCounter : IUpdate
    {
        public int fps;

        private float mTimeEslap;
        private int mCountFrame;

        public int id { get; set; }
        public bool updated { get; set; }

        public void Update(float pDeltaTime)
        {
            updated = false;
            mTimeEslap += pDeltaTime;
            mCountFrame++;

            if (mTimeEslap >= 1)
            {
                fps = Mathf.RoundToInt(mCountFrame * 1f / mTimeEslap);

                mTimeEslap = 0;
                mCountFrame = 0;
                updated = true;
            }
        }
    }
}
