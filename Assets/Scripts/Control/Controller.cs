using UnityEngine;
using GameOfLife.Grid;

namespace GameOfLife.Control
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        InputManager inputManager;
        GridSpawner gridSpawner;
        GridManager gridManager;
        Animator animator;
        float fractionSpeed;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            gridSpawner = FindObjectOfType<GridSpawner>();
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
            if (Input.GetKeyDown(KeyCode.L))
            {
                gridManager.CalculateNextGeneration();
            }else if (Input.GetKeyDown(KeyCode.K))
            {
                gridManager.ReturnPreviousGeneration();
            }
            Move();
        }

        private void Move()
        {
            if (inputManager.direction == Vector3.zero) return;
            Vector3 newPosition = transform.position + (fractionSpeed * Time.deltaTime * inputManager.direction);
            newPosition.x = Mathf.Clamp(newPosition.x, gridSpawner.BottomLeft.x, gridSpawner.TopRight.x);
            newPosition.y = Mathf.Clamp(newPosition.y, gridSpawner.BottomLeft.y, gridSpawner.TopRight.y);
            transform.position = newPosition;
        }

        private void Zoom(int indexChange)
        {
            if (indexChange == 0) return;
            int cameras = gridSpawner.stateDrivenCamera.ChildCameras.Length;
            int currentCameraIndex = animator.GetInteger("CameraIndex");
            currentCameraIndex = Mathf.Clamp(currentCameraIndex + indexChange, 0 ,cameras - 1);
            fractionSpeed = moveSpeed * ((cameras - currentCameraIndex) / (float) cameras);
            animator.SetInteger("CameraIndex", currentCameraIndex);
        }

        private void SetStartPosition()
        {
            transform.position = gridSpawner.GetGridCenter();
        }

        private void OnDisable()
        {
            inputManager.Zoom -= Zoom;
        }

    }
}
