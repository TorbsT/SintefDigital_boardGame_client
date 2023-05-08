using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Network;

public class clickable : MonoBehaviour
{
    //TODO: Add other restrictions here

    public GameObject[] restrictionObjects; 
    public Sprite sprite;
    SpriteRenderer spriteRenderer;
    private bool clicked = true;
    public ParkAndRideStart parkAndRideStart;

    public GameObject chooser;
    private static GameObject chooserObj;



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
        bool noRestrictions = true;
        spriteRenderer.sprite = null;
        foreach (var restriction in GameStateSynchronizer.Instance.GameState.Value.edge_restrictions)
        {
            if (this.gameObject.name == $"{restriction.node_one}-{restriction.node_two}" || this.gameObject.name == $"{restriction.node_two}-{restriction.node_one}") {
                noRestrictions = false;
                if (restriction.edge_restriction == NetworkData.RestrictionType.ParkAndRide.ToString())
                {
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.size = new Vector2(3, 1.25f);
                }
                foreach (var obj in restrictionObjects)
                {
                    if (restriction.edge_restriction == obj.GetComponent<RestrictionType>().type.ToString())
                    {
                        bool exists = false;
                        foreach(RestrictionType child in gameObject.GetComponentsInChildren<RestrictionType>())
                        {
                            if (child.type.ToString() == restriction.edge_restriction)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (exists) continue;
                        var spawnObj = Instantiate(obj);
                        spawnObj.transform.SetParent(transform, false);
                    }
                }
            }
        }
        if (noRestrictions)
        {
            foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
            {
                if (child.tag == "edgeChooser") continue;
                Destroy(child.gameObject);
            }
        }
    }

    void OnMouseDown()
    {
        if (GameStateSynchronizer.Instance.Me.in_game_id != NetworkData.InGameID.Orchestrator.ToString())
        {
            return;
        }
        

        if (clicked == true)
        {
            if ((ParkAndRideStart.readUnlocked > 0) && clicked)
            {
                AddEdgeRestriction(NetworkData.RestrictionType.ParkAndRide);
                clicked = !clicked;
            }
            else if(transform.childCount == 0)
            {

                chooserObj = Instantiate(chooser, transform);
                chooserObj.transform.localPosition = new Vector2(-26, -10);

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
    }
    public void AddEdgeRestriction(NetworkData.RestrictionType restriction)
    {

        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);
        RestAPI.Instance.SetEdgeRestriction(success => {
            //Debug.Log("Succsessfullly added restriction");
        }, failure => { Debug.Log("could not add restriction to this edge : " + failure); }, node1, node2, restriction, false);
    }

    public void RemoveEdgeRestriction(NetworkData.RestrictionType restriction)
    {

        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);
        RestAPI.Instance.SetEdgeRestriction(success => {
            spriteRenderer.sprite = null;
            //Debug.Log("Succsessfullly removed restriction");
        }, failure => { Debug.Log("could not remove restriction from this edge : " + failure); }, node1, node2, restriction, true);
    }
}
