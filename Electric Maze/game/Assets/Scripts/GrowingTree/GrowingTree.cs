using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class GrowingTree : MonoBehaviour
{
    //Current is the node to be checked if it has complete all side go down by one if there is no node left complete 
    // the allgorithme
    // complete node list is for all nodes that has been checked
    // Return noeList is all the node getadded in here for later refrence of sides..

    private Action CallPathfinding;

    List<NodeGridSystem.NodeGridObject> CurrentNodeList = new List<NodeGridSystem.NodeGridObject>();
    List<NodeGridSystem.NodeGridObject> CompleteNodeList = new List<NodeGridSystem.NodeGridObject>();

    // we need the start node and the grid itself.
    public void StartGrowingTreeAlgortime(NodeGridSystem.NodeGridObject startNode,Grid<NodeGridSystem.NodeGridObject> grid, NodeGridSystem nodeGridSystem)
    {
        CurrentNodeList.Clear();
        CompleteNodeList.Clear();
        CallPathfinding+= nodeGridSystem.CleanUpToWhite;
        CallPathfinding += nodeGridSystem.StartPathfinding;


        CurrentNodeList.Add(startNode);
        CurrentNodeList[0].UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.Current);
        StartCoroutine(GenerateMaze(grid));
    }

    IEnumerator GenerateMaze(Grid<NodeGridSystem.NodeGridObject> grid)
    {
        int worldSize = grid.GetHeight() * grid.GetWidth();
        while (CompleteNodeList.Count < worldSize)
        {
            NodeGridSystem.NodeGridObject nodeGrid = CurrentNodeList[CurrentNodeList.Count-1];
            //possiable Direction
            List<NodeGridSystem.NodeGridObject> possibleNodeGridObjects = new List<NodeGridSystem.NodeGridObject>();
            GetNode(nodeGrid, grid, 1, 0, possibleNodeGridObjects);
            GetNode(nodeGrid, grid, 0, 1, possibleNodeGridObjects);
            GetNode(nodeGrid, grid, -1, 0, possibleNodeGridObjects);
            GetNode(nodeGrid, grid, 0, -1, possibleNodeGridObjects);
            


            if (possibleNodeGridObjects.Count>0)
            {
                NodeGridSystem.NodeGridObject chooseNode = possibleNodeGridObjects[UnityEngine.Random.Range(0, possibleNodeGridObjects.Count)];
                CurrentNodeList.Add(chooseNode);
                if (chooseNode.GetMazeCoord().x==1)
                {
                    nodeGrid.West = true;
                    chooseNode.East = true;
                }
                if (chooseNode.GetMazeCoord().x == -1)
                {
                    nodeGrid.East = true;
                    chooseNode.West = true;
                }
                if (chooseNode.GetMazeCoord().y == 1)
                {
                    nodeGrid.North = true;
                    chooseNode.South = true;
                }
                if (chooseNode.GetMazeCoord().y == -1)
                {
                    nodeGrid.South = true;
                    chooseNode.North = true;
                }
             
                chooseNode.UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.Current);
            }
            else
            {
                CompleteNodeList.Add(CurrentNodeList[CurrentNodeList.Count - 1]);

                CurrentNodeList[CurrentNodeList.Count - 1].UpdateTileChecked(NodeGridSystem.NodeGridObject.TileChecked.Checked);
                CurrentNodeList.RemoveAt(CurrentNodeList.Count - 1);
            }
            nodeGrid.SetGraphic();
            yield return new WaitForSeconds(0.05f);
        }
        if (CallPathfinding != null)
        {
            CallPathfinding();
        }
    }

    private void GetNode(NodeGridSystem.NodeGridObject nodeGrid, Grid<NodeGridSystem.NodeGridObject> grid,int addXValue,int addYValue,List<NodeGridSystem.NodeGridObject> possibleNodeGridObjects)
    {
        if (grid.GetGridObject(nodeGrid.X +addXValue , nodeGrid.Y+addYValue) != null)
        {
            NodeGridSystem.NodeGridObject checkNode = grid.GetGridObject(nodeGrid.X + addXValue, nodeGrid.Y + addYValue);
            if (!CompleteNodeList.Contains(checkNode) && !CurrentNodeList.Contains(checkNode))
            {
                checkNode.SetMazeCoord(addXValue, addYValue);
                possibleNodeGridObjects.Add(checkNode);
            }

        }
    }

}
