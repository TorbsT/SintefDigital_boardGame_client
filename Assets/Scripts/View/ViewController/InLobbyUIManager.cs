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
            GameStateSynchronizer.Instance.PlayerConnected += PlayerConnected;
            GameStateSynchronizer.Instance.PlayerDisconnected += PlayerDisconnected;
            // sync now
            CompleteRefresh();
        }
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(gameScene);
        }
        private void PlayerConnected(NetworkData.Player player)
        {
            // There won't be hundreds of players so this should be fine
            CompleteRefresh();
        }
        private void PlayerDisconnected(int id)
        {
            CompleteRefresh();
        }
        private void CompleteRefresh()
        {
            GetComponent<UIListHandler>().Clear();
            foreach (var player in GameStateSynchronizer.Instance.GameState.players)
            {
                bool isHost = player.in_game_id == NetworkData.InGameID.Orchestrator.ToString();
                AddPlayer(player.unique_id.ToString(), player.in_game_id, isHost);
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