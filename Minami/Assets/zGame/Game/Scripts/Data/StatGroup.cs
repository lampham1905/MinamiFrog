using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using UnityEngine;

namespace Lam
{
    public class StatGroup : DataGroup
    {
        public FloatData satisfaction;
        public FloatData money;
        public IntegerData villagers;
        public FloatData beauty;
        public FloatData youth;
        public FloatData elder;
        public FloatData golbin;

        public StatGroup(int pId) : base(pId)
        {
            satisfaction = AddData(new FloatData(0));
            money = AddData(new FloatData(1, 9999));
            villagers = AddData(new IntegerData(2, 0));
            beauty = AddData(new FloatData(3));
            youth = AddData(new FloatData(4));
            elder = AddData(new FloatData(5));
            golbin = AddData(new FloatData(6));
        }

        public void Init()
        {
            StatGame.SATISFACTION = this.satisfaction.Value;
            StatGame.MONEY = this.money.Value;
            StatGame.VILLAGERS = this.villagers.Value;
            StatGame.BEAUTY = this.beauty.Value;
            StatGame.YOUTH = this.youth.Value;
            StatGame.ELDER = this.elder.Value;
            StatGame.GOLBIN = this.golbin.Value;
        }
        
    }
}
