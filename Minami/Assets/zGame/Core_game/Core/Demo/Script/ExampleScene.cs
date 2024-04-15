using Lam.zGame.Core_game.Core.Demo.Script.Data;
using Lam.zGame.Core_game.Core.Demo.Script.UI;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Demo.Script
{
    public class ExampleScene : MonoBehaviour
    {
        void Start()
        {
            ExampleGameData.Instance.Init();
            MainPanel.instance.Init();
            ExamplePoolsManager.Instance.Init();
        }
    }
}