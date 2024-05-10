
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;

namespace Lam
{
    public class DataTestDai : DataGroup
    {
        private IntegerData t1;
        public DataTestDai(int pId) : base(pId)
        {
            t1 = AddData(new IntegerData(0, 5));
        }

        public void testfunc()
        {
            t1.Value = 10;
        }
    }
}
