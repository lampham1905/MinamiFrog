using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.AddressablesManager
{
    public class MonoTracker : MonoBehaviour
    {
        public delegate void DelegateDestroyed(MonoTracker tracker);
        public event DelegateDestroyed onDestroyed;
        public object key;
        private void OnDestroy()
        {
            onDestroyed?.Invoke(this);
        }
    }
}