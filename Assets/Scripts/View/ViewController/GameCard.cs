using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace View 
{
    public class GameCard : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Id { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Traffic { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Goal { get; private set; }

        public int id;

        public int GetId() { return id; }

        public void moveTo(Vector3 newPos) //TODO write with vector3 instead
        {
            return;
            this.transform.position = newPos;
        }

    }
}
