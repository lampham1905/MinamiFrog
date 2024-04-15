using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes
{
    public class SeparatorAttribute : PropertyAttribute
    {
        public readonly string title;

        public SeparatorAttribute()
        {
            this.title = "";
        }

        public SeparatorAttribute(string _title)
        {
            this.title = _title;
        }
    }
}