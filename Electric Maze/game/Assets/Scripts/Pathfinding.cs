using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private NodeGridSystem nodeGridSystem;
    [SerializeField] private PathfindingNodes PathfindingNodes;
    [SerializeField] private NerveSpiralPath nerveSpiralPath;

    private NodeGridSystem.NodeGridObject startPoint;
    private NodeGridSystem.NodeGridObject endPoint;

    private List<PathfindingNodes.PathNodeObject> openNodes = new List<PathfindingNodes.PathNodeObject>();
    private List<PathfindingNodes.PathNodeObject> closedNodes = new List<PathfindingNodes.PathNodeObject>();
    private List<PathfindingNodes.PathNodeObject> path = new List<PathfindingNodes.PathNodeObject>();
    List<PathfindingNodes.PathNodeObject> neigbors = new List<PathfindingNodes.PathNodeObject>();
    private PathfindingNodes.PathNodeObject currentNode;
    PathfindingNodes.PathNodeObject goalNode;
    public void StartPathfinding()
    {
        PlaceStartAndEnd();
    }
    private void PlaceStartAndEnd()
    {
        startPoint = nodeGridSystem.GiveRandomNode(0,0,nodeGridSystem.Width/2,nodeGridSystem.Height);
        endPoint = nodeGridSystem.GiveRandomNode(nodeGridSystem.Width/2,0,nodeGridSystem.Width,nodeGridSystem.Height);

        startPoint.UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.StartPoint);
        endPoint.UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.EndPoint);

        StartCoroutine(DoPathfinding(startPoint, endPoint));
    }

    private IEnumerator DoPathfinding(NodeGridSystem.NodeGridObject start, NodeGridSystem.NodeGridObject end)
    {
        openNodes.Clear();
        closedNodes.Clear();
        path.Clear();
        openNodes.Add(PathfindingNodes.GetNode(start.X, start.Y));
        goalNode = PathfindingNodes.GetNode(end.X, end.Y);
        currentNode = PathfindingNodes.GetNode(start.X, start.Y);
        currentNode.CalculateHScore(goalNode);
        currentNode.CalculatedGScore(0);
        currentNode.CalculateFScore();
        while (openNodes.Count != 0)
        {
            //Debug.Log(openNodes.Count);
            //do pathding..
            foreach (PathfindingNodes.PathNodeObject node in openNodes)
            {
                if(currentNode==null)
                {
                    currentNode = node;
                }
            
                if (currentNode.GetF() > node.GetF())
                {
                    currentNode = node;
                }
            }

            if (currentNode.GetX() == goalNode.GetX() && currentNode.GetY() == goalNode.GetY())
            {
            
                PathfindingNodes.PathNodeObject lastNode = currentNode;
                while (lastNode.GetParent() != null)
                {
                    nodeGridSystem.GetNodeGrid(lastNode.GetX(), lastNode.GetY()).UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.StartPoint);
                    path.Add(lastNode);
                    lastNode = lastNode.GetParent();
                }
                
                Debug.Log("Pathfounded");
                SetNervePath();
                break;
            }

            openNodes.Remove(currentNode);
            nodeGridSystem.GetNodeGrid(currentNode.GetX(), currentNode.GetY()).UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.Checked);
            closedNodes.Add(currentNode);
       
            neigbors.Clear();
            GetNeighBores(currentNode);

            foreach (PathfindingNodes.PathNodeObject neighbore in neigbors)
            {
               
                if (closedNodes.Contains(neighbore))
                {
                    continue;
                }

                float newGScore = (currentNode.GetG()) + HeuristicCalculation(currentNode,neighbore);
                if (newGScore < neighbore.GetG())
                {
                    neighbore.CalculatedGScore(newGScore);
                    neighbore.CalculateHScore(goalNode);
                    neighbore.CalculateFScore();
                    neighbore.SetNodeParent(currentNode);
                    if (!openNodes.Contains(neighbore))
                    {
                        openNodes.Add(neighbore);

                        nodeGridSystem.GetNodeGrid(neighbore.GetX(), neighbore.GetY()).UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.Current);
                    }
                }
            }
            currentNode = null;
            yield return new WaitForSeconds(0.05f);
        }
       
    }

    void SetNervePath()
    {

        nerveSpiralPath.PlaceGameObject(path);

    }

    private float HeuristicCalculation(PathfindingNodes.PathNodeObject currentNode, PathfindingNodes.PathNodeObject goalNode)
    {
        return (Mathf.Abs(currentNode.GetX() - goalNode.GetX()) + Mathf.Abs(currentNode.GetY() - goalNode.GetY()));
    }

    private void GetNeighBores(PathfindingNodes.PathNodeObject currentNode)
    {
        NodeGridSystem.NodeGridObject currentNodeGridObject = nodeGridSystem.GetNodeGrid(currentNode.GetX(), currentNode.GetY());
        if (currentNodeGridObject.East)
        {
            if (nodeGridSystem.GetNodeGrid(currentNode.GetX() - 1, currentNode.GetY()) != null)
            {
                neigbors.Add(PathfindingNodes.GetNode(currentNode.GetX() - 1, currentNode.GetY()));
            }
        }
        if (currentNodeGridObject.West)
        {
            if (nodeGridSystem.GetNodeGrid(currentNode.GetX() + 1, currentNode.GetY()) != null)
            {
                neigbors.Add(PathfindingNodes.GetNode(currentNode.GetX() + 1, currentNode.GetY()));
            }
        }
        if (currentNodeGridObject.North)
        {
            if (nodeGridSystem.GetNodeGrid(currentNode.GetX(), currentNode.GetY() + 1) != null)
            {
                neigbors.Add(PathfindingNodes.GetNode(currentNode.GetX(), currentNode.GetY() + 1));
            }
        }
        if (currentNodeGridObject.South)
        {
            if (nodeGridSystem.GetNodeGrid(currentNode.GetX(), currentNode.GetY() - 1) != null)
            {
                neigbors.Add(PathfindingNodes.GetNode(currentNode.GetX(), currentNode.GetY() - 1));
            }
        }
    }
}
