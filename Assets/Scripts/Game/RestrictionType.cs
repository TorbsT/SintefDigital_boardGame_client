using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Network;

namespace Game
{
    public class RestrictionType : MonoBehaviour
    {
        public NetworkData.RestrictionType type;

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && transform.parent.tag == "edgeChooser")
            {
                gameObject.transform.parent.GetComponentInParent<clickable>().AddEdgeRestriction(type);
                Destroy(gameObject.transform.parent.gameObject);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                gameObject.GetComponentInParent<clickable>().RemoveEdgeRestriction(type);
            }
        }
    }
}