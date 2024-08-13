using UnityEngine;
using GameOfLife.Grid;
using UnityEngine.InputSystem;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;

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
        bool isPlaying;

        private void Awake()
        {
            inputManager = new();
            gridSpawner = FindObjectOfType<GridSpawner>();
            gridManager = FindObjectOfType<GridManager>();
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            inputManager.Player.Enable();
            inputManager.Player.Zoom.performed += Zoom;
            inputManager.Player.PlayNextGeneration.performed += PlayNextGeneration;
            inputManager.Player.PlayPrevGeneration.performed += PlayPreviousGeneration;
            inputManager.Player.AutoPlay.performed += AutoPlayForward;
            inputManager.Player.BackwardAutoPlay.performed += BackwardAutoPlay;
        }
        private void Start()
        {
            SetStartPosition();
            isPlaying = false;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 direction = inputManager.Player.Move.ReadValue<Vector3>();
            if (direction == Vector3.zero) return;
            direction = transform.position + (fractionSpeed * Time.deltaTime * direction);
            direction.x = Mathf.Clamp(direction.x, gridSpawner.BottomLeft.x, gridSpawner.TopRight.x);
            direction.y = Mathf.Clamp(direction.y, gridSpawner.BottomLeft.y, gridSpawner.TopRight.y);
            transform.position = direction;
        }

        private void Zoom(InputAction.CallbackContext context)
        {
            float indexChange = inputManager.Player.Zoom.ReadValue<float>();
            if (indexChange == 0) return;
            indexChange = Mathf.Abs(indexChange) / indexChange;
            int cameras = gridSpawner.stateDrivenCamera.ChildCameras.Length;
            int currentCameraIndex = animator.GetInteger("CameraIndex");
            currentCameraIndex = Mathf.Clamp(currentCameraIndex + (int)indexChange, 0 ,cameras - 1);
            fractionSpeed = moveSpeed * ((cameras - currentCameraIndex) / (float) cameras);
            animator.SetInteger("CameraIndex", currentCameraIndex);
        }

        private void PlayNextGeneration(InputAction.CallbackContext context)
        {
            gridManager.CalculateNextGeneration();
        }

        private void PlayPreviousGeneration(InputAction.CallbackContext context)
        {
            gridManager.ReturnPreviousGeneration();
        }
        private void AutoPlayForward(InputAction.CallbackContext context)
        {
            if (!isPlaying)
            {
                gridManager.PlayForward();
            }
            else
            {
                gridManager.StopPlaying();
            }
            isPlaying = !isPlaying;

        }
        private void BackwardAutoPlay(InputAction.CallbackContext context)
        {
            if (!isPlaying)
            {
                gridManager.PlayBackward();
            }
            else
            {
                gridManager.StopPlaying();
                gridManager.PlayBackward();
            }
            
            isPlaying = !isPlaying;

        }
        private void SetStartPosition()
        {
            transform.position = gridSpawner.GetGridCenter();
        }

        private void OnDisable()
        {
            inputManager.Player.Zoom.performed -= Zoom;
            inputManager.Player.PlayNextGeneration.performed -= PlayNextGeneration;
            inputManager.Player.PlayPrevGeneration.performed -= PlayPreviousGeneration;
            inputManager.Player.AutoPlay.performed -= AutoPlayForward;
            inputManager.Player.BackwardAutoPlay.performed -= BackwardAutoPlay;
        }

    }
}
