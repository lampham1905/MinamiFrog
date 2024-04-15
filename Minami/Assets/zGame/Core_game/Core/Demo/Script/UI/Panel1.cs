﻿using System.Collections;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Demo.Script.UI
{
    public class Panel1 : PanelController
    {
        [SerializeField] private SimpleButton mBtnPanel1A;
        [SerializeField] private SimpleButton mBtnPanel1B;
        [SerializeField] private Panel1A mPanel1A;
        [SerializeField] private Panel1B mPanel1B;
        [SerializeField] public Animator mAnimator;

        public Panel1A Panel1A { get { return GetCachedPanel(mPanel1A); } }
        public Panel1B Panel1B { get { return GetCachedPanel(mPanel1B); } }

        private void Start()
        {
            mBtnPanel1A.onClick.AddListener(ShowPanel1A);
            mBtnPanel1B.onClick.AddListener(ShowPanel1B);
        }

        private void OnValidate()
        {
            if (mAnimator != null)
                mAnimator = GetComponentInChildren<Animator>();
        }

        [ContextMenu("ShowPanel1A")]
        public void ShowPanel1A()
        {
            PushPanel(ref mPanel1A, true);
        }

        [ContextMenu("ShowPanel1B")]
        public void ShowPanel1B()
        {
            PushPanel(ref mPanel1B, true);
        }

        protected override IEnumerator IE_HideFX()
        {
            mAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(0.3f);
        }

        protected override IEnumerator IE_ShowFX()
        {
            mAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(0.3f);
        }
    }
}