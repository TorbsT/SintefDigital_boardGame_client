using UnityEngine;

namespace Game
{
    /// <summary>
    /// Redundant, but has references in scene
    /// </summary>
    public class Point : MonoBehaviour //dummy class for locations where we move things
    {
        public Vector3 GetPos()
        {
            return transform.position;
        }
    }
}
