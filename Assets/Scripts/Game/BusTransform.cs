using Common.Network;
using UnityEngine;

namespace Game
{
    public class BusTransform : MonoBehaviour
    {
        public SpriteRenderer selfSprite;

        private void Update()
        {
            var playersTurn = GameStateSynchronizer.Instance.GameState.Value.current_players_turn;
            if (playersTurn == NetworkData.InGameID.Orchestrator.ToString()) return;
            foreach (var player in GameStateSynchronizer.Instance.GameState.Value.players)
            {
                if (player.in_game_id != playersTurn) continue;
                var current_pos_id = player.position_node_id;
                if (current_pos_id.Value == transform.parent.GetComponent<INode>().Id)
                {
                    selfSprite.color = Color.green;
                    break;
                }
                selfSprite.color = Color.white;
            }
        }

        public void ToggleTransform()
        {
            var should_be_bus = !GameStateSynchronizer.Instance.Me.is_bus;
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id = GameStateSynchronizer.Instance.LobbyId.Value,
                input_type = NetworkData.PlayerInputType.SetPlayerBusBool.ToString(),
                related_bool = should_be_bus,
            };
            RestAPI.Instance.SendPlayerInput(success =>
            {
                UndoSystem.Instance.MovesDone++;
            }, failure =>
            {
                Debug.LogWarning(failure);
            }, input);
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ToggleTransform();
            }
        }
    }
}