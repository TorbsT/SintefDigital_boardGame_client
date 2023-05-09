using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common.Network
{
    [CreateAssetMenu(menuName = "Scriptable Objects/IP", fileName = "IPObject")]
    internal class IPObject : ScriptableObject
    {
        [field: SerializeField] public string URL { get; set; }
        [field: SerializeField] public int Port;
    }
}
