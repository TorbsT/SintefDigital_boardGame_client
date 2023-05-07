using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class GameController : MonoBehaviour
    {
        private Dictionary<int, GameObject> players = new();
        [SerializeField] private GameObject playerPrefab;
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.PlayerConnected += PlayerConnected;
            GameStateSynchronizer.Instance.PlayerDisconnected += PlayerDisconnected;
            CompleteRefreshPlayers();
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.PlayerConnected -= PlayerConnected;
            GameStateSynchronizer.Instance.PlayerDisconnected -= PlayerDisconnected;
        }
        private void PlayerConnected(NetworkData.Player player)
        {
            GameObject playerGameObject = PoolManager.Instance.Depool(playerPrefab);
            players.Add(player.unique_id, playerGameObject);
            //RefreshPosition(player); Do this later
        }
        private void PlayerDisconnected(int playerId)
        {
            GameObject player = players[playerId];
            PoolManager.Instance.Enpool(player);
            players.Remove(playerId);
        }
        private void CompleteRefreshPlayers()
        {
            foreach (var player in players.Keys)
            {
                PlayerDisconnected(player);
            }
            players = new();

            foreach (var player in GameStateSynchronizer.Instance.GameState.Value.players)
            {
                PlayerConnected(player);
            }
        }
        private void RefreshPosition(NetworkData.Player player)
        {
            int playerId = player.unique_id;
            GameObject playerGameObject = players[playerId];
            GameObject node = GraphManager.Instance.GetNode(player.position_node_id.Value).gameObject;
            playerGameObject.transform.position = node.transform.position;
        }
    }
}