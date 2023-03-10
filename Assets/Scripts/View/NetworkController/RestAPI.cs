using GluonGui.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class RestAPI : MonoBehaviour
    {
        public class Result
        {
            public bool Success { get; internal set; }
            public string ErrorMessage { get; internal set; }
            public string JSONResponse { get; internal set; }
        }
        private struct PlayerInfoAndLobbyName
        {
            private struct PlayerInfo
            {
                public int UniqueID;
                public string Name;
            }

            private PlayerInfo playerInfo;
            private string LobbyName;

            public PlayerInfoAndLobbyName(int uniqueId, string name, string lobbyName)
            {
                playerInfo = new()
                {
                    UniqueID = uniqueId,
                    Name = name
                };
                LobbyName = lobbyName;
            }
        }
        public static RestAPI Instance { get; private set; }

        [SerializeField] private string URL = "localhost";
        [SerializeField] private int port = 5000;
        [SerializeField] private GameState gameState;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        public void RefreshLobbies(Action<string> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("InternetMultiplayer/getGameState", successCallback, failureCallback));
        }
        public void CreateGame(Action<GameState> successCallback, Action<string> failureCallback, int uniquePlayerId, string playerName, string lobbyName)
        {
            WWWForm form = new();
            string jsonObject = JsonUtility.ToJson(
                new PlayerInfoAndLobbyName(uniquePlayerId, playerName, lobbyName)
                );
            form.AddField("playerInfoAndLobbyName", jsonObject);
            StartCoroutine(POST("create/game", form, successCallback, failureCallback));
        }
        private IEnumerator GET<T>(string resource, Action<T> successCallback, Action<string> failureCallback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(GetConnectURL(resource));
            yield return request.SendWebRequest();
            HandleResponse(request, successCallback, failureCallback);
        }
        private IEnumerator POST<T>(string resource, WWWForm form, Action<T> successCallback, Action<string> failureCallback)
        {
            using UnityWebRequest request = UnityWebRequest.Post(GetConnectURL(resource), form);
            yield return request.SendWebRequest();
            HandleResponse(request, successCallback, failureCallback);
        }
        private void HandleResponse<T>(UnityWebRequest request, Action<T> successCallback, Action<string> failureCallback)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogWarning(request.error);
                failureCallback?.Invoke(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                successCallback?.Invoke(
                    JsonUtility.FromJson<T>(json)
                    );
            }
        }
        private string GetConnectURL(string resource)
            => $"{URL}:{port}/API/{resource}";
        [Serializable]
        public class GameState
        {
            public int playerTurn;
            public List<Player> players;
            [Serializable]
            public class Player
            {
                public string displayName;
                public List<int> colour;
            }
        }
    }
}