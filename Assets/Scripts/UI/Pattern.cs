using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOfLife.Grid;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

namespace GameOfLife.UI
{
    public class Pattern : MonoBehaviour
    {
        [SerializeField] PatternInfo patternInfo;
        GridManager gridManager;
        UIManager uimanager;
       

        private void Start()
        {
            SetImage();
            SetText();
            uimanager = FindObjectOfType<UIManager>();
            gridManager = FindObjectOfType<GridManager>();
        }

        public void SetPattern()
        {
            gridManager.SetCurrentPattern(patternInfo);
            uimanager.DisplayPatternsCanvas();
        }
        public void SetImage()
        {
            GetComponent<Image>().sprite = patternInfo.GetSprite();
        }

        public void SetText()
        {
            TextMeshProUGUI name = GetComponentInChildren<TextMeshProUGUI>();
            name.text = patternInfo.Name;
        }

    }
}
