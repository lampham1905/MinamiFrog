using Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    public class Mod
    {
        public int id;
        public float[] values;
        public float[] unlocks;
        public float[] increases;
        public float value => values[0];
        public Mod(int pId, float[] pValues, float[] pUnlocks, float[] pIncreases)
        {
            id = pId;
            values = pValues;
            unlocks = pUnlocks;
            increases = pIncreases;
        }
        public Mod(int pId, float pValue)
        {
            id = pId;
            values = new float[1];
            values[0] = pValue;
        }

        public ModDefinition GetDescription()
        {
            return BuiltinData.Instance.GetMod(id - 1);
        }
    }
}
