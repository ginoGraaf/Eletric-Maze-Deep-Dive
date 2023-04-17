using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerveSpiralPath : MonoBehaviour
{
    [SerializeField] private NodeGridSystem nodeGridSystem;
    [SerializeField] private GameObject nervePrefab;
    [SerializeField] private GameObject crossingPrefabsRD, crossingPrefabsLD, crossingPrefabsRU, crossingPrefabsLU;
    private List<PathfindingNodes.PathNodeObject> path = new List<PathfindingNodes.PathNodeObject>();
    private Grid<NerveSpiralObject> grid;

    float gridSize;
    public void SetUpGrid(int width, int height, float gridSize)
    {
        this.gridSize = gridSize;
        grid = new Grid<NerveSpiralObject>(width, height, 3f, Vector3.zero, (Grid<NerveSpiralObject> g, int x, int y) => new NerveSpiralObject(g, x, y));
    }

    public void PlaceGameObject(List<PathfindingNodes.PathNodeObject> nodes)
    {
        path = nodes;
        //[Reminder]Use the NodegridSystem with the North,East,South,West booleans to dictate if it is a turn or not.. be carful because some are also crossing.....
        //or you make a correction after it is done making the world...

        for (int i = 0; i < path.Count; i++)
        {
            int x = path[i].GetX();
            int y = path[i].GetY();
            GameObject nerveObject = Instantiate(nervePrefab);
         
            nerveObject.transform.position = new Vector3(x* gridSize, y* gridSize, 0);
            grid.GetGridObject(x, y).SetGameObject(nerveObject);
        }
        Correcting();
    }

    void Correcting()
    {
        NerveSpiralObject spiral = null;
        NerveSpiralObject end = null;
        for (int i = 0; i < path.Count; i++)
        {
           
            int x = path[i].GetX();
            int y = path[i].GetY();

            spiral = grid.GetGridObject(x, y);
            NerveSpiralObject N=grid.GetGridObject(x,y+1);
            NerveSpiralObject E = grid.GetGridObject(x+1, y );
            NerveSpiralObject S = grid.GetGridObject(x, y - 1);
            NerveSpiralObject W = grid.GetGridObject(x-1, y);

            ChangeDirection(N, E, S, W,spiral);
        }
        CorrectLastPart(spiral = grid.GetGridObject(path[0].GetX(), path[0].GetY()));
        CorrectLastPart(spiral = grid.GetGridObject(path[path.Count-1].GetX(), path[path.Count-1].GetY()));
    }

    private void CorrectLastPart(NerveSpiralObject spiral)
    {
       
        NerveSpiralObject E = grid.GetGridObject(spiral.X + 1, spiral.Y);
        NerveSpiralObject W = grid.GetGridObject(spiral.X - 1, spiral.Y);
        if (E != null)
        {
            if (E.GetspiralObject() != null)
            {
                spiral.GetspiralObject().transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }
        if (W != null)
        {
            if (W.GetspiralObject() != null)
            {
                spiral.GetspiralObject().transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }

    }

    //not so proud on this part. can i do better.... becuase this was a pain to make.
    private void ChangeDirection(NerveSpiralObject N, NerveSpiralObject E, NerveSpiralObject S, NerveSpiralObject W,NerveSpiralObject spiral)
    {
        bool north = false, east = false, south = false, west = false;
        if (N != null)
        {
            NodeGridSystem.NodeGridObject obj = nodeGridSystem.GetNodeGrid(spiral.X, spiral.Y);
            if (N.GetspiralObject() != null && obj.North)
            {
                north = true;
            }
        }
        if (E != null)
        {
            NodeGridSystem.NodeGridObject obj = nodeGridSystem.GetNodeGrid(spiral.X, spiral.Y);
            if (E.GetspiralObject() != null && obj.West)
            {
                east = true;
            }
        }
        if (S != null)
        {
            NodeGridSystem.NodeGridObject obj = nodeGridSystem.GetNodeGrid(spiral.X, spiral.Y);
            if (S.GetspiralObject() != null && obj.South)
            {
                south = true;
            }
        }
        if (W != null)
        {
            NodeGridSystem.NodeGridObject obj = nodeGridSystem.GetNodeGrid(spiral.X, spiral.Y);
            if (W.GetspiralObject() != null && obj.East)
            {
                west = true;
            }
        }
        if (east && west)
        {
            GameObject game = spiral.GetspiralObject();
            game.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (north && east)
        {
            spiral.DeleteGaeObject();
            GameObject nerveObject = Instantiate(crossingPrefabsRD);
            nerveObject.transform.position = new Vector3(spiral.X* gridSize, spiral.Y* gridSize, 0);
            grid.GetGridObject(spiral.X, spiral.Y).SetGameObject(nerveObject);
        }
        else if (north && west)
        {
            spiral.DeleteGaeObject();
            GameObject nerveObject = Instantiate(crossingPrefabsLD);
            nerveObject.transform.position = new Vector3(spiral.X * gridSize, spiral.Y * gridSize, 0);
            grid.GetGridObject(spiral.X, spiral.Y).SetGameObject(nerveObject);
        }
        else if (south && east)
        {
            spiral.DeleteGaeObject();
            GameObject nerveObject = Instantiate(crossingPrefabsRU);
            nerveObject.transform.position = new Vector3(spiral.X * gridSize, spiral.Y * gridSize, 0);
            grid.GetGridObject(spiral.X, spiral.Y).SetGameObject(nerveObject);
        }
        else if (south && west)
        {
            spiral.DeleteGaeObject();
            GameObject nerveObject = Instantiate(crossingPrefabsLU);
            nerveObject.transform.position = new Vector3(spiral.X * gridSize, spiral.Y * gridSize, 0);
            grid.GetGridObject(spiral.X, spiral.Y).SetGameObject(nerveObject);
        }
    }



    public class NerveSpiralObject
    {
        private int x;
        private int y;
        private Grid<NerveSpiralObject> grid;
        private GameObject nerveSpiralGameObject;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public NerveSpiralObject(Grid<NerveSpiralObject> grid,int x, int y)
        {
            this.grid = grid;
            this.X = x;
            this.Y = y;
        }
        public void SetGameObject(GameObject nerveObject)
        {
            if (nerveSpiralGameObject == null)
            {
                nerveSpiralGameObject = nerveObject;
            }
            grid.TriggerGridObjectChange(X, Y);
        }

        public GameObject GetspiralObject()
        {
            return nerveSpiralGameObject;
        }

        public void DeleteGaeObject()
        {
            if(nerveSpiralGameObject!=null)
            {
                Destroy(nerveSpiralGameObject);
                grid.TriggerGridObjectChange(X, Y);
            }
        }

        public override string ToString()
        {
            return "";
        }
    }
}
