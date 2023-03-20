
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
    [SerializeField] private Pathfinding pathfinding;

    [SerializeField] private SpriteObject sprites;
    // Start is called before the first frame update
  


    void Start()
    {
        grid = new Grid<NodeGridObject>(width, height, gridSize, Vector3.zero, (Grid < NodeGridObject > g, int x, int y) => new NodeGridObject( g, x, y,sprites));
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

    public void CleanUpToWhite()
    {
        for (int xWorld = 0; xWorld < grid.GetWidth(); xWorld++)
        {
            for (int yWorld = 0; yWorld < grid.GetHeight(); yWorld++)
            {
                NodeGridObject nodeGridObject = grid.GetGridObject(xWorld, yWorld);
                nodeGridObject.UpdateTileChecked(NodeGridObject.TileChecked.NotChecked);
            }
        }
    }
    public void StartPathfinding()
    {
        pathfinding.StartPathfinding();
    }

    private void DoMazeRound()
    {
        Vector2Int pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        NodeGridObject nodeGridObject = grid.GetGridObject(pos.x, pos.y);
        growingTree.StartGrowingTreeAlgortime(nodeGridObject, grid,this);
   
    }
    public NodeGridObject GiveRandomNode()
    {
        Vector2Int pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        return grid.GetGridObject(pos.x, pos.y);
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

        private int XMazeCoord;
        private int YMazeCoord;
        private Grid<NodeGridObject> grid;
        private Transform transformObject;
        private bool openTile=true;
        public enum TileChecked { NotChecked,Current,Checked,StartPoint,EndPoint};
        private TileChecked tileChecked;
        private string tileOpenConer = "";
        private SpriteRenderer render;
        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        private bool north = false;
        private bool south=false;
        private bool east=false;
        private bool west=false;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public bool North { get => north; set => north = value; }
        public bool South { get => south; set => south = value; }
        public bool East { get => east; set => east = value; }
        public bool West { get => west; set => west = value; }

        public NodeGridObject(Grid<NodeGridObject> grid, int x, int y, SpriteObject spritesObjects)
        {
            this.X = x;
            this.Y = y;
            this.grid = grid;



            for (int i = 0; i < spritesObjects.spriteObject.Count; i++)
            {
                SpriteList spriteListObject = spritesObjects.spriteObject[i];
                sprites.Add(spriteListObject.spriteName, spriteListObject.sprite);
            }
        }

        public void SetNode(Transform transformObject)
        {
            this.transformObject = transformObject;
            grid.TriggerGridObjectChange(X, Y);
            render = transformObject.GetComponent<SpriteRenderer>();
        }

        public void SetMazeCoord(int x, int y)
        {
            XMazeCoord = x;
            YMazeCoord = y;
        }

        public Vector2Int GetMazeCoord()
        {
            return new Vector2Int(XMazeCoord, YMazeCoord);
        }
        public void SetRenderColor(Color color)
        {
            render.color= color; 
        }

        public void SetGraphic()
        {
            string buildString="";
            if(north)
            {
                buildString += "N";
            }
            if (west)
            {
                buildString += "W";
            }
            if (south)
            {
                buildString += "S";
            }
            if (east)
            {
                buildString += "E";
            }
            if(sprites.ContainsKey(buildString))
            {
                render.sprite = sprites[buildString];
            }
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
                case TileChecked.StartPoint:
                    SetRenderColor(Color.green);
                    break;
                case TileChecked.EndPoint:
                    SetRenderColor(Color.red);
                    break;
                default:
                    SetRenderColor(Color.white);
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
