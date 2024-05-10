using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using TMPro;
using UnityEngine;

namespace Lam
{
    public class UITime : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtTime;
        [SerializeField] private JustButton btnScale;
        public void UpdateTime(string time)
        {
            m_TxtTime.text = time;
        }
    }
}
