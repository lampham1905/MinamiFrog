using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class Node
{
    public bool isObstacle;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;
    public GameObject visual;
    public int idBuiling;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node(bool _isObstacle, Vector3 _worldPos, int _gridX, int _gridY, GameObject visual, int idBuiling = -1)
    {
        isObstacle = _isObstacle;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        this.visual = visual.transform.GetChild(0).gameObject;
        this.visual.SetActive(false);
        this.idBuiling = idBuiling;
    }
}

public class AStarPathfinding : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public float distance;

    public Node[,] grid;
    public List<Node> path;
    public GameObject pathPiece;
    private int gridSizeX;
    private int gridSizeY;
    void Start()
    {
        //CreateGrid();
        // startNode = grid[0, 0].worldPosition;
        // targetNode = grid[(int)gridSize.x - 1, (int)gridSize.y - 1].worldPosition;
        //path = FindPath(startNode.position, targetNode.position);
        // Sử dụng path tìm được ở đây
    }

    // void CreateGrid()
    // {
    //     int gridSizeX = Mathf.RoundToInt(gridSize.x / distance);
    //     int gridSizeY = Mathf.RoundToInt(gridSize.y / distance);
    //     grid = new Node[gridSizeX, gridSizeY];
    //     Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
    //
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * distance + nodeRadius) + Vector3.forward * (y * distance + nodeRadius);
    //             bool obstacle = Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);
    //             grid[x, y] = new Node(obstacle, worldPoint, x, y);
    //             GameObject gb = Instantiate(pathPiece, worldPoint, quaternion.identity);
    //             gb.name = "[" + x.ToString() + "," + y.ToString() + "]";
    //         }
    //     }
    // }

    public List<Node> FindPath(Node startPos, Node targetPos)
    {
        Node startNode = (startPos);
        Node targetNode = (targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor) || neighbor.isObstacle)
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.Log("nulllllllll");
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
//        Debug.Log(startNode.gridX +" " +startNode.gridY+ "----" + endNode.gridX +" " +endNode.gridY);
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        for(int i = 0 ; i < path.Count; i++)
        {
            path[i].visual.SetActive(true);
        }
        ResetObstacle();

        return path;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    Node GetNodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPos.z + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((grid.GetLength(0) - 1) * percentX);
        int y = Mathf.RoundToInt((grid.GetLength(1) - 1) * percentY);

        return grid[x, y];
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
    public int GetIndexOfNode(Node node)
    {
        gridSizeX = grid.GetLength(0);
        gridSizeY = grid.GetLength(1);
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (grid[i,j] == node)
                {
                    return j;
                }
            }
        }
        return -1;
    }

    public void ResetObstacle()
    {
         gridSizeX = grid.GetLength(0);
         gridSizeY = grid.GetLength(1);
         for (int i = 0; i < gridSizeX; i++)
         {
             for (int j = 0; j < gridSizeY; j++)
             {
                 grid[i, j].isObstacle = false;
             }
         }
    }

    // float GetDistance(List<Node> listNode)
    // {
    //     float distance
    // }
    }

