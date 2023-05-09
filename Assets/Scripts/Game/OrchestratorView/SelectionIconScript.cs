using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.OrchestratorView
{
    public class SelectionIconScript : MonoBehaviour
    {
        public GameObject iconMarker;
        void Start()
        {

        }

        void OnMouseOver()
        {
            setIconMarkerVisibility(true);
        }

        public void setIconMarkerVisibility(bool visible)
        {
            iconMarker.SetActive(visible);
        }
    }
}