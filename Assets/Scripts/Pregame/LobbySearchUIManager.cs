using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Common.Network;
using Common;

namespace Pregame
{
    public class LobbySearchUIManager : MonoBehaviour
    {
        public static LobbySearchUIManager Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI noLobbiesText;
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button refreshButton;
        [SerializeField] private GameObject lobbyPrefab;
        [SerializeField] private TextMeshProUGUI infoText;

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
            string txt = $"Name: {NetworkData.Instance.PlayerName}\n" +
                $"ID: {NetworkData.Instance.UniqueID}";
            infoText.text = txt;


            // Remove all previously found lobbies
            GetComponent<UIListHandler>().Clear();

            createLobbyButton.interactable = false;
            refreshButton.interactable = false;
            noLobbiesText.gameObject.SetActive(true);
            noLobbiesText.text = "Searching for lobbies...";

            RestAPI.Instance.RefreshLobbies(
                (result) =>
                {
                    foreach (var item in result.lobbies)
                    {
                        GameObject gameObject = PoolManager.Instance.Depool(lobbyPrefab);
                        LobbyButtonUI lobby = gameObject.GetComponent<LobbyButtonUI>();
                        lobby.Name = $"{item.name}";
                        lobby.LobbyId = item.id;
                        int quantity = item.players.Count;
                        int maxQuantity = 7;
                        lobby.Quantity = $"{quantity}/{maxQuantity} players";
                        bool joinable = item.is_lobby && quantity < maxQuantity;
                        lobby.GetComponent<Button>().interactable = joinable;
                        GetComponent<UIListHandler>().AddItem(gameObject);
                    }
                    createLobbyButton.interactable = true;
                    refreshButton.interactable = true;
                    noLobbiesText.gameObject.SetActive(false);
                },
                (failure) =>
                {
                    createLobbyButton.interactable = true;
                    refreshButton.interactable = true;
                    noLobbiesText.text = $"Found no lobbies: {failure}";
                    noLobbiesText.gameObject.SetActive(true);
                }
                );
        }
        public void CreateLobby()
        {
            RestAPI.Instance.CreateGame(
                (success) =>
                {
                    MainMenuUIController.Instance.JoinLobby(success.id);
                    RestAPI.Instance.ChangeToFirstAvailableRole((_) => { }, (_) => { }, success, false);
                }, 
                (failure) =>
                {
                    noLobbiesText.text = $"Could not create lobby: {failure}";
                }
                );
        }
    }
}