using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using UnityEngine;

namespace Lam
{
    public class UICustomizeTime : MonoBehaviour
    {
        [SerializeField] private List<JustButton> m_listBtn = new List<JustButton>();
        public List<int> m_listTimeScale = new List<int>();

        private void Start()
        {
            int index = 0;
            foreach (var VARIABLE in m_listBtn)
            {
                VARIABLE.onClick.AddListener(() =>
                {
                    Ajust(m_listTimeScale[index]);
                    index++;
                });
            }
        }

        void Ajust(int scale)
        {
            TimeManager.SALE_TIME = scale;
            Debug.Log("scale time: " +  TimeManager.SALE_TIME);
        }
    }
}
