using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Network;

public class clickable : MonoBehaviour
{
    //TODO: Add other restrictions here

    public Sprite sprite;
    SpriteRenderer spriteRenderer;
    private bool clicked = true;
    public ParkAndRideStart parkAndRideStart;

    public GameObject chooser;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateSynchronizer.Instance.GameState == null)
        {
            return;
        }

        spriteRenderer.sprite = null;
        foreach (var restriction in GameStateSynchronizer.Instance.GameState.Value.edge_restrictions)
        {
            if (this.gameObject.name == $"{restriction.node_one}-{restriction.node_two}" || this.gameObject.name == $"{restriction.node_two}-{restriction.node_one}") {
                if (restriction.edge_restriction == NetworkData.RestrictionType.ParkAndRide.ToString())
                {
                    float w = 7;
                    float h = 6 * (0.28f / transform.localScale.y);
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.size = new Vector2(w, h);
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (GameStateSynchronizer.Instance.Me.in_game_id != NetworkData.InGameID.Orchestrator.ToString())
        {
            return;
        }
        
        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);

        if (clicked == true)
        {
            if ((ParkAndRideStart.readUnlocked > 0) && clicked)
            {
                AddEdgeRestriction(NetworkData.RestrictionType.ParkAndRide);
                clicked = !clicked;
            }
            else
            {
                chooser.SetActive(true);
            }
        }
        else if (clicked == false)
        {
            if ((ParkAndRideStart.readUnlocked > 0) && !clicked)
            {
                RemoveEdgeRestriction(NetworkData.RestrictionType.ParkAndRide);
                clicked = !clicked;
            }
        }
        //Debug.Log(parkAndRideStart.readUnlocked);
        //Debug.Log(clicked);
    }
    public void AddEdgeRestriction(NetworkData.RestrictionType restriction)
    {

        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);
        RestAPI.Instance.SetEdgeRestriction(success => {
            float w = 7; //8
            float h = 6 * (0.28f / transform.localScale.y);
            spriteRenderer.sprite = sprite;
            spriteRenderer.size = new Vector2(w, h);
        }, failure => { Debug.Log("could not add restriction to this edge : " + failure); }, node1, node2, restriction, false);
    }

    public void RemoveEdgeRestriction(NetworkData.RestrictionType restriction)
    {

        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);
        RestAPI.Instance.SetEdgeRestriction(success => {
            spriteRenderer.sprite = null;
        }, failure => { Debug.Log("could not remove restriction from this edge : " + failure); }, node1, node2, restriction, true);
    }
}
