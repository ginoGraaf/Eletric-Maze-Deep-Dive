using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XscoUtils;

public class TestingGrid : MonoBehaviour
{
    private Grid<BooleanGrid> grid;
    private Grid<HeatMapGradObject> heatmap;
    [SerializeField] private HeatMapVisual tileMapVisual;
    private void Start()
    {
        //grid = new Grid<BooleanGrid>(4, 2, 10f, new Vector3(-50,0,0),(Grid<BooleanGrid> g, int x,int y)=>new BooleanGrid(g,x,y));
        heatmap= new Grid<HeatMapGradObject>(40, 40, 1f, Vector3.zero,(Grid<HeatMapGradObject> g,int x,int y)=>new HeatMapGradObject(g,x,y));
        tileMapVisual.SetGrid(heatmap);
    }

    private void Update()
    {
        Vector3 position = Utils.GetWorldPosition();
        if (Input.GetMouseButtonUp(0))
        {
            //grid.SetValue( Utils.GetWorldPosition(),true);
            HeatMapGradObject heatMapGradObject = heatmap.GetGridObject(position);
            if(heatMapGradObject!=null)
            {
                heatMapGradObject.AddValueRange(position, 100, 5,40);
               // heatMapGradObject.AddValue(5);
            }
        
        }
        if(Input.GetMouseButtonUp(1))
        {
            HeatMapGradObject heatMapGradObject = heatmap.GetGridObject(position);
            if (heatMapGradObject != null)
            {
                heatMapGradObject.AddValueRange(position, 100, -40,0);
            }
            // grid.SetValue(Utils.GetWorldPosition(), false);
        }
    }


}

public class BooleanGrid
{
    public bool gridState;
    private int x;
    private int y;

    private Grid<BooleanGrid> grid;

    public BooleanGrid(Grid<BooleanGrid> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void SetBool()
    {
        gridState = !gridState;
        grid.TriggerGridObjectChange(x, y);
    }

    public override string ToString()
    {
        return gridState.ToString();
    }
}

public class HeatMapGradObject
{
    public int value;
    private const int MIN = 0;
    private const int MAX = 255;
    private int x;
    private int y;

    private Grid<HeatMapGradObject> grid;

    public HeatMapGradObject(Grid<HeatMapGradObject> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;

    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChange(x, y);
    }


    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }
    public override string ToString()
    {
        return value.ToString();
    }

    public void AddValueRange(Vector3 worldPosition, int value, int fullValueRange,int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)(value) / (totalRange - fullValueRange));
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int raduis = x + y;
                int addValueAmount = value;
                if (raduis > fullValueRange)
                {
                    addValueAmount -= lowerValueAmount * (raduis - fullValueRange);
                }
                
                UpdateMap(DiamondSide((int)worldPosition.x + (x * (int)grid.GetCellSize()), (int)worldPosition.y + (y * (int)grid.GetCellSize())),addValueAmount);
                if (x != 0)
                {
                    UpdateMap(DiamondSide((int)worldPosition.x - (x * (int)grid.GetCellSize()), (int)worldPosition.y + (y * (int)grid.GetCellSize())), addValueAmount);
                }
                if(y!=0)
                {
                    UpdateMap(DiamondSide((int)worldPosition.x + (x * (int)grid.GetCellSize()), (int)worldPosition.y - (y * (int)grid.GetCellSize())), addValueAmount);
                    if(x!=0)
                    {
                        UpdateMap(DiamondSide((int)worldPosition.x - (x * (int)grid.GetCellSize()), (int)worldPosition.y - (y * (int)grid.GetCellSize())), addValueAmount);
                    }
                }

            }
        }
    }

    private HeatMapGradObject DiamondSide(int x, int y)
    {
        Vector3 worldObjectPos = new Vector3(x, y);
        return grid.GetGridObject(worldObjectPos);
    }

    private void UpdateMap(HeatMapGradObject heatMap,int value)
    {
        if (heatMap != null)
            heatMap.AddValue(value);
    }
}
