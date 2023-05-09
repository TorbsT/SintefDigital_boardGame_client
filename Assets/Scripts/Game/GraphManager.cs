using Common.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GraphManager : MonoBehaviour
    {
        public static GraphManager Instance { get; private set; }
        private Dictionary<int, INode> nodes = new();

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
        private void Awake()
        {
            Instance = this;
            foreach (INode node in GetComponentsInChildren<INode>())
            {
                nodes.Add(node.Id, node);
            }
        }
        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += StateChanged;
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= StateChanged;
        }
        private void StateChanged(NetworkData.GameState? state)
        {
            foreach (var parkAndRideNode in GetAllParkAndRideNodes())
            {
                parkAndRideNode.gameObject.GetComponentInChildren<BusTransform>(true).gameObject.SetActive(false);
            }

            foreach (var restriction in state.Value.edge_restrictions)
            {
                if (restriction.edge_restriction != NetworkData.RestrictionType.ParkAndRide.ToString()) continue;
                var node_one = GetNode(restriction.node_one);
                var node_two = GetNode(restriction.node_two);
                if (node_one.gameObject.tag == "ParkRide")
                {
                    node_one.gameObject.GetComponentInChildren<BusTransform>(true).gameObject.SetActive(true);
                }
                else if (node_two.gameObject.tag == "ParkRide")
                {
                    node_two.gameObject.GetComponentInChildren<BusTransform>(true).gameObject.SetActive(true);
                }
            }
        }
    }
}
