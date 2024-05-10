using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.Manager;
using ntDev;
using UnityEngine;

namespace Lam
{
    public class GameController : MonoBehaviour
    {
        private static GameController m_Instance;
        public static GameController Instance => m_Instance;
        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            GameInit.Instance.Init();
        }
    }
}
