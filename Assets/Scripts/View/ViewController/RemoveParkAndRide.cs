using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParkAndRide : MonoBehaviour
{
    public ParkAndRideStart parent;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            parent.RemoveParkAndRide();
        }
    }
}
