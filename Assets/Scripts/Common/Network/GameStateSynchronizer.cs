using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Network
{
    /// <summary>
    /// Provides access to network data about the lobby/game the player is in.
    /// Regularly pings backend and fires the new gamestate as an event to its listeners.
    /// </summary>
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

        public event Action<NetworkData.GameState?> StateChanged;  // Most objects subscribe to this
        public event Action< List<NetworkData.DistrictModifier> > districtModifierChanged;
        public event Action<NetworkData.SituationCard> situationCardChanged;
        [field: SerializeField] public int? LobbyId { get; private set; } = null;  // null means out of lobby
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
        
        /// <summary>
        /// Call when joining a lobby or disconnecting from a lobby/game.
        /// Will start to ping this id if it's not null,
        /// in which case it will stop.
        /// </summary>
        /// <param name="id">The lobby id, or null</param>
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
                    Debug.LogWarning($"Couldn't fetch server: {failure}");
                    currentCooldown = 0f;
                    state = State.FAILED;
                }, (int)LobbyId
            );
        }
        private void SetGamestate(NetworkData.GameState? newState)
        {
            bool districtHasChanged = false;
            if (GameState != null && newState != null)
            {
                districtHasChanged = (GameState.Value.district_modifiers.Count != newState.Value.district_modifiers.Count);
            }
 
            GameState = newState;
            
            if (GameState == null)
                return;  // Prevent nullpointerexceptions

            if (districtHasChanged)
            {
                districtModifierChanged?.Invoke(GameState.Value.district_modifiers);
     
            }

            situationCardChanged?.Invoke(GameState.Value.situation_card.Value);
            
            StateChanged?.Invoke(newState);
        }
    }
}
