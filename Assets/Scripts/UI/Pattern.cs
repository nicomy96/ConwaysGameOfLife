using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOfLife.Grid;
using UnityEngine.UI;

namespace GameOfLife.UI
{
    public class Pattern : MonoBehaviour
    {
        [SerializeField] PatternInfo patternInfo;
        GridManager gridManager;

        private void Start()
        {
            GetComponent<Image>().sprite = patternInfo.GetSprite();
        }

        public void SetPattern()
        {
            gridManager.SetCurrentPattern(patternInfo);
        }

    }
}
