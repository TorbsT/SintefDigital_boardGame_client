using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View
{
    public class InLobbyUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private string gameScene;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button refreshButton;

        private void Start()
        {
            
        }
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += CompleteRefresh;
            CompleteRefresh();
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= CompleteRefresh;
        }
        public void StartGameClicked()
        {
            SceneManager.LoadSceneAsync(gameScene);
        }
        public void RefreshClicked()
        {
            CompleteRefresh();
        }
        public void LeaveLobbyClicked()
        {
            RestAPI.Instance.LeaveLobby(
                (success) =>
                {
                    Debug.Log("YEP");
                    GameStateSynchronizer.Instance.SetLobbyId(null);
                    MainMenuUIController.Instance.BackToMainMenu();
                },
                (failure) =>
                {
                    Debug.LogWarning($"Couldn't leave lobby: {failure}");
                }
                );
        }
        private void CompleteRefresh()
        {
            Debug.Log("Yabbai " + GameStateSynchronizer.Instance.GameState.players.Count);
            GetComponent<UIListHandler>().Clear();
            foreach (var player in GameStateSynchronizer.Instance.GameState.players)
            {
                bool isHost = player.in_game_id == NetworkData.InGameID.Orchestrator.ToString();
                AddPlayer(player.name, player.in_game_id, isHost);
            }
        }
        private void AddPlayer(string playerName, string roleName, bool host)
        {
            GameObject panel = PoolManager.Instance.Depool(playerPrefab);
            LobbyPlayerUI player = panel.GetComponent<LobbyPlayerUI>();
            player.Name = playerName;
            player.Role = roleName;
            if (host) player.Host = "Host";
            else player.Host = "";
            GetComponent<UIListHandler>().AddItem(panel);
        }
    }
}