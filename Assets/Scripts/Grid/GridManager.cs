using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//using System;
using TMPro;
using System.Linq;

namespace GameOfLife.Grid
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI generation;
        [SerializeField] Pattern defaultPattern;
       
        public readonly int width = 160;
        public readonly int height = 90;
        public readonly float tileSize = 1f;

        Pattern currentPattern;
        List<Tile> grid = new();
        HashSet<int> aliveTiles = new();
        List<int> changes = new();
        Stack<List<int>> history = new();
        int currentGeneration;
        float delayNextGeneration;
        int startIndex;


        private void Start()
        {
            currentGeneration = 0;
            delayNextGeneration = 0.5f;
            currentPattern = defaultPattern;
        }
        public List<Tile> Grid
        {
            get { return grid;}
        }

        private void SaveAliveTiles(int id)
        {
            if (grid[id].IsAlive())
            {
                aliveTiles.Add(id);
            }
            else
            {
                aliveTiles.Remove(id);
            }
        } 

        private void RestartHistory()
        {
            if (currentGeneration == 0) return;
            history.Clear();
            changes.Clear();
            SetCurrentGeneration(0);
        }
        public void SubscribeToTiles()
        {
            foreach (Tile tile in grid)
            {
                tile.OnHover += DrawPatternShadow;
                tile.OnHoverExit += ErasePatternShadow;
                tile.OnStateChange += SaveAliveTiles;
                tile.OnManualStateChange += RestartHistory;
                tile.OnManualStateChange += DrawPattern;
            }
        }
        public void CalculateNextGeneration()
        {
            foreach(int tile in aliveTiles)
            {
                CheckNeighbors(tile, true);
            }
            ApplyCurrentChanges();
            SetCurrentGeneration(currentGeneration + 1);
        }

        public void RandomGeneration()
        {
            RestartHistory();
            int numberOfLiveCells = Random.Range(grid.Count / 4, grid.Count);
            for (int i = 0; i < numberOfLiveCells; i++)
            {
                changes.Add(Random.Range(0, grid.Count - 1));
            }
            ApplyChanges(changes);
        }

        public void ClearGrid()
        {
            RestartHistory();
            changes = new List<int>(aliveTiles);
            aliveTiles.Clear();
            foreach (int aliveTile in changes)
            {
                grid[aliveTile].ChangeState();
            }
            changes.Clear();
        }

        public void ReturnPreviousGeneration()
        {
            if(history.Count > 0)
            {
                ApplyChanges(history.Pop());
                changes.Clear();
                SetCurrentGeneration(currentGeneration - 1);
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
        }

        private void ApplyChanges(List<int> changesToApply)
        {
            foreach (int indexTile in changesToApply)
            {
                grid[indexTile].ChangeState();
            }
            changes.Clear();
        }

        public void PlayForward()
        {
            StartCoroutine(PlayForwardCoroutine());
        }

        public void PlayBackward()
        {
            StartCoroutine(PlayBackwardCoroutine());
        }

        public void StopPlaying()
        {
            StopAllCoroutines();
        }
        IEnumerator PlayForwardCoroutine()
        {
            while (aliveTiles.Count > 0)
            {
                CalculateNextGeneration();
                yield return new WaitForSeconds(delayNextGeneration);
            }
        }

        IEnumerator PlayBackwardCoroutine()
        {   
            while (history.Count > 0)
            {
                ReturnPreviousGeneration();
                yield return new WaitForSeconds(delayNextGeneration);
            }
        }

        private void SetCurrentGeneration(int newGeneration)
        {
            currentGeneration = newGeneration;
            generation.text = $"Generation: {currentGeneration}";
        }
        public void SetDelayNextGeneration(float newDelay)
        {
            delayNextGeneration = newDelay;
        }

        private void DrawPatternShadow(int tileIndex)
        {
            DefinePivotToDraw(tileIndex);
            startIndex = tileIndex + currentPattern.TopTilesNeeded() - (currentPattern.LeftTilesNeeded() * height);
            for(int i = 0; i < currentPattern.Height; i ++)
            {
                for(int j = 0; j < currentPattern.Width; j++)
                {
                    grid[startIndex - i + (j * height)].ChangeToHoverColor(currentPattern.IsActiveTile(i * currentPattern.Width + j));
                }
            }
        }

        private void DrawPattern()
        {
            for (int i = 0; i < currentPattern.Height; i++)
            {
                for (int j = 0; j < currentPattern.Width; j++)
                {
                    if (currentPattern.IsActiveTile(i * currentPattern.Width + j))
                        grid[startIndex - i + (j * height)].ChangeState();
                }
            }
        }

        private void ErasePatternShadow()
        {
            for (int i = 0; i < currentPattern.Height; i++)
            {
                for (int j = 0; j < currentPattern.Width; j++)
                {
                    grid[startIndex - i + (j * height)].ChangeToCurrentColor();
                }
            }
            currentPattern.RestartPivot();
        }

        private void DefinePivotToDraw(int tileIndex)
        {
            int leftTilesAvailable = Mathf.FloorToInt(tileIndex / height);
            int rightTilesAvailable = width - leftTilesAvailable - 1;
            int bottomTilesAvailable = tileIndex % height;
            int topTilesAvailabe = height - bottomTilesAvailable - 1;
            
            
            if(leftTilesAvailable < currentPattern.LeftTilesNeeded())
            {
                currentPattern.Pivot -= currentPattern.LeftTilesNeeded() - leftTilesAvailable;
            }
            if(rightTilesAvailable < currentPattern.RightTilesNeeded())
            {
                currentPattern.Pivot += currentPattern.RightTilesNeeded() - rightTilesAvailable;
            }
            if(bottomTilesAvailable < currentPattern.BottomTilesNeeded())
            {
                currentPattern.Pivot += (currentPattern.BottomTilesNeeded() - bottomTilesAvailable) * currentPattern.Width;
            }
            if(topTilesAvailabe < currentPattern.TopTilesNeeded())
            {
                currentPattern.Pivot -= (currentPattern.TopTilesNeeded() - topTilesAvailabe) * currentPattern.Width;
            }
        }
        private void OnDisable()
        {
            foreach (Tile tile in grid)
            {
                tile.OnHover -= DrawPatternShadow;
                tile.OnHoverExit -= ErasePatternShadow;
                tile.OnStateChange -= SaveAliveTiles;
                tile.OnManualStateChange -= RestartHistory;
                tile.OnManualStateChange -= DrawPattern;
            }
        }
    }
}
