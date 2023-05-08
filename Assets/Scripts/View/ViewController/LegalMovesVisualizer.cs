using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
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
            Debug.Log("ye");
            if (!state.HasValue) return;

            HashSet<int> toNodes = new();
            foreach (var node in state.Value.legal_nodes)
                toNodes.Add(node);

            foreach (var node in toNodes)
                Debug.Log("YEEPPSHH " + node);

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
