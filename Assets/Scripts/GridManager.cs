using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameOfLife.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] float width;
        [SerializeField] float height;
        float cameraDistance = -10f;
        [SerializeField] int tileSize;
        [SerializeField] Tile tile;
        
        Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();

        void Start()
        {
            GenerateGrid(width, height);
            SetCameraPosition(width / 2, height / 2);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateGrid(width, height);
            }
        }

        private void GenerateGrid(float width, float height)
        {
            ClearGrid();
            width /= tileSize;
            height /= tileSize;
            for (float x = (float)(tileSize - 1) / 2; x < width * tileSize; x += tileSize)
            {
                for (float y = (float)(tileSize - 1) / 2; y < height * tileSize; y += tileSize)
                {
                    Vector2 coordinates = new Vector2(x, y);
                    Tile newTile = Instantiate(tile, coordinates, Quaternion.identity);
                    newTile.transform.localScale *= (tileSize - 0.1f);
                    newTile.transform.parent = transform;
                    grid[coordinates] = newTile;
                }
            }
        }

        private void ClearGrid()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            grid.Clear();
        }
        private void SetCameraPosition(float x, float y)
        {
            Camera.main.transform.position = new Vector3(x - 0.5f, y - 0.5f, cameraDistance);
        }
    }
}
