using Cinemachine;
using System.Collections.Generic;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using System;
using TMPro;

namespace GameOfLife.Grid
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI generation;
        public readonly int width = 160;
        public readonly int height = 90;
        public readonly float tileSize = 1f;

        List<Tile> grid = new List<Tile>();
        HashSet<int> aliveTiles = new HashSet<int>();
        List<int> changes = new List<int>();
        Stack<List<int>> history = new Stack<List<int>>();
        int currentGeneration;
        float delayNextGeneration;


        private void Start()
        {
            currentGeneration = 0;
            delayNextGeneration = 0.5f;
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
            history.Clear();
            SetCurrentGeneration(0);
        }
        public void SubscribeToTiles()
        {
            foreach (Tile tile in grid)
            {
                tile.OnStateChange += SaveAliveTiles;
                tile.OnManualStateChange += RestartHistory;
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

        public void ReturnPreviousGeneration()
        {
            print("Return previous generation");
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
                tile.OnStateChange -= SaveAliveTiles;
            }
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
    }
}
