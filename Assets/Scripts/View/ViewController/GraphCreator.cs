using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    [ExecuteInEditMode]
    public class GraphCreator : MonoBehaviour
    {
        private enum GizmosMode
        {
            None,
            Selected,
            Always
        }
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GizmosMode gizmosMode = GizmosMode.Always;
        [SerializeField] private float gizmosNodeSize = 1f;
        [SerializeField] private Color gizmosNodeColor = Color.yellow;
        [SerializeField] private Color gizmosEdgeColor = Color.yellow;
        [SerializeField] private string serializedGraphOutput;
        private void OnDrawGizmos()
        {
            DrawGizmos(GizmosMode.Always);
        }
        private void OnDrawGizmosSelected()
        {
            DrawGizmos(GizmosMode.Selected);
        }
        private void DrawGizmos(GizmosMode mode)
        {
            if (mode != gizmosMode) return;
            foreach (INode nodeA in GetComponentsInChildren<INode>())
            {
                Vector2 posA = nodeA.gameObject.transform.position;
                foreach (INode nodeB in nodeA.GetNeighbours())
                {
                    if (nodeB == null) continue;
                    Vector2 posB = nodeB.gameObject.transform.position;
                    Gizmos.color = gizmosEdgeColor;
                    Gizmos.DrawLine(posA, posB);
                }
                Gizmos.color = gizmosNodeColor;
                Gizmos.DrawCube(posA, Vector3.one * gizmosNodeSize);
            }
        }
        void OnTransformChildrenChanged()
        {
            int nodeId = 0;
            foreach (INode node in GetComponentsInChildren<INode>())
            {
                node.gameObject.name = nodeId.ToString();
                nodeId++;
            }
        }
        public void AddNode()
        {
            GameObject node = Instantiate(nodePrefab);
            node.transform.SetParent(transform);
        }
        public void SerializeGraph()
        {
            List<string> result = new();
            foreach (INode node in GetComponentsInChildren<INode>())
            {
                List<string> neighbourStrings = new();
                foreach (INode neig in node.GetNeighbours())
                    neighbourStrings.Add(neig.gameObject.name);
                string nodeDetails = $"neigs:{string.Join(",", neighbourStrings)}";
                result.Add(nodeDetails);
            }

            serializedGraphOutput = string.Join(";", result);
        }
    }
    public interface INode
    {
        ICollection<INode> GetNeighbours();
        public GameObject gameObject { get; }
    }
}