using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lam
{
    public class FrogMove : MonoBehaviour
    {
        [SerializeField] private FrogAnim m_frogAnim;
        [SerializeField] private float speedMove;
        [SerializeField] private float timeJump;
        [SerializeField] private float timeIdle;
        private float m_timeJump = 0;
        private float m_timeCurrent = 0;
        private bool canAnimJump;
        public float time = 0;
        private Vector3 targetDirection;
        private bool canJumpTemp = true;
        private WaitForSeconds _waitForPerBuilding = new WaitForSeconds(GameConfig.TIME_PER_INTERACT_BUILDING);
        private bool isCompleteCoroutine = true;
        public RouteFrog _routeFrog;
        public List<Node> listNode;
        IEnumerator FollowPathCor(List<Node> path)
        {
            isCompleteCoroutine = false;
            if (path != null)
            { 
                this.transform.position = path[0].worldPosition;
            transform.forward = (path[1].worldPosition - transform.position).normalized;
            for (int i = 0; i < path.Count; i++)
            { 
                while (transform.position != path[i].worldPosition )
                {
                    time += Time.deltaTime * TimeManager.SALE_TIME;
                    //float m_currentTimeBackup = m_timeCurrent;
                    m_timeCurrent += Time.deltaTime * TimeManager.SALE_TIME;
                    if (m_timeCurrent > timeJump)
                    {
                        if (canJumpTemp)
                        {
                            canJumpTemp = false;
                            float time = m_timeCurrent- timeJump ;
                            transform.position = Vector3.MoveTowards(transform.position, path[i].worldPosition, speedMove * time * TimeManager.SALE_TIME);
                        }
                    }
                    
                    if (m_timeCurrent <= timeJump)
                    {
                        // aniamtion
                        if (canAnimJump)
                        {
                            canAnimJump = false;
                            m_frogAnim.PlayAnim("jump", 0.5f);
                        }
                        // move
                        transform.position = Vector3.MoveTowards(transform.position, path[i].worldPosition, speedMove * Time.deltaTime * TimeManager.SALE_TIME);
                        // rotate
                        targetDirection = path[i].worldPosition - transform.position;
                        transform.forward = Vector3.Lerp(transform.forward, targetDirection.normalized, Time.deltaTime * 10 * TimeManager.SALE_TIME);
                    }
                    
                    if (m_timeCurrent >= timeJump + timeIdle)
                    {
                        m_timeCurrent -= (timeJump + timeIdle);
                        canAnimJump = true;
                        canJumpTemp = true;
                    }
                    //transform.position = Vector3.MoveTowards(transform.position, path[i].worldPosition, speedMove * Time.deltaTime);
                    yield return null;
                }
                path[i].visual.SetActive(false);
            }
            }
            isCompleteCoroutine = true;
        }
        public void FollowPath(List<Node> path)
        {
            StartCoroutine(FollowPathCor(path));
            //Debug.Log("distance :" + PathManager.GetDistance(path) + ".......time: " + LogicAPI.GetTimeMove(PathManager.GetDistance(path), speedMove, timeJump, timeIdle));
        }

     
        public void FollowRoute(RouteFrog routeFrog)
        {
            listNode.Clear();
            listNode.Add(routeFrog.startNode);
            for (int i = 0; i < routeFrog.listBuilding.Count; i++)
            {
                listNode.Add(routeFrog.listBuilding[i].node);
            }
            listNode.Add(routeFrog.endNode);
            StartCoroutine(ExecuteListAction(listNode, routeFrog.Lane));
        }
        IEnumerator ExecuteListAction(List<Node> listNode, int Lane)
        {
            float time = 0;
            // while (true)
            // {
            //     if (isCompleteCoroutine)
            //     {
            //         if (listNode.Count > 1)
            //         {
            //             yield return StartCoroutine(FollowPathCor(GameManager.Instance.m_pathManager.GetPath(listNode[0], listNode[1], Lane)));
            //             if (listNode[1].idBuiling != -1)
            //             {
            //                 while (time <= GameConfig.TIME_PER_INTERACT_BUILDING)
            //                 {
            //                     time += Time.deltaTime * TimeManager.SALE_TIME;
            //                 }
            //
            //                 time -= GameConfig.TIME_PER_INTERACT_BUILDING;
            //                 yield return null;
            //             }
            //
            //             listNode.Remove(listNode[0]);
            //         }
            //         else
            //         {
            //             break;
            //         }
            //     }
            //     yield return null;
            // }
            if (this.listNode.Count > 1)
            {
                for (int i = 0; i < listNode.Count - 1; i++)
                {
                    yield return StartCoroutine(FollowPathCor(GameManager.Instance.m_pathManager.GetPath(listNode[i], listNode[i+1], Lane)));
                    if (listNode[i+1].idBuiling != -1)
                    {
                        while (time <= GameConfig.TIME_PER_INTERACT_BUILDING)
                        {
                            time += Time.deltaTime * TimeManager.SALE_TIME;
                            yield return null;
                        }
                    
                        time -= GameConfig.TIME_PER_INTERACT_BUILDING;
                    }
                }
            }
            this.gameObject.SetActive(false);
        }
    }
}
