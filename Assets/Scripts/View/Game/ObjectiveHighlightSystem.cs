using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.Game
{
    using ExtensionMethods;
    public class ObjectiveHighlightSystem : MonoBehaviour
    {
        public static ObjectiveHighlightSystem Instance { get; private set; }

        private readonly HashSet<ObjectiveHighlightComponent> components = new();
        [SerializeField] private float highlightScale = 2f;
        [SerializeField] private float normalScale = 1f;
        [SerializeField] private int normalLayer = 0;
        [SerializeField] private int highlightLayer = 200;
        internal void Track(ObjectiveHighlightComponent component)
        {
            components.Add(component);
        }
        internal void Untrack(ObjectiveHighlightComponent component)
        {
            components.Remove(component);
        }
        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += StateChanged;
            TurnManager.Instance.TurnChanged += TurnChanged;
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= StateChanged;
            TurnManager.Instance.TurnChanged -= TurnChanged;
        }
        private void StateChanged(NetworkData.GameState? state)
        {
            GameStateSynchronizer.Instance.StateChanged -= StateChanged;
            Refresh(NetworkData.StringToInGameId(state.Value.current_players_turn));
        }
        private void TurnChanged()
        {
            Refresh(TurnManager.Instance.TurnPlayerRole);
        }
        private void Refresh(NetworkData.InGameID? turnPlayer)
        {
            if (turnPlayer == null) return;
            foreach (var component in components)
            {
                var playerOwned = component.PlayerOwned;
                var spriteSorter = component.SpriteSorter;
                NetworkData.InGameID myRole = NetworkData.StringToInGameId(GameStateSynchronizer.Instance.Me.in_game_id);

                bool orchestratorPlaysAndThisIsMine = turnPlayer.Value == NetworkData.InGameID.Orchestrator && playerOwned.Owner == myRole;
                bool thisIsTurnPlayers = turnPlayer.Value == playerOwned.Owner;
                bool match = thisIsTurnPlayers || orchestratorPlaysAndThisIsMine;
                float scale = normalScale;
                if (match) scale = highlightScale;
                int layerMod = normalLayer;
                if (match) layerMod = highlightLayer;

                spriteSorter.HighlightLayer = layerMod;
                component.transform.SetGlobalScale(Vector3.one * scale);
            }
        }
    }
}