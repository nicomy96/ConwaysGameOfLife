using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace GameOfLife.Grid
{
    public class GridSpawner: MonoBehaviour
    {

        [SerializeField] Tile tile;
        public CinemachineStateDrivenCamera stateDrivenCamera;
        Vector2 bottomLeft;
        Vector2 topRight;
        GridManager gridManager;
        PolygonCollider2D polygonCollider;

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
            gridManager = GetComponent<GridManager>();
        }
        void Start()
        {
            GenerateGrid(gridManager.width, gridManager.height);
            gridManager.SubscribeToTiles();
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
                    DefineNeighbors(newTile.Id, newTile.Neighbors);
                    newTile.transform.parent = transform;
                    gridManager.Grid.Add(newTile);
                }
            }
        }

        private void DefineWorldBounds()
        {
            Vector3 offset = Vector3.one * gridManager.tileSize;
            bottomLeft = gridManager.Grid[0].transform.position - offset;
            topRight = gridManager.Grid[^1].transform.position + offset;
            Vector2[] points = { bottomLeft, new Vector2(bottomLeft.x, topRight.y), topRight, new Vector2(topRight.x, bottomLeft.y) };
            polygonCollider.SetPath(0, points);
            stateDrivenCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = polygonCollider;
        }
        public Vector2 GetGridCenter()
        {
            float offset = gridManager.tileSize / 2;
            return new Vector2((gridManager.width/2) - offset , (gridManager.height/2) - offset); 
        }

        private void DefineNeighbors(int id, List<int> neighbors)
        {
            int bottomNeighbor = id - 1;
            int topNeighbor = id + 1;
            int leftNeighbor = id - gridManager.height;
            int rightNeighbor = id + gridManager.height;


            if(HasBottomNeighbor(id)) 
            {
                neighbors.Add(bottomNeighbor);
                if(HasRightNeighbor(bottomNeighbor))
                {
                    neighbors.Add(bottomNeighbor + gridManager.height);
                }
                if (HasLeftNeighbor(bottomNeighbor))
                {
                    neighbors.Add(bottomNeighbor - gridManager.height);
                }
            }
            if(HasTopNeighbor(id)) 
            {
                neighbors.Add(topNeighbor);
                if (HasRightNeighbor(topNeighbor))
                {
                    neighbors.Add(topNeighbor + gridManager.height);
                }
                if (HasLeftNeighbor(topNeighbor))
                {
                    neighbors.Add(topNeighbor - gridManager.height);
                }
            }

            if(HasLeftNeighbor(id)) neighbors.Add(leftNeighbor);
            if(HasRightNeighbor(id)) neighbors.Add(rightNeighbor);
        }
        private bool HasBottomNeighbor(int id)
        {
            if (id % gridManager.height == 0) return false;
            return true;
        }

        private bool HasTopNeighbor(int id)
        {
            if (id % gridManager.height == gridManager.height - 1) return false;
            return true;
        }

        private bool HasLeftNeighbor(int id)
        {
            if (id < gridManager.height) return false;
            return true;
        }
        private bool HasRightNeighbor(int id)
        {
            if (id >= (gridManager.height * (gridManager.width-1))) return false;
            return true;
        }
    }
}
