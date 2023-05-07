using Network;
using Newtonsoft.Json;
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

        public event Action<NetworkData.GameState?> StateChanged;
        public event Action<NetworkData.Player> PlayerConnected;
        public event Action< List<NetworkData.DistrictModifier> > districtModifierChanged;
        public event Action<NetworkData.SituationCard> situationCardChanged;
        public event Action<int> PlayerDisconnected;
        [field: SerializeField] public int? LobbyId { get; private set; } = null;
        public NetworkData.GameState? GameState { get; private set; }
        public NetworkData.Player Me => GameState.Value.players.Find
            (match => match.unique_id == NetworkData.Instance.UniqueID);
        public NetworkData.Player? Orchestrator
        {
            get
            {
                GameState.Value.players.Find
               (match => match.in_game_id == NetworkData.InGameID.Orchestrator.ToString());
                if (GameState == null) return null;
                foreach (var player in GameState.Value.players)
                    if (player.in_game_id == NetworkData.InGameID.Orchestrator.ToString())
                        return player;
                return null;
            }
        }
        public bool IsOrchestrator => Orchestrator != null && Me.unique_id == Orchestrator.Value.unique_id;
        [SerializeField, Range(0f, 10f)] private float fetchSuccessCooldown = 1f; 
        [SerializeField, Range(0f, 30f)] private float fetchFailCooldown = 5f;
        [SerializeField] private float currentCooldown = 0f;
        [SerializeField] private State state;

        [SerializeField] private NetworkData.GameState cock;

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
                SetGamestate(null);
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
        private void SetGamestate(NetworkData.GameState? newState)
        {
            // Check for differences between old and new state
            
            bool differenceExists = false;
            Dictionary<int, NetworkData.Player> oldPlayerIds = new();
            Dictionary<int, NetworkData.Player> newPlayerIds = new();
            if (GameState != null)
                foreach (NetworkData.Player player in GameState.Value.players) oldPlayerIds.Add(player.unique_id, player);
            if (newState != null)
                foreach (NetworkData.Player player in newState.Value.players) newPlayerIds.Add(player.unique_id, player);
            
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
            
            bool districtHasChanged = false;
            if (GameState != null && newState != null)
            {
                districtHasChanged = (GameState.Value.district_modifiers.Count != newState.Value.district_modifiers.Count);
            }
 
            GameState = newState;
            
            if (GameState == null) {
                return;
            }

            if (districtHasChanged)
            {
                districtModifierChanged?.Invoke(GameState.Value.district_modifiers);
     
            }

            if (true) //Could do a check thats limits the number of calls, but that solution is kinda buggy
            {
                situationCardChanged?.Invoke(GameState.Value.situation_card.Value);
            }
            // the following is very good code
            if (true || differenceExists)
            {
                StateChanged?.Invoke(newState);
            }
        }

        internal void ClearAllStateChangeSubscribers()
        {
            this.StateChanged = null;
        }
    }
}
