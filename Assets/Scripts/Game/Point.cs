using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class Point : MonoBehaviour //dummy class for locations where we move things
    {
        public Vector3 GetPos()
        {
            return transform.position;
        }
    }
}
