
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NodeGridSystem;

public class NodeGridSystem : MonoBehaviour
{
    [SerializeField] private float gridSize;
    [SerializeField] private int width;
    [SerializeField] private int height;
    private Grid<NodeGridObject> grid;
    [SerializeField] private Transform MazeNodePrefab;
    [SerializeField] private GrowingTree growingTree;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<NodeGridObject>(width, height, gridSize, Vector3.zero, (Grid < NodeGridObject > g, int x, int y) => new NodeGridObject( g, x, y));
        SetMaze();
        DoMazeRound();
    }

    private void SetMaze()
    {
        for (int xWorld = 0; xWorld <grid.GetWidth() ; xWorld++)
        {
            for (int yWorld = 0; yWorld < grid.GetHeight(); yWorld++)
            {
                Transform nodeObject=Instantiate(MazeNodePrefab);
                nodeObject.transform.localPosition = new Vector3(xWorld,yWorld ,0);
                grid.GetGridObject(xWorld, yWorld).SetNode(nodeObject);
            }
        }
    }

    private void DoMazeRound()
    {
        Vector2Int pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        NodeGridObject nodeGridObject = grid.GetGridObject(pos.x, pos.y);
        growingTree.StartGrowingTreeAlgortime(nodeGridObject, grid);
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
        private bool openTile=true;
        public enum TileChecked { NotChecked,Current,Checked};
        private TileChecked tileChecked;
        private string tileOpenConer = "";
        private SpriteRenderer render;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public NodeGridObject(Grid<NodeGridObject> grid, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.grid = grid;
        }

        public void SetNode(Transform transformObject)
        {
            this.transformObject = transformObject;
            grid.TriggerGridObjectChange(X, Y);
            render = transformObject.GetComponent<SpriteRenderer>();
        }

        public void SetRenderColor(Color color)
        {
            render.color= color; 
        }

        public Transform GetNode()
        {
            return transformObject;
        }

        public override string ToString()
        {
            string debugText = "";

            return debugText;
        }

        public TileChecked IsTileChecked()
        {
            return tileChecked;
        }
        public void UpdateTileChecked(TileChecked tileChecked)
        {
            this.tileChecked = tileChecked;
            switch (tileChecked)
            {
                case TileChecked.Current:
                    SetRenderColor(Color.yellow);
               break;
                case TileChecked.Checked:
                    SetRenderColor(Color.blue);
                    break;

            }
  
        }
        public string GetTileCorner()
        {
            return tileOpenConer;
        }
        public void AddTileCornerCharacter(string tileCornerCharacter)
        {
            tileOpenConer += tileCornerCharacter;
        }
    }
}
