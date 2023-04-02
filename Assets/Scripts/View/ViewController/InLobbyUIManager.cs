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
        public void Show()
        {
            GetComponent<UIListHandler>().Clear();
            Refresh();
        }
        public void Refresh()
        {
            startGameButton.interactable = false;
            refreshButton.interactable = false;
            RestAPI.Instance.GetGameState((success) =>
            {
                GetComponent<UIListHandler>().Clear();

                foreach (var player in success.players)
                {
                    bool isHost = player.in_game_id == NetworkData.InGameID.Orchestrator.ToString();
                    AddPlayer(player.unique_id.ToString(), player.in_game_id, isHost);
                }
                startGameButton.interactable = true;
                refreshButton.interactable = true;
            }, (failure) =>
            {
                Debug.Log(failure);
                MainMenuUIController.Instance.BackToMainMenu();
            });

        }
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(gameScene);
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