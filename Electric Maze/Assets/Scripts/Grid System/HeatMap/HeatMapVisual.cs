using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XscoUtils;
using UnityEditor;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class HeatMapVisual : MonoBehaviour
{
    private Grid<HeatMapGradObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<HeatMapGradObject> grid)
    {
        this.grid = grid;
        UpdateTileVisual();
        grid.OnGridObjectChange += Grid_OnGridValueChange;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTileVisual();
        }
    }

    private void Grid_OnGridValueChange(object sender, Grid<HeatMapGradObject>.OnGridObjectChangeEventArgs e)
    {
        // UpdateTileVisual();
        updateMesh = true;
    }

    private void UpdateTileVisual()
    {
        Utils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                HeatMapGradObject heatmap= grid.GetGridObject(x, y);
                float heatValue=(float) heatmap.GetValueNormalized();

                Vector2 gridValueUV = new Vector2(heatValue, 0);
                Utils.AddToMeshArrays(vertices, uv, index, triangles, grid.GetWorldPosition(x, y)+quadSize*0.5f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
