using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XscoUtils;


//learnt from the Code Monkey 
public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangeEventArgs> OnGridObjectChange;
    public class OnGridObjectChangeEventArgs:EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3 orginPosition;

    public Grid(int width, int height, float cellSize, Vector3 orginPosition,Func<Grid<TGridObject>,int,int,TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.orginPosition = orginPosition; 

        gridArray = new TGridObject[this.width, this.height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x <gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }
        bool showDebug = true;

        if (showDebug)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                   // Debug.Log(x + ", " + y);
                    debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 8, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            OnGridObjectChange += (object sender, OnGridObjectChangeEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize+ orginPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition- orginPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition- orginPosition).y / cellSize);
    }

    public void SetGridObejct(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChange != null)
            {
                OnGridObjectChange(this, new OnGridObjectChangeEventArgs { x = x, y = y });
            }
        }
    }

    public void TriggerGridObjectChange(int x, int y)
    {
        if(OnGridObjectChange!=null)
        {
            OnGridObjectChange(this, new OnGridObjectChangeEventArgs { x = x, y = y });
        }
    }


    public void SetGridObejct(Vector3 worldPosition, TGridObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetGridObejct(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x,y);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    
}
