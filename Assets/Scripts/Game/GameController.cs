using Common;
using Common.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private string disconnectToScene = "MainMenu";
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += StateChanged;
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= StateChanged;
        }

        private void StateChanged(NetworkData.GameState? state)
        {
            if (state == null)
                return;
            if (state.Value.is_lobby)
                SceneManager.LoadSceneAsync(disconnectToScene);
        }
    }
}