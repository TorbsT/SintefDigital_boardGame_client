using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkAndRideStart : MonoBehaviour
{
    public GameObject parkAndRideStart;
    public GameObject highlight;

    private bool mouseOver;

    //public bool clicked = true;
    public static int unlocked = 0;
    public int readUnlocked = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseOver)
        {
            parkAndRideStart.SetActive(true);
            highlight.SetActive(false);
            unlocked++;
            readUnlocked = unlocked;
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
        unlocked--;
        readUnlocked = unlocked;
    }
    //void OnMouseDown()
    //{
    //    if (clicked == true)
    //    {
    //        if (CompareTag("ParkRide"))
    //        {
    //            unlocked++;
    //            readUnlocked = unlocked;
    //        }
    //        clicked = !clicked;
    //    }
    //    else if (clicked == false)
    //    {
    //        if (CompareTag("ParkRide"))
    //        {
    //            unlocked--;
    //            readUnlocked = unlocked;
    //        }
    //        clicked = !clicked;
    //    }
    //    Debug.Log(unlocked);
    //    Debug.Log(clicked);
    //}
}
