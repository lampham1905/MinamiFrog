

using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.Scrollview
{
    public class OptimizedScrollItem : MonoBehaviour
    {
        protected int mIndex = -1;

        public virtual void UpdateContent(int pIndex)
        {
            mIndex = pIndex;
        }
    }
}
