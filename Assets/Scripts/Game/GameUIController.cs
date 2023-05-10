using UnityEngine;
using UnityEngine.SceneManagement;
using Common.Network;

namespace Game
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private string disconnectToScene;
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private GameObject pauseMenu;

        private void Awake()
        {
            pauseMenu.SetActive(false);
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
            RestAPI.Instance.DisconnectFromGame(success=>{SceneManager.LoadSceneAsync(disconnectToScene); }, failure=>{Debug.LogWarning("Could not disconnect from the game");});
        }
        public void ContinueClicked()
        {
            pauseMenu.SetActive(false);
        }
    }
}

