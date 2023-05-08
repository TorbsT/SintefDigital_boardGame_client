using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace View
{
    public class Movement : MonoBehaviour
    {
        GameObject parent;
        // Start is called before the first frame update
        void Start()
        {
            parent = transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
