using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GameStateSynchronizer : MonoBehaviour
    {
        private enum State
        {
            OUTOFLOBBY,
            PINGING,
            FAILED,
            SUCCESS
        }
        public static GameStateSynchronizer Instance { get; private set; }

        public event Action StateChanged;
        public event Action<NetworkData.Player> PlayerConnected;
        public event Action<int> PlayerDisconnected;
        [field: SerializeField] public int? LobbyId { get; private set; } = null;
        [field: SerializeField] public NetworkData.GameState GameState { get; private set; }
        [SerializeField, Range(0f, 10f)] private float fetchSuccessCooldown = 1f; 
        [SerializeField, Range(0f, 30f)] private float fetchFailCooldown = 5f;
        [SerializeField] private float currentCooldown = 0f;
        [SerializeField] private State state;
        

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            state = State.OUTOFLOBBY;
        }
        private void Update()
        {
            if (LobbyId == null || state == State.PINGING || state == State.OUTOFLOBBY)
            {
                currentCooldown = 0f;
                return;
            }

            currentCooldown += Time.deltaTime;
            if ((state == State.FAILED && currentCooldown >= fetchFailCooldown)
                || (state == State.SUCCESS && currentCooldown >= fetchSuccessCooldown)) FetchServer();
        }
        public void SetLobbyId(int? id)
        {
            LobbyId = id;
            if (id == null)
            {
                state = State.OUTOFLOBBY;
                SetGamestate(new());
            } else
            {
                state = State.PINGING;
                FetchServer();
            }
        }
        private void FetchServer()
        {
            state = State.PINGING;
            RestAPI.Instance.GetGameState(
                (success) =>
                {
                    if (state == State.OUTOFLOBBY) return;
                    SetGamestate(success);
                    currentCooldown = 0f;
                    state = State.SUCCESS;
                    Debug.Log("Gamestate has " + success.players.Count);
                },
                (failure) =>
                {
                    if (state == State.OUTOFLOBBY) return;
                    Debug.Log($"Couldn't fetch server: {failure}");
                    currentCooldown = 0f;
                    state = State.FAILED;
                }, (int)LobbyId
            );
        }
        private void SetGamestate(NetworkData.GameState newState)
        {
            // Check for differences between old and new state
            bool differenceExists = false;
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
                    differenceExists = true;
                    PlayerDisconnected?.Invoke(id);
                }
                else if (!oldPlayerIds.ContainsKey(id) && newPlayerIds.ContainsKey(id))
                {
                    // Just connected
                    differenceExists = true;
                    PlayerConnected?.Invoke(newPlayerIds[id]);
                }
            }

            GameState = newState;
            if (differenceExists)
            {
                Debug.Log("State changed: " + allPlayerIds.Count);
                StateChanged?.Invoke();
            }
        }
    }
}