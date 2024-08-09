using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace GameOfLife.Grid
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField] Tile tile;
        public CinemachineStateDrivenCamera stateDrivenCamera;

        readonly int width = 160;
        readonly int height = 90;
        readonly float tileSize = 1f;
      
        PolygonCollider2D polygonCollider;
        List<Tile> grid = new List<Tile>();

        Vector2 bottomLeft;
        Vector2 topRight;

        public Vector2 BottomLeft
        {
            get { return bottomLeft;}
        }

        public Vector2 TopRight
        {
            get { return topRight;}
        }


        private void Awake()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }
        void Start()
        {
            GenerateGrid(width, height);
            DefineWorldBounds();
        }

        private void GenerateGrid(int width, int height)
        {
            for (int x = 0; x < width; x ++)
            {
                for (int y = 0; y < height; y ++)
                {
                    Vector2 coordinates = new Vector2(x, y);
                    Tile newTile = Instantiate(tile, coordinates, Quaternion.identity);
                    newTile.Id = x * height + y;
                    newTile.transform.parent = transform;
                    grid.Add(newTile);
                }
            }
        }

        private void DefineWorldBounds()
        {
            Vector3 offset = Vector3.one * tileSize;
            bottomLeft = grid[0].transform.position - offset;
            topRight = grid[^1].transform.position + offset;
            Vector2[] points = { bottomLeft, new Vector2(bottomLeft.x, topRight.y), topRight, new Vector2(topRight.x, bottomLeft.y) };
            polygonCollider.SetPath(0, points);
            stateDrivenCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = polygonCollider;
        }

        public Vector2 GetGridCenter()
        {
            float offset = tileSize / 2;
            return new Vector2((width/2) - offset , (height/2) - offset); 
        }

    }
}
