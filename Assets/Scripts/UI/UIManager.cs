using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOfLife.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject controlCanvas;
        [SerializeField] GameObject[] startObjects;
        [SerializeField] GameObject loading;
        AsyncOperation gameScene;
        bool showControls;
        private void Start()
        {
            showControls = false;
            int activeScene = SceneManager.GetActiveScene().buildIndex;
            if (activeScene != 1)
            {
                gameScene = SceneManager.LoadSceneAsync(1);
                gameScene.allowSceneActivation = false;
            }
        }

        public void LoadGameScene()
        {
            foreach (GameObject startObject in startObjects)
            {
                startObject.SetActive(false);
            }
            loading.SetActive(true);
            gameScene.allowSceneActivation = true;
        }

        public void DisplayControlsCanvas()
        {
            showControls = !showControls;
            controlCanvas.SetActive(showControls);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
