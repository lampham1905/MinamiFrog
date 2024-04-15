

 using UnityEngine.UI;

 namespace Lam.zGame.Core_game.Core.Utilities.Components.Scrollview
{
    public class OptimizedScrollItemTest : OptimizedScrollItem
    {
        public Text mTxtIndex;

        public override void UpdateContent(int pIndex)
        {
            base.UpdateContent(pIndex);

            name = pIndex.ToString();
            mTxtIndex.text = pIndex.ToString();
        }
    }
}