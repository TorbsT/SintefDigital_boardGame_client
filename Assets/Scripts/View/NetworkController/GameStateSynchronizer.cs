using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GameStateSynchronizer : MonoBehaviour
    {
        public static GameStateSynchronizer Instance { get; private set; }

        public event Action<NetworkData.Player> PlayerConnected;
        public event Action<int> PlayerDisconnected;

        [field: SerializeField] public int? LobbyId { get; private set; }
        [field: SerializeField] public NetworkData.GameState GameState { get; private set; }
        [SerializeField, Range(0f, 10f)] private float fetchSuccessCooldown = 1f; 
        [SerializeField, Range(0f, 30f)] private float fetchFailCooldown = 5f;
        [SerializeField] private float currentCooldown = 0f;
        private bool lastFailed = false;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            FetchServer();

        }
        private void Update()
        {
            currentCooldown += Time.deltaTime;
            if ((lastFailed && currentCooldown >= fetchFailCooldown)
                || (!lastFailed && currentCooldown >= fetchSuccessCooldown))
        }
        public void SetLobbyId(int id)
        {
            LobbyId = id;
        }
        private void FetchServer()
        {
            RestAPI.Instance.GetGameState(
                (success) =>
                {
                    SetGamestate(success);
                    Invoke(nameof(FetchServer), fetchSuccessCooldown);
                },
                (failure) =>
                {
                    Debug.Log($"Couldn't fetch server: {failure}");
                    Invoke(nameof(FetchServer), fetchFailCooldown);
                }
            );
        }
        private void SetGamestate(NetworkData.GameState newState)
        {
            // Check for differences between old and new state
            Dictionary<int, NetworkData.Player> oldPlayerIds = new();
            Dictionary<int, NetworkData.Player> newPlayerIds = new();
            foreach (NetworkData.Player player in GameState.players) oldPlayerIds.Add(player.unique_id, player);
            foreach (NetworkData.Player player in newState.players) newPlayerIds.Add(player.unique_id, player);
            
            HashSet<int> allPlayerIds = new();
            foreach (int id in oldPlayerIds.Keys) allPlayerIds.Add(id);
            foreach (int id in newPlayerIds.Keys) allPlayerIds.Add(id);
            
            foreach (int id in allPlayerIds)
            {
                // Check if this was added or removed
                if (oldPlayerIds.ContainsKey(id) && !newPlayerIds.ContainsKey(id))
                {
                    // Just disconnected
                    PlayerDisconnected?.Invoke(id);
                }
                else if (!oldPlayerIds.ContainsKey(id) && newPlayerIds.ContainsKey(id))
                {
                    // Just connected
                    PlayerConnected?.Invoke(newPlayerIds[id]);
                }
            }

            // Throw away the old state
            GameState = newState;
        }
    }
}
