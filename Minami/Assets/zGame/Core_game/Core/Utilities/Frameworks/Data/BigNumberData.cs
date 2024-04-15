using Lam.zGame.Core_game.Core.Utilities.Common.Other;

namespace Lam.zGame.Core_game.Core.Utilities.Frameworks.Data
{
    public class BigNumberData : DataGroup
    {
        private FloatData mReadable;
        private IntegerData mPow;

        public BigNumberAlpha Value
        {
            get { return new BigNumberAlpha(mReadable.Value, mPow.Value); }
            set
            {
                mReadable.Value = value.readableValue;
                mPow.Value = value.pow;
            }
        }

        public BigNumberData(int pId, BigNumberAlpha pDefaultNumber = null) : base(pId)
        {
            mReadable = AddData(new FloatData(0, pDefaultNumber == null ? 0 : pDefaultNumber.readableValue));
            mPow = AddData(new IntegerData(1, pDefaultNumber == null ? 0 : pDefaultNumber.pow));
        }
    }
}