using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View
{
    public class PriorityMarker : MonoBehaviour
    {
        public Text priorityText;
        void Start()
        {

        }

        public void setPriority(int prioity)
        {
            if (prioity <= 0) return;
            priorityText.text = "+" + prioity;
        }
        

        public void SetActive(bool boolean)
        {
            gameObject.SetActive(boolean);
        }
        void Update()
        {

        }
    }

}

