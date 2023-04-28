using Network;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View
{
    public class InLobbyUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private string startGameScene = "CardScene";
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button refreshButton;
        [SerializeField] private Button changeRoleButton;
        [SerializeField] private TextMeshProUGUI changeRoleText;
        private void Start()
        {
            
        }
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += CompleteRefresh;
            CompleteRefresh(GameStateSynchronizer.Instance.GameState);
            SceneManager.LoadSceneAsync(startGameScene, LoadSceneMode.Additive);
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= CompleteRefresh;
        }
        public void StartGameClicked()
        {
            RestAPI.Instance.StartGame(
                (gameState) =>
                {
                    // Only checking this periodically in
                    // CompleteRefresh
                    // SceneManager.LoadSceneAsync(gameScene);
                    Debug.Log($"Successfully started lobby: " + gameState.is_lobby);
                },
                (failure) =>
                {
                    Debug.LogWarning("Couldn't start game");
                });
            
        }
        public void ChangeRoleClicked()
        {
            changeRoleButton.interactable = false;

            RestAPI.Instance.ChangeToFirstAvailableRole(
                (gameState) =>
                {
                    // Not necessary but refreshes faster, improves user experience
                    CompleteRefresh(gameState);
                },
                (failure) =>
                {
                    Debug.LogWarning("Couldn't change role");
                    changeRoleButton.interactable = true;
                    // uh oh
                }, GameStateSynchronizer.Instance.GameState
                );
        }
        public void LeaveLobbyClicked()
        {
            RestAPI.Instance.LeaveLobby(
                (success) =>
                {
                    GameStateSynchronizer.Instance.SetLobbyId(null);
                    MainMenuUIController.Instance.BackToMainMenu();
                },
                (failure) =>
                {
                    Debug.LogWarning($"Couldn't leave lobby: {failure}");
                }
                );
        }
        private void CompleteRefresh(NetworkData.GameState gameState)
        {
            if (gameState == null) return;
            GetComponent<UIListHandler>().Clear();
            bool orchestratorExists = false;
            bool meIsOrchestrator = false;
            foreach (var player in gameState.players)
            {
                string roleName = player.in_game_id;
                NetworkData.InGameID role = (NetworkData.InGameID)System.Enum.Parse
                    (typeof(NetworkData.InGameID), roleName);

                bool isOrchestrator = role == NetworkData.InGameID.Orchestrator;
                bool isMe = player.unique_id == NetworkData.Instance.Me.unique_id;
                if (isMe && isOrchestrator)
                    meIsOrchestrator = true;

                orchestratorExists = orchestratorExists || isOrchestrator;
                AddPlayer(player.name, roleName, isMe);
            }

            if (meIsOrchestrator) changeRoleText.text = "Switch to player";
            else changeRoleText.text = "Switch to orchestrator";

            bool enableRoleSwitch = meIsOrchestrator || !orchestratorExists;
            changeRoleButton.interactable = enableRoleSwitch;
            startGameButton.interactable = meIsOrchestrator;

            if (!gameState.is_lobby)
            {  // Game has started
                
            }
        }
        private void AddPlayer(string playerName, string roleName, bool isMe)
        {
            GameObject panel = PoolManager.Instance.Depool(playerPrefab);
            LobbyPlayerUI player = panel.GetComponent<LobbyPlayerUI>();
            player.Name = playerName;
            player.Role = roleName;
            
            if (isMe)
            {
                player.Me = "Me";
            }
            else
            {
                player.Me = "";
            }
            GetComponent<UIListHandler>().AddItem(panel);
        }
    }
}