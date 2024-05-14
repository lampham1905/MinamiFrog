using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Lam
{
    [Serializable]
    public class PathLane
    {
        public List<Node> listNode = new List<Node>();
    }

    [Serializable]
    public class RouteFrog
    {
        public Node startNode;
        public List<IBuilding> listBuilding;
        public Node endNode;
        public int Lane;
    }
    public class PathManager : MonoBehaviour
    {
        private List<Transform> path = new List<Transform>();
        public List<float> posZLane = new List<float>();
        public List<PathLane> listLane = new List<PathLane>();
        private float posXCurrent = -1.5f;
        public GameObject pathPiece;
        [SerializeField] private AStarPathfinding m_aStarPathfinding;
        public List<int> listIndexNodeBuilding = new List<int>();
        private int amountCurrent;
        private List<IBuilding> listBulidng;
        public FrogMove frogMove;
        private void Awake()
        {
            ManagerEvent.RegEvent(EventCMD.INITPATH, Init);
            ManagerEvent.RegEvent(EventCMD.ADDPATH, AddPath);
        }

        private void Start()
        {
        }

        void Init(object e)
        {
            listBulidng = e as List<IBuilding>;
             int amount = 0; 
             for (int i = 0; i < listBulidng.Count; i++)
             {
                    amount += listBulidng[i].size;
                    listIndexNodeBuilding.Add(amount-1);
             }
            for (int i = 0; i < posZLane.Count; i++)
            {
                PathLane lane = new PathLane();
                listLane.Add(lane);
            }
            CreatPath(amount);
        }        
    
        public void CreatPath(int amount)
        {
            amountCurrent = amount + 1;
            m_aStarPathfinding.grid = new Node[listLane.Count, amount + 1];
            for (int i = 0; i < listLane.Count; i++)
            {
                float posX = posXCurrent;
                for (int j = 0; j < amount + 1; j++)
                {
                    Vector3 worldPostion = new Vector3(posX, 0.5f, posZLane[i]);
                    GameObject g = Instantiate(pathPiece, worldPostion, Quaternion.identity);
                    g.name = i.ToString() + "," + j.ToString();
                    Node node  = new Node(false, worldPostion, i, j, g);
                    m_aStarPathfinding.grid[i, j] = node;
                    listLane[i].listNode.Add( m_aStarPathfinding.grid[i, j]);
                    posX -= GameConfig.WIDTHSTONE;
                }
        
                if (i == listLane.Count - 1)
                {
                    posXCurrent = posX;
                }
            }

            for (int i = 0; i < listBulidng.Count; i++)
            {
                listBulidng[i].node = listLane[0].listNode[listIndexNodeBuilding[i]];
                listLane[0].listNode[listIndexNodeBuilding[i]].idBuiling = listBulidng[i].id;
            }
        }

        
        public void AddPath(object e)
        {
            int amountAdd = (int)e;
            Node[,] tempNode = new Node[listLane.Count, amountCurrent];
            for (int i = 0; i < listLane.Count; i++)
            {
                for (int j = 0; j < amountCurrent; j++)
                {
                    tempNode[i, j] = m_aStarPathfinding.grid[i, j];
                }
            }
        
            m_aStarPathfinding.grid = new Node[listLane.Count, amountCurrent + amountAdd];
            for (int i = 0; i < listLane.Count; i++)
            {
                for (int j = 0; j < amountCurrent; j++)
                {
                    m_aStarPathfinding.grid[i, j] = tempNode[i, j];
                }
            }
        
            for (int i = 0; i < listLane.Count; i++)
            {
                float posX = posXCurrent;
                for (int j = amountCurrent; j < amountCurrent + amountAdd; j++)
                {
                    Vector3 worldPostion = new Vector3(posX, 0.5f, posZLane[i]);
                    GameObject g = Instantiate(pathPiece, worldPostion, Quaternion.identity);
                    g.name = i.ToString() + "," + j.ToString();
                    Node node = new Node(false, worldPostion, i, j, g);
                    m_aStarPathfinding.grid[i, j] = node;
                    listLane[i].listNode.Add(node);
                    posX -= GameConfig.WIDTHSTONE;
                }
                if (i == listLane.Count - 1)
                {
                    posXCurrent = posX;
                }
            }
            amountCurrent += amountAdd;
            listBulidng[listBulidng.Count -1].node = listLane[0].listNode[amountCurrent - 2];
            listLane[0].listNode[amountCurrent - 2].idBuiling = listBulidng[listBulidng.Count - 1].id;
            listIndexNodeBuilding.Add(amountCurrent - 2);
        }

        public List<Node> GetPath(Node start, Node target, int Lane)
        {
            // int indexStart = m_aStarPathfinding.GetIndexOfNode(start);
            // int indexEnd = m_aStarPathfinding.GetIndexOfNode(target);
            // if (indexStart <= indexEnd)
            // {
            //     for (int i = 0; i < Lane; i++)
            //     {
            //         for (int j = indexStart+1; j < indexEnd; j++)
            //         {
            //             m_aStarPathfinding.grid[i, j].isObstacle = true;
            //         }
            //     }
            // }
            // else
            // {
            //     for (int i = 0; i < Lane; i++)
            //     {
            //         for (int j = indexStart+1; j > indexEnd; j--)
            //         {
            //             m_aStarPathfinding.grid[i, j].isObstacle = true;
            //         }
            //     }
            // }
            // return m_aStarPathfinding.FindPath(start, target);
            //---------------
            List<Node> res = new List<Node>();
            res.Add(start);
            
            int indexStart = start.gridY;
            int indexEnd = target.gridY;
            if (indexStart <= indexEnd)
            {
                for(int i = indexStart + 1; i <= indexEnd - 1; i++)
                {
                    res.Add(listLane[Lane].listNode[i]);
                }
            }
            else
            {
                for(int i = indexStart - 1; i >= indexEnd + 1; i--)
                {
                    res.Add(listLane[Lane].listNode[i]);
                }
            }
            res.Add(target);
            for (int i = 0; i < res.Count; i++)
            {
                res[i].visual.SetActive(true);
            }
            return res;
        }

        public static float GetDistance(List<Node> listNode)
        {
            float distance = 0;
            if (listNode != null)
            {
                for (int i = 0; i < listNode.Count-1; i++)
                {
                    distance += Vector3.Distance(listNode[i].worldPosition, listNode[i + 1].worldPosition);
                }
            }
            return distance; 
        }

        Node GetRandomNode(TypeFrog typeFrog)
        {
            int random = Random.Range(0, 3);
            int randomLane = Random.Range(0, listLane.Count);
            switch (random)
            {
                case 0:
                    return listLane[randomLane].listNode[0];
                    break;
                case 1:
                    return listLane[randomLane].listNode[listLane[randomLane].listNode.Count - 1];
                    break;
                case 2:
                    List<IBuilding> listBuilding = new List<IBuilding>();
                    if (listBuilding.Count > 0)
                    {
                        switch (typeFrog)
                        {
                            case TypeFrog.youth:
                                for (int i = 0; i < listBulidng.Count; i++)
                                {
                                    if (listBulidng[i].idBuilding == 1)
                                    {
                                        listBuilding.Add(listBulidng[i]);
                                    }
                                }
                                break;
                            case TypeFrog.elder:
                                for (int i = 0; i < listBulidng.Count; i++)
                                {
                                    if (listBulidng[i].idBuilding == 5)
                                    {
                                        listBuilding.Add(listBulidng[i]);
                                    }
                                }
                                break;
                            case TypeFrog.golbin:
                                for (int i = 0; i < listBulidng.Count; i++)
                                {
                                    if (listBulidng[i].idBuilding == 11)
                                    {
                                        listBuilding.Add(listBulidng[i]);
                                    }
                                }
                                break;
                        }

                        int randomBuilding = Random.Range(0, listBuilding.Count);
                        return listBuilding[randomBuilding].node;
                    }
                    else
                    {
                        return listLane[randomLane].listNode[0];
                    }
                    break;
                  
            }
            return null;
        }

        List<IBuilding> GetRandomListBuilding()
        {
            List<IBuilding> listServiceBuilding = GameManager.Instance.buildingSystem.GetListBuildingService();
            List<IBuilding> res = new List<IBuilding>();
            int randomAmount = Random.Range(0, listServiceBuilding.Count);
            int amountTypeShopLevel = 0;
            int shopLevelTemp = 0;
            for (int i = 0; i < listServiceBuilding.Count; i++)
            {
                if (listServiceBuilding[i].shopData.shopLevel != shopLevelTemp)
                {
                    shopLevelTemp = listServiceBuilding[i].shopData.shopLevel;
                    amountTypeShopLevel += 1;
                }
            }

            switch (amountTypeShopLevel)
            {
                case 1:
                    for (int i = 0; i < listServiceBuilding.Count; i++)
                    {
                        if (listServiceBuilding[i].shopData.shopLevel != 0)
                        {
                            if (IsGoShop(90))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else
                        {
                            if (IsGoShop(50))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                    }
                    break;
                case 2:
                    int levelMax = 0;
                    for (int i = 0; i < listServiceBuilding.Count; i++)
                    {
                        if (listServiceBuilding[i].shopData.shopLevel > levelMax)
                        {
                            levelMax = listServiceBuilding[i].shopData.shopLevel;
                        }
                    }

                    for (int i = 0; i < listServiceBuilding.Count; i++)
                    {
                        if (listServiceBuilding[i].shopData.shopLevel >= levelMax)
                        {
                            if (IsGoShop(80))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else if(listServiceBuilding[i].shopData.shopLevel < levelMax && listServiceBuilding[i].shopData.shopLevel > 0)
                        {
                            if (IsGoShop(60))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else
                        {
                            if (IsGoShop(50))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < listServiceBuilding.Count; i++)
                    {
                        if (listServiceBuilding[i].shopData.shopLevel == 1)
                        {
                            if (IsGoShop(50))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else if (listServiceBuilding[i].shopData.shopLevel == 2)
                        {
                            if (IsGoShop(70))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else if (listServiceBuilding[i].shopData.shopLevel == 3)
                        {
                            if (IsGoShop(90))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                        else
                        {
                            if (IsGoShop(50))
                            {
                                res.Add(listServiceBuilding[i]);
                            }
                        }
                    }
                    break;
            }
           
            return res;
        }

        bool IsGoShop(float ratio)
        {
            int random = Random.Range(1, 101);
            if (random <= ratio)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public RouteFrog GetRandomRouteFrog(TypeFrog typeFrog)
        {
            RouteFrog routeFrog = new RouteFrog();
            routeFrog.startNode = GetRandomNode(typeFrog);
            while (true)
            {
                routeFrog.endNode = GetRandomNode(typeFrog);
                if (routeFrog.endNode != routeFrog.startNode)
                {
                    break;
                }
            }
            routeFrog.Lane = Random.Range(1, listLane.Count);
            routeFrog.listBuilding = GetRandomListBuilding();
            return routeFrog;
        }

        float GetTimePath(List<Node> path)
        {
            return LogicAPI.GetTimeMove(GetDistance(path), GameConfig.SPEEDFROG, GameConfig.TIME_JUMP, GameConfig.TIME_IDLE);
        }
        public float CalculateTime(RouteFrog routeFrog)
        {
            float res = 0;
            if (routeFrog.listBuilding.Count == 0)
            {
                res += GetTimePath(GetPath(routeFrog.startNode, routeFrog.endNode, routeFrog.Lane));
            }
            else
            {
                res += GetTimePath(GetPath(routeFrog.startNode, routeFrog.listBuilding[0].node, routeFrog.Lane));
                res += GameConfig.TIME_PER_INTERACT_BUILDING;
                res += GetTimePath(GetPath(routeFrog.listBuilding[routeFrog.listBuilding.Count - 1].node, routeFrog.endNode, routeFrog.Lane));
                if (routeFrog.listBuilding.Count > 1)
                {
                    for (int i = 0; i < routeFrog.listBuilding.Count - 1; i++)
                    {
                        res += GetTimePath(GetPath(routeFrog.listBuilding[i].node, routeFrog.listBuilding[i+1].node, routeFrog.Lane));
                        res += GameConfig.TIME_PER_INTERACT_BUILDING;
                    }
                }
            }
            return res;
        }
        
    }
    
 

  
}
