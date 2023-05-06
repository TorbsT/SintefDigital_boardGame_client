using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkAndRideStart : MonoBehaviour
{
    public GameObject parkAndRideStart;
    public GameObject highlight;

    private bool mouseOver;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseOver)
        {
            parkAndRideStart.SetActive(true);
            highlight.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
        if (parkAndRideStart.activeSelf == false)
        {
            highlight.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        highlight.SetActive(false);
    }

    public void RemoveParkAndRide()
    {
        parkAndRideStart.SetActive(false);
    }
}
