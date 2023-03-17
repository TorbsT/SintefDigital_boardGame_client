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
            if (Input.GetMouseButtonDown(0))
            {
                GameObject newParent = parent.GetComponent<NodeTraversal>().neighbourNodes[0];
                transform.parent = newParent.transform;
                parent = transform.parent.gameObject;
                transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
