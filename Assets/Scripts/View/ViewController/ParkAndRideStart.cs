using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

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
        if (NetworkData.Instance.Me.Value.in_game_id != NetworkData.InGameID.Orchestrator.ToString()) return;
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
        if (NetworkData.Instance.Me.Value.in_game_id != NetworkData.InGameID.Orchestrator.ToString()) return;
        mouseOver = true;
        if (parkAndRideStart.activeSelf == false)
        {
            highlight.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (NetworkData.Instance.Me.Value.in_game_id != NetworkData.InGameID.Orchestrator.ToString()) return;
        mouseOver = false;
        highlight.SetActive(false);
    }

    public void RemoveParkAndRide()
    {
        if (NetworkData.Instance.Me.Value.in_game_id != NetworkData.InGameID.Orchestrator.ToString()) return;
        parkAndRideStart.SetActive(false);
        unlocked--;
        readUnlocked = unlocked;
    }
}
