using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    // Attached to a car gameobject to identify its vehicle types
    internal class VehicleType : MonoBehaviour
    {
        [SerializeField] private List<NetworkData.RestrictionType> types = new();

        public bool Exactly(ICollection<NetworkData.RestrictionType> types)
        {
            foreach (var type in types)
                if (!this.types.Contains(type))
                    return false;
            foreach (var type in this.types)
                if (!types.Contains(type))
                    return false;
            return true;
        }
    }
}
