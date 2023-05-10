using UnityEngine;

namespace Common.Network
{
    [CreateAssetMenu(menuName = "Scriptable Objects/IP", fileName = "IPObject")]
    public class IPObject : ScriptableObject
    {
        [field: SerializeField] public string URL { get; set; }
        [field: SerializeField] public int Port;
    }
}
