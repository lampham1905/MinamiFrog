using System.Collections;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components
{
    public class DontDestroyObject : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;

            if (transform.childCount > 0)
                DontDestroyOnLoad(gameObject);
            else
                Destroy(gameObject);
        }
    }
}