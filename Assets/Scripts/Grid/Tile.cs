using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace GameOfLife.Grid
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Color aliveColor = Color.black;
        [SerializeField] Color dethColor = Color.white;
        [SerializeField] Color hoverColorActive = Color.gray;
        [SerializeField] Color hoverColorInactive = Color.white;
        SpriteRenderer spriteRenderer;

        public event Action<int> OnStateChange;
        public event Action<int> OnHover;
        public event Action OnHoverExit;
        public event Action OnManualStateChange;

        List<int> neighbors = new();
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
            
        }

        private void Start()
        {
            isAlive = false;
            currentColor = spriteRenderer.color;
        }

        private void OnMouseEnter()
        {
            OnHover(id);
        }

        private void OnMouseExit()
        {
            OnHoverExit();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            OnManualStateChange();
        }

        public void ChangeState()
        {
            isAlive = !isAlive;
            currentColor = isAlive ? aliveColor : dethColor;
            spriteRenderer.color = currentColor;
            OnStateChange(id);
        }
       
        public bool IsAlive()
        {
            return isAlive;
        }

        public void ChangeToHoverColor(bool active)
        {
            spriteRenderer.color = active? hoverColorActive : hoverColorInactive;
        }

        public void ChangeToCurrentColor()
        {
            spriteRenderer.color = currentColor; ;
        }
    }
}