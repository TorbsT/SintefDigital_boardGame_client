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
            RestAPI.Instance.RefreshLobbies((result) => DoneRefresh(result));
            // Remove all previously found lobbies
            GetComponent<UIListHandler>().Clear();
                
            createLobbyButton.interactable = false;
            refreshButton.interactable = false;
            noLobbiesText.gameObject.SetActive(true);
            noLobbiesText.text = "Searching for lobbies...";
        }
        public void CreateLobby()
        {
            MainMenuUIController.Instance.JoinLobby(-1);
        }
        public void DoneRefresh(RestAPI.Result result)
        {
            createLobbyButton.interactable = true;
            refreshButton.interactable = true;
            if (!result.Success)
            {
                noLobbiesText.text = $"Found no lobbies: {result.ErrorMessage}";
                noLobbiesText.gameObject.SetActive(true);
                return;
            }
            for (int i = 0; i < UnityEngine.Random.Range(1, 6); i++)
            {
                GameObject gameObject = PoolManager.Instance.Depool(lobbyPrefab);
                LobbyButtonUI lobby = gameObject.GetComponent<LobbyButtonUI>();
                lobby.Name = $"Lobby with cool name {i}";
                lobby.LobbyId = i;
                lobby.Quantity = "1/5";
                GetComponent<UIListHandler>().AddItem(gameObject);
            }
            noLobbiesText.gameObject.SetActive(false);
            // do more
        }
    }
}