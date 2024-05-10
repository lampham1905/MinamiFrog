using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lam
{
    public class FrogAnim : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        
        public void PlayAnim(string animName, float time)
        {
            m_animator.CrossFadeInFixedTime(animName, time);
        }

        private void Update()
        {
            m_animator.SetFloat("SpeedAnim", TimeManager.SALE_TIME);
        }
    }
}
