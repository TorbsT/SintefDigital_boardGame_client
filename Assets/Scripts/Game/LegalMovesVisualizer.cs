using Common.Network;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    internal class LegalMovesVisualizer : MonoBehaviour
    {
        [SerializeField] private List<int> legalMoves = new();
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += ChangeState;
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= ChangeState;
        }
        private void ChangeState(NetworkData.GameState? state)
        {
            if (!state.HasValue) return;

            HashSet<int> toNodes = new();
            foreach (var node in state.Value.legal_nodes)
                toNodes.Add(node);

            Dictionary<int, bool> nodeShowPairs = new();
            foreach (var node in GraphManager.Instance.CopyNodes())
                nodeShowPairs.Add(node, toNodes.Contains(node));

            foreach(var node in nodeShowPairs.Keys)
            {
                bool show = nodeShowPairs[node];
                NodeTraversal nt = GraphManager.Instance.GetNode(node).gameObject.GetComponent<NodeTraversal>();
                nt.SetInteractable(show);
            }
            foreach (var node in toNodes)
                legalMoves.Add(node);
        }
    }
}
