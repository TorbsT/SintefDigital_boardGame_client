using GluonGui.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public static RestAPI Instance { get; private set; }

        [SerializeField] private string URL = "localhost";
        [SerializeField] private int port = 5000;
        [SerializeField] private GameState gameState;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        void Start()
        {
            
        }
        public void RefreshLobbies(Action<Result> callback)
        {
            StartCoroutine(GET("InternetMultiplayer/getGameState", callback));
        }
        private IEnumerator POST(string resource, WWWForm form, Action<Result> callback)
        {
            string connectURL = $"{URL}:{port}/{resource}";
            using (UnityWebRequest request = UnityWebRequest.Post(connectURL, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogWarning(request.error);
                    callback?.Invoke(new()
                    {
                        Success = false,
                        ErrorMessage = request.error
                    });
                }
                else
                {
                    string json = request.downloadHandler.text;
                    gameState = GameState.FromJSON(json);
                    callback?.Invoke(new()
                    {
                        Success = true,
                        JSONResponse = json
                    });
                }
            }
        }
        private IEnumerator GET(string resource, Action<Result> callback)
        {
            string connectURL = $"{URL}:{port}/{resource}";
            using (UnityWebRequest request = UnityWebRequest.Get(connectURL))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogWarning(request.error);
                    callback?.Invoke(new()
                    {
                        Success = false,
                        ErrorMessage = request.error
                    });
                } else
                {
                    string json = request.downloadHandler.text;
                    gameState = GameState.FromJSON(json);
                    callback?.Invoke(new()
                    {
                        Success = true,
                        JSONResponse = json
                    });
                }
            }
        }
    }
    [Serializable]
    internal class GameState
    {
        public int playerTurn;
        public List<Player> players;
        [Serializable]
        public class Player
        {
            public string displayName;
            public List<int> colour;
        }

        public static GameState FromJSON(string json)
        {
            return JsonUtility.FromJson<GameState>(json);
        }
    }
}