using Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Attached to a car gameobject to identify its vehicle types,
    /// e.g. which restrictions it satisfies
    /// </summary>
    internal class VehicleType : MonoBehaviour
    {
        [SerializeField] private List<NetworkData.RestrictionType> types = new();
        [SerializeField] private bool cargoIsPeople;  // Only applies to heavy type

        public bool Exactly(ICollection<NetworkData.RestrictionType> types, bool cargoIsPeople)
        {
            foreach (var type in types)
                if (!this.types.Contains(type))
                    return false;
            foreach (var type in this.types)
                if (!types.Contains(type))
                    return false;
            if (types.Contains(NetworkData.RestrictionType.Heavy) &&
                this.types.Contains(NetworkData.RestrictionType.Heavy) &&
                cargoIsPeople != this.cargoIsPeople) return false;
            return true;
        }
    }
}
