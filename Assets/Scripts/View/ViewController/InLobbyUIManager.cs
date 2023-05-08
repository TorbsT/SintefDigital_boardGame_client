using Network;
using Newtonsoft.Json;
using System;
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
        [SerializeField] private string startGameScene = "GameScene";
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button situationViewButton;
        [SerializeField] private TextMeshProUGUI situationViewText;
        [SerializeField] private Button changeRoleButton;
        [SerializeField] private TextMeshProUGUI changeRoleText;
        [SerializeField] private RectTransform situationView;
        [SerializeField] private RectTransform playerView;
        [SerializeField] private TextMeshProUGUI lobbyHelpText;

        private int retry_counter = 0;
        private const int times_to_retry = 5;

        private NetworkData.GameState? gameState;

        private void Start()
        {
            
        }
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += CompleteRefresh;
            GameCardController.Instance.SelectedCards += Refresh;
            CompleteRefresh(GameStateSynchronizer.Instance.GameState);
            SwitchView(false);
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= CompleteRefresh;
            GameCardController.Instance.SelectedCards -= Refresh;
        }
        public void SituationButtonClicked()
        {
            SwitchView(!situationView.gameObject.activeSelf);
        }
        public void StartGameClicked()
        {
            GameCardController.Instance.Confirm(
                success =>
                {
                    RestAPI.Instance.StartGame(
                    gameState =>
                    {
                        
                    },
                    failure =>
                    {
                        Debug.LogWarning("Couldn't start game");
                    });
                },
                failure => { Debug.LogWarning("Could not confirm situation cards");
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
                }, GameStateSynchronizer.Instance.GameState.Value
                , false);
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
        private void SwitchView(bool showSituations)
        {
            situationView.gameObject.SetActive(showSituations);
            playerView.gameObject.SetActive(!showSituations);
            if (showSituations)
            {
                situationViewText.text = "See players";
                GameCardController.Instance.Refresh();
            }
            else
                situationViewText.text = "Choose situation";
        }
        private void CompleteRefresh(NetworkData.GameState? gameState)
        {
            this.gameState = gameState;
            Refresh();
        }
        private void Refresh()
        {
            if (this.gameState == null) return;
            NetworkData.GameState gameState = this.gameState.Value;
            GetComponent<UIListHandler>().Clear();
            NetworkData.Player? orchestrator = GameStateSynchronizer.Instance.Orchestrator;
            NetworkData.Player me = GameStateSynchronizer.Instance.Me;
            bool meIsOrchestrator = GameStateSynchronizer.Instance.IsOrchestrator;
            bool orchestratorExists = orchestrator != null;

            List<NetworkData.Player> sortedPlayerList = new();
            foreach (var player in gameState.players)
                sortedPlayerList.Add(player);

            string debug = "Before: ";
            foreach (NetworkData.Player player in sortedPlayerList)
                debug += player.in_game_id + " ";
            sortedPlayerList.Sort(NetworkData.PlayOrder);

            debug = "After: ";
            foreach (NetworkData.Player player in sortedPlayerList)
                debug += player.in_game_id + " ";

            foreach (var player in sortedPlayerList)
            {
                string roleName = player.in_game_id;
                bool isMe = player.unique_id == me.unique_id;
                AddPlayer(player.name, roleName, isMe);
            }

            if (meIsOrchestrator) changeRoleText.text = "Switch to player";
            else changeRoleText.text = "Switch to orchestrator";

            bool enableRoleSwitch = meIsOrchestrator || !orchestratorExists;
            bool situationChosen = GameCardController.Instance.ChosenCount > 0;
            int minPlayers = 2;
            bool sufficientPlayers = gameState.players.Count >= minPlayers;
            bool enableStartGame = situationChosen && sufficientPlayers && meIsOrchestrator;
            changeRoleButton.interactable = enableRoleSwitch;
            startGameButton.interactable = enableStartGame;
            situationViewButton.interactable = meIsOrchestrator;
            if (!meIsOrchestrator)
                SwitchView(false);

            string orchestratorName = "";
            if (orchestratorExists)
                orchestratorName = orchestrator.Value.name;
            // lobby help text
            if (meIsOrchestrator)
            {
                if (!situationChosen)
                    lobbyHelpText.text = "Choose a situation card";
                else if (!sufficientPlayers)
                    lobbyHelpText.text = $"Need at least {minPlayers} players";
                else
                    lobbyHelpText.text = "Ready to start";
            }
            else
            {
                if (!sufficientPlayers)
                    lobbyHelpText.text = $"Need at least {minPlayers} players";
                else if (!orchestratorExists)
                    lobbyHelpText.text = $"There must be one orchestrator";
                else
                    lobbyHelpText.text = $"Waiting for {orchestratorName} to start the game";
            }

            if (me.in_game_id == NetworkData.InGameID.Undecided.ToString() && retry_counter >= times_to_retry)
            {
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
                }, GameStateSynchronizer.Instance.GameState.Value
                , true);
                retry_counter = 0;
            }
            retry_counter++;

            if (!gameState.is_lobby)
            {  // Game has started
                SceneManager.LoadSceneAsync(startGameScene);
            }
        }
        private void AddPlayer(string playerName, string roleName, bool isMe)
        {
            NetworkData.InGameID role = (NetworkData.InGameID)Enum.Parse(typeof(NetworkData.InGameID), roleName);
            if (role != NetworkData.InGameID.Orchestrator) roleName = "Player";
            GameObject panel = PoolManager.Instance.Depool(playerPrefab);
            LobbyPlayerUI player = panel.GetComponent<LobbyPlayerUI>();
            player.GetComponent<PlayerOwned>().Owner = role;
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