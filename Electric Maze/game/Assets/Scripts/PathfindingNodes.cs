using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNodes : MonoBehaviour
{
    private Grid<PathNodeObject> grid;
    public void SetUpPathfinding(int width,int height,float gridSize)
    {
        grid = new Grid<PathNodeObject>(width, height, gridSize, Vector3.zero, (Grid<PathNodeObject> g, int x, int y) => new PathNodeObject(g, x, y));
    }

    public PathNodeObject GetNode(int x, int y)
    {
        if(grid.GetGridObject(x,y)!=null)
        {
            return grid.GetGridObject(x, y);
        }
        return null;
    }

    public class PathNodeObject
    {
        private int x;
        private int y;
        private float f;
        private float g=int.MaxValue;
        private float h=1;
        private PathNodeObject parent;
        Grid<PathNodeObject> grid;

        public PathNodeObject(Grid<PathNodeObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void CalculateFScore()
        {
            f = h + g;
            grid.TriggerGridObjectChange(x,y);//only for visual map
        }
        public void CalculateHScore(PathNodeObject endGoal)
        {
            //mathatten calculation
            h = (Mathf.Abs(this.x - endGoal.x) + Mathf.Abs(this.y - endGoal.y));
        }

        public void CalculatedGScore(float score)
        {
            g = score;
        }
        public float GetG()
        {
            return g;
        }
        public float GetH()
        {
            return h;
        }
        public float GetF()
        {
            return f;
        }

        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public void SetNodeParent(PathNodeObject node)
        {
            parent = node;
        }
        public PathNodeObject GetParent()
        {
            return parent;
        }
        public override string ToString()
        {
            return "";
        }
    }


}
