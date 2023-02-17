using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGridSystem : MonoBehaviour
{
    private Grid<NodeGridObject> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<NodeGridObject>(40, 40, 1f, Vector3.zero, (Grid < NodeGridObject > g, int x, int y) => new NodeGridObject( g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public class NodeGridObject
    {
        /*
         * x and y is the position.
         * grid is a call function if something has change
         * openTile is for pathfinding to check if it's walkable or not.
         * transform object is to see all the nodes when creating the maze.
         */

        private int x;
        private int y;
        private Grid<NodeGridObject> grid;
        private Transform transformObject;
        private bool OpenTile=true;

        public NodeGridObject(Grid<NodeGridObject> grid, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.grid = grid;
        }

        public void SetNode(Transform transformObject)
        {
            this.transformObject = transformObject;
            grid.TriggerGridObjectChange(x, y);
        }

        public override string ToString()
        {
            string debugText = "";

            return debugText;
        }
    }
}
