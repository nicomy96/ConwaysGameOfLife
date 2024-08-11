using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace GameOfLife.Grid
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Color aliveColor = Color.black;
        [SerializeField] Color dethColor = Color.white;
        [SerializeField] Color hoverColor = Color.gray;

        TextMeshProUGUI positionText;
        SpriteRenderer spriteRenderer;

        public event Action<int> StateChange;

        List<int> neighbors = new List<int>();
        Color currentColor;
        bool isAlive;
        int id = -1;
        
        
        public int Id
        {
            get { return id; }
            set 
            {
                if (id != -1) return;
                id = value; 
            }
        }

        public List<int> Neighbors
        {
            get 
            { 
                return  neighbors;
            }
        }

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            positionText = FindObjectOfType<TextMeshProUGUI>();
        }

        private void Start()
        {
            isAlive = false;
            currentColor = spriteRenderer.color;
        }

        private void OnMouseEnter()
        {
            spriteRenderer.color = hoverColor;
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = currentColor;
        }

        private void OnMouseDown()
        {
            ChangeState();
            positionText.text = id.ToString();
        }

        public void ChangeState()
        {
            isAlive = !isAlive;
            currentColor = isAlive ? aliveColor : dethColor;
            spriteRenderer.color = currentColor;
            StateChange(id);
        }
       
        public bool IsAlive()
        {
            return isAlive;
        }
    }
}