using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace View 
{
    public class GameCard : MonoBehaviour
    {

        public int id;

        public int GetId() { return id; }

        public void moveTo(Vector3 newPos) //TODO write with vector3 instead
        {
            this.transform.position = newPos;
        }

    }
}
