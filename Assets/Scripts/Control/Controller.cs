using UnityEngine;
using GameOfLife.Grid;

namespace GameOfLife.Control
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        InputManager inputManager;
        GridManager gridManager;
        Animator animator;
        float fractionSpeed;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            gridManager = FindObjectOfType<GridManager>();
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            inputManager.Zoom += Zoom;
        }
        private void Start()
        { 
            SetStartPosition();
        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (inputManager.direction == Vector3.zero) return;
            Vector3 newPosition = transform.position + (fractionSpeed * Time.deltaTime * inputManager.direction);
            newPosition.x = Mathf.Clamp(newPosition.x, gridManager.BottomLeft.x, gridManager.TopRight.x);
            newPosition.y = Mathf.Clamp(newPosition.y, gridManager.BottomLeft.y, gridManager.TopRight.y);
            transform.position = newPosition;
        }

        private void Zoom(int indexChange)
        {
            if (indexChange == 0) return;
            int cameras = gridManager.stateDrivenCamera.ChildCameras.Length;
            int currentCameraIndex = animator.GetInteger("CameraIndex");
            currentCameraIndex = Mathf.Clamp(currentCameraIndex + indexChange, 0 ,cameras - 1);
            fractionSpeed = moveSpeed * ((cameras - currentCameraIndex) / (float) cameras);
            animator.SetInteger("CameraIndex", currentCameraIndex);
        }

        private void SetStartPosition()
        {
            transform.position = gridManager.GetGridCenter();
        }

        private void OnDisable()
        {
            inputManager.Zoom -= Zoom;
        }

    }
}
