using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameOfLife.Grid
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Color aliveColor = Color.black;
        [SerializeField] Color dethColor = Color.white;
        [SerializeField] Color hoverColor = Color.gray;
        TextMeshProUGUI positionText;
        SpriteRenderer spriteRenderer;
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
            isAlive = !isAlive;
            currentColor = isAlive ? aliveColor : dethColor;
            spriteRenderer.color = currentColor;
            positionText.text = transform.position.ToString();
        }
    }
}