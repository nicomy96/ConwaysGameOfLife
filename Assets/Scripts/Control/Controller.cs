using UnityEngine;
using GameOfLife.Grid;
using UnityEngine.InputSystem;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameOfLife.Control
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] float playSpeed;
        [SerializeField] float moveSpeed;
        [SerializeField] Slider slider;
        InputManager inputManager;
        InputManager.PlayerActions playerInput;
        GridSpawner gridSpawner;
        GridManager gridManager;
        Animator animator;
        float fractionSpeed;
        bool isPlaying;

        private void Awake()
        {
            inputManager = new();
            playerInput = inputManager.Player;
            gridSpawner = FindObjectOfType<GridSpawner>();
            gridManager = FindObjectOfType<GridManager>();
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            playerInput.Enable();
            playerInput.Zoom.performed += Zoom;
            playerInput.PlayNextGeneration.performed += PlayNextGeneration;
            playerInput.PlayPrevGeneration.performed += PlayPreviousGeneration;
            playerInput.AutoPlay.performed += ForwardAutoPlay;
            playerInput.BackwardAutoPlay.performed += BackwardAutoPlay;
            playerInput.RandomGeneration.performed += RandomGeneration;
            playerInput.Clear.performed += ClearGrid;
            playerInput.SetPatternAsDefault.performed += SetPatternAsDefault;
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
            Vector3 direction = inputManager.Player.Move.ReadValue<Vector2>();
            if (direction == Vector3.zero) return;
            direction = transform.position + (fractionSpeed * Time.deltaTime * direction);
            direction.x = Mathf.Clamp(direction.x, gridSpawner.BottomLeft.x, gridSpawner.TopRight.x);
            direction.y = Mathf.Clamp(direction.y, gridSpawner.BottomLeft.y, gridSpawner.TopRight.y);
            transform.position = direction;
        }

        private void Zoom(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            float indexChange = inputManager.Player.Zoom.ReadValue<float>();
            if (indexChange == 0) return;
            indexChange = Mathf.Abs(indexChange) / indexChange;
            int cameras = gridSpawner.stateDrivenCamera.ChildCameras.Length;
            int currentCameraIndex = animator.GetInteger("CameraIndex");
            currentCameraIndex = Mathf.Clamp(currentCameraIndex + (int)indexChange, 0 ,cameras - 1);
            fractionSpeed = moveSpeed * ((cameras - currentCameraIndex) / (float) cameras);
            animator.SetInteger("CameraIndex", currentCameraIndex);
        }

        private void ClearGrid(InputAction.CallbackContext context)
        {
            PauseGame();
            gridManager.ClearGrid();
        }
        private void PlayNextGeneration(InputAction.CallbackContext context)
        {
            PauseGame();
            gridManager.CalculateNextGeneration();
        }

        private void PlayPreviousGeneration(InputAction.CallbackContext context)
        {
            PauseGame();
            gridManager.ReturnPreviousGeneration();
        }
        private void RandomGeneration(InputAction.CallbackContext context)
        {
            PauseGame();
            gridManager.RandomGeneration();
        }
        private void ForwardAutoPlay(InputAction.CallbackContext context)
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

        public void PauseGame()
        {
            if (!isPlaying) return;
            gridManager.StopPlaying();
            isPlaying = false;
        }
        public void PlayForward()
        {
            PauseGame();
            gridManager.PlayForward();
            isPlaying = true;
        }
        private void BackwardAutoPlay(InputAction.CallbackContext context)
        {
            PauseGame();
            gridManager.PlayBackward();
            isPlaying = true;
        }
        private void SetStartPosition()
        {
            transform.position = gridSpawner.GetGridCenter();
        }
        public void UpdateSpeed()
        {
            gridManager.SetDelayNextGeneration(playSpeed - slider.value);
        }
        private void SetPatternAsDefault(InputAction.CallbackContext context)
        {
            gridManager.SetPatternAsDefault();
        }
        private void OnDisable()
        {
            playerInput.Zoom.performed -= Zoom;
            playerInput.PlayNextGeneration.performed -= PlayNextGeneration;
            playerInput.PlayPrevGeneration.performed -= PlayPreviousGeneration;
            playerInput.AutoPlay.performed += ForwardAutoPlay;
            playerInput.BackwardAutoPlay.performed -= BackwardAutoPlay;
            playerInput.RandomGeneration.performed -= RandomGeneration;
            playerInput.Clear.performed -= ClearGrid;
            playerInput.SetPatternAsDefault.performed += SetPatternAsDefault;
            playerInput.Disable();
        } 
    }
}
