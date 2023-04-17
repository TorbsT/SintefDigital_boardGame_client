using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class RestAPI : MonoBehaviour
    {
        public static RestAPI Instance { get; private set; }

        [SerializeField] private string URL = "localhost";
        [SerializeField] private int port = 5000;
        [SerializeField] private string lastBody;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // Implemented
        internal void RefreshLobbies(Action<NetworkData.LobbyList> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("games/lobbies", successCallback, failureCallback));
        }
        internal void CreateUniquePlayerId(Action<int> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("create/playerID", successCallback, failureCallback));
        }
        internal void CreateGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
        {
            string jsonObject = JsonUtility.ToJson(
                new NetworkData.NewGameInfo
                {
                    host = NetworkData.Instance.Me,
                    name = $"{NetworkData.Instance.Me.name}'s lobby"
                }
                );
            lastBody = jsonObject;
            StartCoroutine(POST("create/game", jsonObject, successCallback, failureCallback));
        }
        internal void GetGameState(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            StartCoroutine(GET($"games/game/{lobbyId}", successCallback, failureCallback));
        }
        internal void LeaveLobby(Action<string> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(DELETE($"games/leave/{NetworkData.Instance.UniqueID}", successCallback, failureCallback));
        }
        internal void JoinLobby
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            string jsonObject = JsonUtility.ToJson(NetworkData.Instance.Me);
            StartCoroutine(POST($"games/join/{lobbyId}", jsonObject, successCallback, failureCallback));
        }
        internal void SendPlayerInput
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.PlayerInput input)
        {
            string jsonObject = JsonUtility.ToJson(
                    input
                );
            lastBody = jsonObject;
            StartCoroutine(POST("games/input", jsonObject, successCallback, failureCallback));
        }
        
        // Helpers - used often
        internal void ChangeToFirstAvailableRole(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.GameState state)
        {
            NetworkData.InGameID chosenRole = NetworkData.Instance.GetFirstAvailableRole(state, false);
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.Me.unique_id,
                game_id = state.id,
                input_type = NetworkData.PlayerInputType.ChangeRole.ToString(),
                related_role = chosenRole.ToString(),
                related_node_id = null,
                district_modifier = null
            };
            Debug.Log(input.player_id);
            Debug.Log(input.game_id);
            SendPlayerInput(successCallback, failureCallback, input);
        }

        // Debug
        internal void DebugPlayerCount(Action<int> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("debug/playerIDs/amount", successCallback, failureCallback));
        }


        private IEnumerator DELETE<T>(string resource, Action<T> successCallback, Action<string> failureCallback)
        {
            using UnityWebRequest request = UnityWebRequest.Delete(GetConnectURL(resource));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            HandleResponse(request, successCallback, failureCallback);
        }
        private IEnumerator GET<T>(string resource, Action<T> successCallback, Action<string> failureCallback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(GetConnectURL(resource));
            yield return request.SendWebRequest();
            HandleResponse(request, successCallback, failureCallback);
        }
        private IEnumerator POST<T>(string resource, string data, Action<T> successCallback, Action<string> failureCallback)
        {
            using UnityWebRequest request =
                UnityWebRequest.Post(GetConnectURL(resource),
                data, "application/json");
            yield return request.SendWebRequest();
            HandleResponse(request, successCallback, failureCallback);
        }
        private void HandleResponse<T>(UnityWebRequest request, Action<T> successCallback, Action<string> failureCallback)
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                T responseObject;
                if (int.TryParse(json, out int numberResponse) && typeof(T) == typeof(int))
                {
                    responseObject = (T)(object)numberResponse;
                }
                else
                {
                    try
                    {
                        responseObject = JsonUtility.FromJson<T>(json);
                    }
                    catch
                    {
                        Debug.LogWarning(
                            $"Expected return type {typeof(T)}, received json text: {json}." +
                            $" You could try changing the expected return type.");
                        responseObject = default;
                    }
                }

                successCallback?.Invoke(responseObject);
            }
            else
            {
                Debug.LogWarning(request.error);
                failureCallback?.Invoke(request.error);
            }
        }
        private string GetConnectURL(string resource)
            => $"http://{URL}:{port}/{resource}";
    }
}