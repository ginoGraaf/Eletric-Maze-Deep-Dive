using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private NodeGridSystem nodeGridSystem;
    private NodeGridSystem.NodeGridObject startPoint;
    private NodeGridSystem.NodeGridObject endPoint;

    private List<NodeGridSystem.NodeGridObject> openNodes = new List<NodeGridSystem.NodeGridObject>();
    private List<NodeGridSystem.NodeGridObject> closedNodes = new List<NodeGridSystem.NodeGridObject>();
    private NodeGridSystem.NodeGridObject currentNode;
    public void StartPathfinding()
    {
        PlaceStartAndEnd();
    }
    private void PlaceStartAndEnd()
    {
        startPoint = nodeGridSystem.GiveRandomNode();
        endPoint= nodeGridSystem.GiveRandomNode();

        startPoint.UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.StartPoint);
        endPoint.UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.EndPoint);
    }

    private void DoPathfinding(NodeGridSystem.NodeGridObject start, NodeGridSystem.NodeGridObject end)
    {
        openNodes.Clear();
        closedNodes.Clear();
        openNodes.Add(start);
        while(openNodes.Count!=0)
        {
            //do pathding..
            foreach(NodeGridSystem.NodeGridObject node in openNodes)
            {
                if(currentNode==null)
                {
                    currentNode = node;
                }
               // else if(currentNode)
            }


        }
    }
}
