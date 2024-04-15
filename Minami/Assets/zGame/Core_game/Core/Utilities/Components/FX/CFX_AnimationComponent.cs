﻿
#pragma warning disable 0649

using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.FX
{
    public class CFX_AnimationComponent : CFX_Component
    {
        public Animator animator;

        private int mTriggerValue;
        private int triggerValue
        {
            set
            {
                if (mTriggerValue != value)
                {
                    animator.SetInteger("trigger", value);

                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                    lifeTime = info.length;
                }
            }
            get { return mTriggerValue; }
        }

        public void Play(int pTriggerValue, bool pAutoDeactive)
        {
            if (!initialized)
                Initialize();

            if (animator != null)
            {
                gameObject.SetActive(true);
                animator.enabled = true;
                animator.Rebind();
                triggerValue = pTriggerValue;

                base.Play(pAutoDeactive);
            }
        }

        public override void Play(bool pAutoDeactive, float pCustomLifeTime = 0)
        {
            if (!initialized)
                Initialize();

            if (animator != null)
            {
                gameObject.SetActive(true);
                animator.enabled = true;
                animator.Rebind();

                base.Play(pAutoDeactive, pCustomLifeTime);
            }
        }

        public override void Stop()
        {
            gameObject.SetActive(false);
        }

        public override void Clear()
        {
            gameObject.SetActive(false);
        }

        public override float GetLifeTime()
        {
            if (animator != null)
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                lifeTime = info.length;
            }

            return lifeTime;
        }

        //[DEBUG]
        public int GetCurrentTriggerValue()
        {
            return animator.GetInteger("trigger");
        }

        protected override void Validate()
        {
            if (animator == null)
#if UNITY_2019_2_OR_NEWER
                gameObject.TryGetComponent(out animator);
#else
                animator = GetComponent<Animator>();
#endif

            if (animator != null)
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                lifeTime = info.length;
            }

            if (renderers == null || renderers.Length == 0)
            {
                int count = 0;
#if UNITY_2019_2_OR_NEWER
                gameObject.TryGetComponent(out Renderer p);
#else
                var p = gameObject.GetComponent<Renderer>();
#endif
                if (p != null)
                    count++;

                var childP = gameObject.GetComponentsInChildren<Renderer>();
                if (childP != null)
                    count += childP.Length;

                if (count > 0)
                {
                    renderers = new Renderer[count];
                    if (p != null)
                        renderers[0] = p;
                    if (childP != null)
                        for (int i = 1; i <= childP.Length; i++)
                        {
                            if (p != null)
                                renderers[i] = childP[i - 1];
                            else
                                renderers[i - 1] = childP[i - 1];
                        }
                }
            }
        }
    }
}