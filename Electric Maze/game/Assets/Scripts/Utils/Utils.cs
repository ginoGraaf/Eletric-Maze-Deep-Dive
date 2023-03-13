using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace XscoUtils
{
    public static class Utils
    {
        public static TextMesh CreateWorldText(string text, Transform parent=null, Vector3 localPosition=default(Vector3), int fontsize=10, Color color=new Color(), TextAnchor textAnchor=TextAnchor.MiddleCenter, TextAlignment textAlignment= TextAlignment.Center, int sortingOrder=0)
        {
            if(color==null)
                color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontsize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition,int fontsize, Color color,TextAnchor textAnchor,TextAlignment textAlignment,int sortingOrder)
        {
            GameObject gameObject = new GameObject("world_text", typeof(TextMesh));
            Transform transform= gameObject.transform;
            transform.SetParent(parent,false);
            transform.localPosition = localPosition;
            TextMesh textMesh=gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize= fontsize;
            textMesh.color= color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        public static Vector3 GetWorldPosition()
        {
            Vector3 vec = GetmouseWordlPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0; ;
            return vec;

        }

        public static Vector3 GetmouseWordlPositionWithZ()
        {
            return GetmouseWordlPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetmouseWordlPositionWithZ(Camera worldCamera)
        {
            return GetmouseWordlPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetmouseWordlPositionWithZ(Vector3 screenPosition,Camera worldCamera) 
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        private static readonly Vector3 Vector3Zero = Vector3.zero;
        private static readonly Vector3 Vector3One = Vector3.one;
        private static readonly Vector3 Vector3Down = new Vector3(0, -1);

        private static Quaternion[] chachedQuaternionEulerArr;

        private static void ChacheQuaternionEuler()
        {
            if(chachedQuaternionEulerArr ==null)
            {
                chachedQuaternionEulerArr = new Quaternion[360];
            }
            for (int i = 0; i < 360; i++)
            {
                chachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, 0);
            }
        }

        public static void CreateEmptyMeshArrays(int quadCount,out Vector3[] vertices,out Vector2[] uvs,out int[]triangles)
        {
            vertices = new Vector3[4*quadCount];
            uvs = new Vector2[4 * quadCount];
            triangles = new int[6 * quadCount];
        }

        public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs,int index, int[]triangles,Vector3 pos, float rot, Vector3 cellSize, Vector2 uv00, Vector2 uv11)
        {
            int VIndex = index * 4;
            int vIndex0 = VIndex;
            int vIndex1 = VIndex+1;
            int vIndex2 = VIndex+2;
            int vIndex3 = VIndex+3;


            cellSize *= 0.5f;
            bool skewd = cellSize.x != cellSize.y;

            if (skewd)
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-cellSize.x, cellSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-cellSize.x, -cellSize.y);
                vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(cellSize.x, -cellSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rot) * cellSize;
            }
            else
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot-270) * new Vector3(-cellSize.x, cellSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rot-180) * new Vector3(-cellSize.x, -cellSize.y);
                vertices[vIndex2] = pos + GetQuaternionEuler(rot-90) * new Vector3(cellSize.x, -cellSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rot-0) * cellSize;
            }

            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

            int tIndex = index * 6;

            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex3;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex3;
            triangles[tIndex + 5] = vIndex2;
        }

        public static void MeshUV(Vector3[] vertices, Vector3 pos, float rot, int index, Vector2[] uvs, Vector3 cellSize, Vector2 uv00, Vector2 uv11)
        {
            int VIndex = index * 4;
            int vIndex0 = VIndex;
            int vIndex1 = VIndex + 1;
            int vIndex2 = VIndex + 2;
            int vIndex3 = VIndex + 3;

            cellSize *= 0.5f;
            bool skewd = cellSize.x != cellSize.y;

            if (skewd)
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-cellSize.x, cellSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-cellSize.x, -cellSize.y);
                vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(cellSize.x, -cellSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rot) * cellSize;
            }
            else
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * new Vector3(-cellSize.x, cellSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * new Vector3(-cellSize.x, -cellSize.y);
                vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * new Vector3(cellSize.x, -cellSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * cellSize;
            }


            cellSize *= 0.5f;

            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
        }

        public static Quaternion GetQuaternionEuler(float rotation)
        {
            int rot = Mathf.RoundToInt(rotation);
            rot = rot % 360;
            if (rot < 0) rot += 360;

            if(chachedQuaternionEulerArr==null)
            {
                ChacheQuaternionEuler();
            }
            return chachedQuaternionEulerArr[rot];
        }
    }
}
