using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Network;

namespace View
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private string disconnectToScene;
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private string amogusScene;
        private void Awake()
        {
            pauseMenu.SetActive(false);
            SceneManager.LoadSceneAsync(amogusScene, LoadSceneMode.Additive);
        }
        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
                pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        /*
        public void OrchestratorToggle()
        {
            OrchestratorViewHandler.Instance.gameObject.
                SetActive(!OrchestratorViewHandler.Instance.gameObject.activeSelf);
        }
        */
        public void PauseClicked()
        {
            pauseMenu.SetActive(true);
        }
        public void DisconnectClicked()
        {
            RestAPI.Instance.DisconnectFromGame(success=>{SceneManager.LoadSceneAsync(disconnectToScene); Debug.Log("Successfully quit the game!");}, failure=>{Debug.Log("Could not disconnect from the game");});
        }
        public void ContinueClicked()
        {
            pauseMenu.SetActive(false);
        }
    }
}

