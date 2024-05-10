using System;
using System.Collections;
using System.Collections.Generic;
using Lam;
using ntDev;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Lam
{
    [Serializable]
    public class InfoFrogAction
    {
        public IFrog frog;
        public float timeAction;
        public RouteFrog routeFrog;
    }
    public class FrogManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_frogPrefab;
        private List<IFrog> m_listFrog = new List<IFrog>();
        
        private void Awake()
        {
            ManagerEvent.RegEvent(EventCMD.SPAWN_FROG, SpawnFrog);
        }

        IEnumerator SpawnFrogCor(float totalTime)
        {
            int amountFrog = StatGame.VILLAGERS > 0 ? StatGame.VILLAGERS : 1;
            List<InfoFrogAction> lisAction = new List<InfoFrogAction>();
            for (int i = 0; i < amountFrog; i++)
            {
                RouteFrog routeFrog = GameManager.Instance.m_pathManager.GetRandomRouteFrog(TypeFrog.youth);
                float t = GameManager.Instance.m_pathManager.CalculateTime(routeFrog);
                GameObject frog = Instantiate(m_frogPrefab, routeFrog.startNode.worldPosition, quaternion.identity);
                frog.SetActive(false);
                if(frog.TryGetComponent(out IFrog ifrog))
                {
                    InfoFrogAction infoFrogAction = new InfoFrogAction();
                    infoFrogAction.frog = ifrog;
                    infoFrogAction.routeFrog = routeFrog;
                    infoFrogAction.timeAction = t;
                    lisAction.Add(infoFrogAction);
                }
            }

            int indexMax = 0;
            for (int i = 0; i < lisAction.Count; i++)
            {
                if (lisAction[i].timeAction >= lisAction[indexMax].timeAction)
                {
                    indexMax = i;
                }
            }

            InfoFrogAction actionMax = lisAction[indexMax];
            lisAction.Remove(actionMax);
            float timePerAction = (totalTime - actionMax.timeAction) / lisAction.Count;
            WaitForSeconds waitForPerAction = new WaitForSeconds(timePerAction);
            for (int i = 0; i < lisAction.Count; i++)
            {
                lisAction[i].frog.gameObject.SetActive(true);
                lisAction[i].frog.frogMove.FollowRoute( lisAction[i].routeFrog);
                yield return waitForPerAction;
            }
            actionMax.frog.gameObject.SetActive(true);
            actionMax.frog.frogMove.FollowRoute(actionMax.routeFrog);
        }

        public void Spawn()
        {
            RouteFrog routeFrog = GameManager.Instance.m_pathManager.GetRandomRouteFrog(TypeFrog.youth);
            GameObject frog = Instantiate(m_frogPrefab, routeFrog.startNode.worldPosition, quaternion.identity);
            if(frog.TryGetComponent(out FrogMove frogMove))
            {
                frogMove._routeFrog = routeFrog;
                frogMove.FollowRoute(routeFrog);
            }
        }
        void SpawnFrog(object e)
        {
            Debug.Log("total time: " + GameManager.Instance.buildingSystem.listBuilding.Count * GameConfig.TIME_A_DAY_PER_BUILDING);
            StartCoroutine(SpawnFrogCor(GameManager.Instance.buildingSystem.listBuilding.Count * GameConfig.TIME_A_DAY_PER_BUILDING));
            //StartCoroutine(a());
        }

        IEnumerator a()
        {
            GameManager.Instance.m_pathManager.GetPath(GameManager.Instance.m_pathManager.listLane[1].listNode[0],
                GameManager.Instance.m_pathManager.listLane[1].listNode[5], 2);
            yield return null;
            GameManager.Instance.m_pathManager.GetPath(GameManager.Instance.m_pathManager.listLane[1].listNode[0],
                GameManager.Instance.m_pathManager.listLane[1].listNode[5], 1);
        }
       
    }
    
}
[CustomEditor(typeof(FrogManager))] 
public class EditorbtnSpawn : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FrogManager frogManager = (FrogManager)target;
        if (GUILayout.Button("SPawn"))
        {
            frogManager.Spawn();
        }
    }
}
