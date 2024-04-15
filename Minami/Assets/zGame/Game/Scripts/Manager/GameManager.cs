using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.Manager;
using UnityEngine;

namespace Lam
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance;
        public BuildingSystem buildingSystem;
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
