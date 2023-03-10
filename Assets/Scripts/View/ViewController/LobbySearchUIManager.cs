using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Network;

namespace View
{
    public class LobbySearchUIManager : MonoBehaviour
    {
        public static LobbySearchUIManager Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI noLobbiesText;
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button refreshButton;
        [SerializeField] private GameObject lobbyPrefab;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Refresh();
        }
        public void Refresh()
        {
            if (RestAPI.Instance == null)
            {
                Debug.LogWarning(
                    "Tried refreshing lobbies, but there is no RestAPI instance. " +
                    "Do not open Main Menu Scene directly, always go via IntroScene");
                return;
            }
            RestAPI.Instance.RefreshLobbies(
                (result) => RefreshSuccess(result),
                (result) => RefreshFailure(result)
                );
            // Remove all previously found lobbies
            GetComponent<UIListHandler>().Clear();
                
            createLobbyButton.interactable = false;
            refreshButton.interactable = false;
            noLobbiesText.gameObject.SetActive(true);
            noLobbiesText.text = "Searching for lobbies...";
        }
        public void CreateLobby()
        {
            RestAPI.Instance.CreateGame(
                (success) => CreateLobbySuccess(success), 
                (failure) => CreateLobbyFailure(failure), 
                0, "per arne", "perkele"
                );
            
        }
        private void CreateLobbySuccess(RestAPI.GameState result)
        {
            Debug.Log(result.playerTurn);
            MainMenuUIController.Instance.JoinLobby(-1);
        }
        private void CreateLobbyFailure(string errorMsg)
        {
            noLobbiesText.text = $"Could not create lobby: {errorMsg}";
        }
        private void RefreshSuccess(string result)
        {
            for (int i = 0; i < UnityEngine.Random.Range(1, 6); i++)
            {
                GameObject gameObject = PoolManager.Instance.Depool(lobbyPrefab);
                LobbyButtonUI lobby = gameObject.GetComponent<LobbyButtonUI>();

                lobby.Name = $"Lobby with cool name {i}";
                lobby.LobbyId = i;
                lobby.Quantity = "1/5";
                GetComponent<UIListHandler>().AddItem(gameObject);
            }
            createLobbyButton.interactable = true;
            refreshButton.interactable = true;
            noLobbiesText.gameObject.SetActive(false);
        }
        private void RefreshFailure(string result)
        {
            createLobbyButton.interactable = true;
            refreshButton.interactable = true;
            noLobbiesText.text = $"Found no lobbies: {result}";
            noLobbiesText.gameObject.SetActive(true);
        }
    }
}