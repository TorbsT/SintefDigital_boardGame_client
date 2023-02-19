using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class TempNode : MonoBehaviour, INode
    {
        [SerializeField] private List<TempNode> neighbours = new();
        public ICollection<INode> GetNeighbours()
        {
            List<INode> result = new();
            foreach (INode node in neighbours)
            {
                result.Add(node);
            }
            return result;
        }
    }
}
