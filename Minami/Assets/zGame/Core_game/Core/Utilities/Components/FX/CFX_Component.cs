using System;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.FX
{
    public abstract class CFX_Component : MonoBehaviour
    {
        public bool initialized;
        public Action onHidden;
        public Renderer[] renderers;
        public float lifeTime;

        private bool mAutoDeactive;
        private float mElapsedTime;
        private float mCustomLifeTime;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            //UnityEngine.Debug.Log(gameObject.name + "  " + mAutoDeactive + "   " + mCustomLifeTime+"   "+ mElapsedTime);
            if (mAutoDeactive && mCustomLifeTime > 0)
            {
                //mElapsedTime += Time.deltaTime;//FIXME
                mElapsedTime += Time.unscaledDeltaTime;//FIXME
                if (mElapsedTime >= mCustomLifeTime)
                {
                    Clear();
                    Stop();
                    onHidden.Raise();
                    gameObject.SetActive(false);
                }
            }
            else
                enabled = false;
        }

        public virtual void Initialize()
        {
            if (initialized)
                return;

            initialized = true;

            Validate();
        }

        public virtual float GetLifeTime()
        {
            if (!initialized)
                Initialize();

            return lifeTime;
        }

        public virtual void Play(bool pAutoDeactive, float pCustomLifeTime = 0)
        {
            mElapsedTime = 0;
            mCustomLifeTime = pCustomLifeTime > 0 ? pCustomLifeTime : GetLifeTime();
            mAutoDeactive = pAutoDeactive;
            enabled = pAutoDeactive;
        }

        public abstract void Stop();

        public abstract void Clear();

        public virtual void SetSortingOrder(int pValue)
        {
            if (!initialized)
                Initialize();

            if (renderers != null)
                for (int i = 0; i < renderers.Length; i++)
                    renderers[i].sortingOrder = pValue;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            initialized = false;
            Initialize();
        }
#endif

        protected abstract void Validate();
    }
}