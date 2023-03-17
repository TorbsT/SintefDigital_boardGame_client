using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class RestAPI : MonoBehaviour
    {
        public static RestAPI Instance { get; private set; }

        [SerializeField] private string URL = "localhost";
        [SerializeField] private int port = 5000;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        internal void RefreshLobbies(Action<string> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("InternetMultiplayer/getGameState", successCallback, failureCallback));
        }
        internal void CreateGame(Action<NetworkData.GameState> successCallback, Action<string> failureCallback)
        {
            string jsonObject = JsonUtility.ToJson(
                new NetworkData.PlayerInfoAndLobbyName(
                    NetworkData.Instance.UniqueID,
                    NetworkData.Instance.Name,
                    NetworkData.Instance.Name)
                );
            StartCoroutine(POST("create/game", jsonObject, successCallback, failureCallback));
        }
        internal void CreateUniquePlayerId(Action<int> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("create/playerID", successCallback, failureCallback));
        }
        internal void DebugPlayerCount(Action<int> successCallback, Action<string> failureCallback)
        {
            StartCoroutine(GET("debug/playerIDs/amount", successCallback, failureCallback));
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
                        Debug.LogError(
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
            => $"{URL}:{port}/API/{resource}";
    }
}