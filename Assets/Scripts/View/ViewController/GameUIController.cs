using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace View
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private string disconnectToScene;
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private GameObject pauseMenu;
        private void Awake()
        {
            
        }
        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
                pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        public void PauseClicked()
        {
            pauseMenu.SetActive(true);
        }
        public void DisconnectClicked()
        {
            SceneManager.LoadSceneAsync(disconnectToScene);
        }
        public void ContinueClicked()
        {
            pauseMenu.SetActive(false);
        }
    }
}

