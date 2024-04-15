using System.Collections;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Demo.Script.UI
{
    public class Panel3 : PanelController
    {
        [SerializeField] public Animator mAnimator;

        [ContextMenu("Validate")]
        private void Validate()
        {
            mAnimator = GetComponentInChildren<Animator>();
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