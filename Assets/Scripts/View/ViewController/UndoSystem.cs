using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class UndoSystem : MonoBehaviour
    {
        public static UndoSystem Instance { get; private set; }
        [field: SerializeField] public Button UndoButton { get; private set; }
        public int MovesDone { get; set; }

        private bool undoing;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            GameStateSynchronizer.Instance.StateChanged += StateChanged;
        }
        private void StateChanged(NetworkData.GameState? state)
        {
            if (state == null) return;
            bool show = !undoing && TurnManager.Instance.IsMyTurn && MovesDone > 0;
            SetUndoAvailable(show);
        }
        public void UndoLast()
        {
            SetUndoAvailable(false);
            undoing = true;
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.UndoAction.ToString(),
                related_role = GameStateSynchronizer.Instance.Me.in_game_id
            };
            RestAPI.Instance.SendPlayerInput(
                success =>
                {
                    undoing = false;
                    MovesDone--;
                },
                failure => { undoing = false; },
                input
                );
        }
        private void SetUndoAvailable(bool show)
        {
            //UndoButton.interactable = show;
            UndoButton.gameObject.SetActive(show);
        }
    }
}