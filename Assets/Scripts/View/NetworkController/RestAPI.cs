using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;


/*
 DO NOT USE JsonUtility!!!
 Use NewtonSoft.Json.JsonConvert.whatever
 JsonUtility has big issues
 (with nullable integers and related)
 */
namespace Network
{
    public class RestAPI : MonoBehaviour
    {
        public static RestAPI Instance { get; private set; }
        private string URL => ip.URL;
        private int Port => ip.Port;

        [SerializeField] private IPObject ip;
        [SerializeField] private string lastBody;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // Implemented
        internal void GetSituationCards(Action<NetworkData.SituationCardList> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("resources/situationcards", successCallback, failureCallback));
        }
        internal void CheckIn(Action<string> successCallback, Action<string> failureCallback, int id)
        {
            StartCoroutine(GET($"check-in/{id}", successCallback, failureCallback));
        }
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
            string jsonObject = JsonConvert.SerializeObject(
                new NetworkData.NewGameInfo
                {
                    host = NetworkData.Instance.Me.Value,
                    name = $"{NetworkData.Instance.Me.Value.name}'s lobby"
                }, Formatting.Indented
                );
            lastBody = jsonObject;
            StartCoroutine(POST("create/game", jsonObject, successCallback, failureCallback));
        }

        internal void GetGameState(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            StartCoroutine(GET($"games/game/{lobbyId}", successCallback, failureCallback));
        }
        internal void LeaveLobby(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
        {
            DisconnectFromGame(successCallback, failureCallback);//StartCoroutine(DELETE($"games/leave/{NetworkData.Instance.UniqueID}", successCallback, failureCallback));
        }
        internal void JoinLobby
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            string jsonObject = JsonConvert.SerializeObject(NetworkData.Instance.Me.Value, Formatting.Indented);
            StartCoroutine(POST($"games/join/{lobbyId}", jsonObject, successCallback, failureCallback));
        }
        internal void SendPlayerInput
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.PlayerInput input)
        {
            string jsonObject = JsonConvert.SerializeObject(input, Formatting.Indented);
            // DO NOT USE JsonUtility.ToJson! it doesn't include situation_card_id for some unholy reason
            lastBody = jsonObject;
            StartCoroutine(POST("games/input", jsonObject, successCallback, failureCallback));
        }
        
        // Helpers - used often, or abstracts
        internal void ChangeToFirstAvailableRole(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.GameState state)
        {
            NetworkData.InGameID chosenRole = NetworkData.Instance.GetFirstAvailableRole(state, false);
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = state.id,
                input_type = NetworkData.PlayerInputType.ChangeRole.ToString(),
                related_role = chosenRole.ToString(),
                related_node_id = null,
                district_modifier = null
            };
            SendPlayerInput(successCallback, failureCallback, input);
        }
        internal void StartGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
        {
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.StartGame.ToString(),
                related_role = NetworkData.InGameID.Orchestrator.ToString(),  // Should always be this
            };
            SendPlayerInput(successCallback, failureCallback, input);
        }

        internal void DisconnectFromGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback) {
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.LeaveGame.ToString(),
            };

            SendPlayerInput(success=>{GameStateSynchronizer.Instance.SetLobbyId(null); successCallback(success);}, failureCallback, input);
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
                        responseObject = JsonConvert.DeserializeObject<T>(json);
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
                Debug.LogWarning($"{request.responseCode} {request.error}: {request.downloadHandler.text}");
                failureCallback?.Invoke(request.error);
            }
        }
        private string GetConnectURL(string resource)
            => $"http://{URL}:{Port}/{resource}";

        internal void SetEdgeRestriction(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int node_one_id, int node_two_id, NetworkData.RestrictionType chosen_edge_restriction, bool shouldDelete)
        {
            NetworkData.EdgeRestriction edgeRestriction = new()
            {
                node_one = node_one_id,
                node_two = node_two_id,
                edge_restriction = chosen_edge_restriction.ToString(),
                delete = shouldDelete,
            };
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.ModifyEdgeRestrictions.ToString(),
                edge_modifier = edgeRestriction
            };

            SendPlayerInput(successCallback, failureCallback, input);
        }
    }
}