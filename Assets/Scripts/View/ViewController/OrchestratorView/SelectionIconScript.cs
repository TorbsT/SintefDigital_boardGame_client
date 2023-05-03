using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
