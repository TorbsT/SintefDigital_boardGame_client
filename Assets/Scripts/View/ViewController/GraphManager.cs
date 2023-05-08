using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class GraphManager : MonoBehaviour
    {
        public static GraphManager Instance { get; private set; }
        private Dictionary<int, INode> nodes = new();

        private void Awake()
        {
            Instance = this;
            foreach (INode node in GetComponentsInChildren<INode>())
            {
                nodes.Add(node.Id, node);
            }
        }
        public INode GetNode(int id)
        {
            return nodes[id];
        }
        public ICollection<int> CopyNodes()
        {
            HashSet<int> nodes = new();
            foreach (var node in this.nodes.Keys)
                nodes.Add(node);
            return nodes;
        }
        public List<INode> GetAllParkAndRideNodes()
        {
            var parkAndRideNodeList = new List<INode>();
            foreach (var node in nodes.Values)
            {
                if (node.gameObject.tag == "ParkRide") parkAndRideNodeList.Add(node);
            }
            return parkAndRideNodeList;
        }
    }
}
