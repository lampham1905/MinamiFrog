using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    public class FPS : MonoBehaviour
    {
        private static FPS m_Instance;
        public static FPS Instance => m_Instance;

        public int fps;
        public bool updated;

        private float mTimeEslap;
        private int mCountFrame;

        private void Start()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        private void Update()
        {
            updated = false;
            mTimeEslap += Time.deltaTime;
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
