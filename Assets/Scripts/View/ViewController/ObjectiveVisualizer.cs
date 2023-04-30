﻿using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    internal class ObjectiveVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject startPrefab;
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private GameObject goalPrefab;
        private void Start()
        {
            Invoke(nameof(CursedShit), 1f);
        }
        private void CursedShit()
        {
            foreach (var player in GameStateSynchronizer.Instance.GameState.Value.players)
            {
                if (player.unique_id == GameStateSynchronizer.Instance.Orchestrator.Value.unique_id)
                    continue;
                NetworkData.PlayerObjectiveCard? card = player.objective_card;
                if (card == null) Debug.LogError("What in the fuck");
                NetworkData.InGameID role =
                    (NetworkData.InGameID)Enum.Parse(typeof(NetworkData.InGameID), player.in_game_id);
                Spawn(startPrefab, card.Value.start_node_id, role);
                Spawn(pickupPrefab, card.Value.pick_up_node_id, role);
                Spawn(goalPrefab, card.Value.drop_off_node_id, role);
            }
        }
        private void Spawn(GameObject prefab, int nodeId, NetworkData.InGameID role)
        {
            GameObject go = PoolManager.Depool(prefab);
            INode node = GraphManager.Instance.GetNode(nodeId);
            go.GetComponent<PlayerOwned>().Owner = role;
            go.transform.SetParent(node.gameObject.transform, false);
        }
    }
}