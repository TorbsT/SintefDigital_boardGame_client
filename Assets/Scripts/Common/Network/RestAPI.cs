using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


/*
 note: DO NOT USE JsonUtility!!!
 Use NewtonSoft.Json.JsonConvert.etc
 JsonUtility can't deserialize nullable integers
 */
namespace Common.Network
{
    /// <summary>
    /// Has the responsibility of sending requests and awaiting responses from backend.
    /// Also see <see cref="GameStateSynchronizer"/> and <see cref="NetworkData"/>
    /// </summary>
    public class RestAPI : MonoBehaviour
    {
        public static RestAPI Instance { get; private set; }
        private string URL => ip.URL;
        private int Port => ip.Port;

        [SerializeField] private IPObject ip;  // Quickly choose between localhost/live backend
        [SerializeField] private string lastBody;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        /// <summary>
        /// Fetch all situation cards.
        /// </summary>
        public void GetSituationCards(Action<NetworkData.SituationCardList> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("resources/situationcards", successCallback, failureCallback));
        }
        /// <summary>
        /// Check-in to let the server know the user is still there.
        /// </summary>
        public void CheckIn(Action<string> successCallback, Action<string> failureCallback, int id)
        {
            StartCoroutine(GET($"check-in/{id}", successCallback, failureCallback));
        }
        /// <summary>
        /// Fetch all non-game lobbies.
        /// </summary>
        public void RefreshLobbies(Action<NetworkData.LobbyList> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("games/lobbies", successCallback, failureCallback));
        }
        /// <summary>
        /// Effectively log in by fetching a unique player id to use
        /// for the rest of the session.
        /// </summary>
        public void CreateUniquePlayerId(Action<int> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("create/playerID", successCallback, failureCallback));
        }
        /// <summary>
        /// Create a lobby (not starting the game!)
        /// </summary>
        public void CreateGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
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
        /// <summary>
        /// Fetch the game state of the given lobby,
        /// should mainly be used by <see cref="GameStateSynchronizer"/>
        /// </summary>
        public void GetGameState(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            StartCoroutine(GET($"games/game/{lobbyId}", successCallback, failureCallback));
        }
        /// <summary>
        /// Join a lobby with given id.
        /// </summary>
        public void JoinLobby
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int lobbyId)
        {
            string jsonObject = JsonConvert.SerializeObject(NetworkData.Instance.Me.Value, Formatting.Indented);
            StartCoroutine(POST($"games/join/{lobbyId}", jsonObject, successCallback, failureCallback));
        }
        /// <summary>
        /// Base request for sending common game-related requests.
        /// </summary>
        public void SendPlayerInput
            (Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.PlayerInput input)
        {
            string jsonObject = JsonConvert.SerializeObject(input, Formatting.Indented);
            // DO NOT USE JsonUtility.ToJson! it doesn't include situation_card_id for some unholy reason
            lastBody = jsonObject;
            StartCoroutine(POST("games/input", jsonObject, successCallback, failureCallback));
        }

        // Helpers - used often, or abstracts
        /// <summary>
        /// Check for the first available role locally,
        /// and send a request to become that role.
        /// </summary>
        public void ChangeToFirstAvailableRole(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, NetworkData.GameState state, bool ignoreOrchestrator)
        {
            NetworkData.InGameID chosenRole = NetworkData.Instance.GetFirstAvailableRole(state, ignoreOrchestrator);
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
        /// <summary>
        /// Called in lobby to start the game.
        /// Must be called after <see cref="GetSituationCards">
        /// </summary>
        public void StartGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
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
        /// <summary>
        /// As orchestrator, send a request to set edge restriction
        /// </summary>
        public void SetEdgeRestriction(Action<NetworkData.GameState> successCallback, Action<string> failureCallback, int node_one_id, int node_two_id, NetworkData.RestrictionType chosen_edge_restriction, bool shouldDelete)
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
        /// <summary>
        /// Disconnect, whether it be from a lobby or from inside a game.
        /// Remember to send the player to MainMenu.
        /// </summary>
        public void DisconnectFromGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback) {
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.LeaveGame.ToString(),
            };

            SendPlayerInput(success=>{GameStateSynchronizer.Instance.SetLobbyId(null); successCallback(success);}, failureCallback, input);
        }

        // Debug
        public void DebugPlayerCount(Action<int> successCallback, Action<string> failureCallback)
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
    }
}