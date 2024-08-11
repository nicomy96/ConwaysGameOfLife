using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace GameOfLife.Grid
{
    public class GridManager : MonoBehaviour
    { 
        public readonly int width = 160;
        public readonly int height = 90;
        public readonly float tileSize = 1f;

        List<Tile> grid = new List<Tile>();
        HashSet<int> aliveTiles = new HashSet<int>();
        List<int> changes = new List<int>();
        Stack<List<int>> history = new Stack<List<int>>();
        int currentGeneration;

        private void Start()
        {
            currentGeneration = 0;
        }
        public List<Tile> Grid
        {
            get { return grid;}
        }

        private void SaveAliveTiles(int id)
        {
            if (grid[id].IsAlive())
            {
                print("You are alive Tile No.  " + id);
                aliveTiles.Add(id);
            }
            else
            {
                aliveTiles.Remove(id);
            }
        } 
        public void SubscribeToTiles()
        {
            foreach (Tile tile in grid)
            {
                tile.StateChange += SaveAliveTiles;
            }
        }
        public void CalculateNextGeneration()
        {
            foreach(int tile in aliveTiles)
            {
                print(tile);
                CheckNeighbors(tile, true);
            }
            ApplyCurrentChanges();
            currentGeneration++;
        }

        public void ReturnPreviousGeneration()
        {
            if(history.Count > 0)
            {
                ApplyChanges(history.Pop());
                changes.Clear();
                currentGeneration--;
            }
        }

        private void CheckNeighbors(int id, bool checkNeighborsNextState)
        {
            int livingNeighbors = 0;
            foreach (int neighbor in grid[id].Neighbors)
            {
                if (grid[neighbor].IsAlive())
                {
                    livingNeighbors++;
                }
                else if(checkNeighborsNextState)
                {
                    CheckNeighbors(neighbor, false);
                }
            }
            CheckNextState(id, livingNeighbors);
        }

        private void CheckNextState(int id, int livingNeighbors)
        {
            if (grid[id].IsAlive())
            {
                if (livingNeighbors < 2 || livingNeighbors > 3)
                {
                    changes.Add(id);
                }
            }
            else if (livingNeighbors == 3)
            {
                    changes.Add(id);
            }
        }

        private void ApplyCurrentChanges()
        {
            history.Push(new List<int>(changes));
            ApplyChanges(changes);
            changes.Clear();
        }

        private void ApplyChanges(List<int> changesToApply)
        {
            foreach (int indexTile in changesToApply)
            {
                grid[indexTile].ChangeState();
            }
        }

        private void OnDisable()
        {
            foreach (Tile tile in grid)
            {
                tile.StateChange -= SaveAliveTiles;
            }
        }
    }
}
