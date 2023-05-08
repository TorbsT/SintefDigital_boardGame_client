using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Network;
using View;

public class clickable : MonoBehaviour
{
    //TODO: Add other restrictions here

    public GameObject[] restrictionObjects; 

    public GameObject chooser;
    private static GameObject chooserObj;

    // Update is called once per frame
    void Update()
    {

        if (GameStateSynchronizer.Instance.GameState == null)
        {
            return;
        }
        bool noRestrictions = true;
        foreach (var restriction in GameStateSynchronizer.Instance.GameState.Value.edge_restrictions)
        {
            if (this.gameObject.name == $"{restriction.node_one}-{restriction.node_two}" || this.gameObject.name == $"{restriction.node_two}-{restriction.node_one}") {
                noRestrictions = false;
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

    private int[] GetNodeIDsFromName()
    {
        string[] nodes = name.Split('-');
        int node1 = int.Parse(nodes[0]);
        int node2 = int.Parse(nodes[1]);
        return new int[] { node1, node2 }; 
    }

    void OnMouseDown()
    {
        if (GameStateSynchronizer.Instance.Me.in_game_id != NetworkData.InGameID.Orchestrator.ToString() || GameStateSynchronizer.Instance.GameState.Value.current_players_turn != NetworkData.InGameID.Orchestrator.ToString())
        {
            return;
        }

        if (transform.childCount == 0)
        {
            chooserObj = Instantiate(chooser, transform);
            chooserObj.transform.localPosition = new Vector2(-26, -10);
            var node_ids = GetNodeIDsFromName();
            var node_one = GraphManager.Instance.GetNode(node_ids[0]);
            var node_two = GraphManager.Instance.GetNode(node_ids[1]);
            bool neighbouring_park_and_ride_restriction_exists = false;

            foreach (var restriction in GameStateSynchronizer.Instance.GameState.Value.edge_restrictions)
            {
                if (restriction.edge_restriction != NetworkData.RestrictionType.ParkAndRide.ToString()) continue;
                if (restriction.node_one != node_ids[0] && restriction.node_one != node_ids[1] && restriction.node_two != node_ids[0] && restriction.node_two != node_ids[1]) continue;
                neighbouring_park_and_ride_restriction_exists = true;
            }
            Debug.Log($"{node_one.gameObject.tag == "ParkRide"}, {node_two.gameObject.tag == "ParkRide"}, {neighbouring_park_and_ride_restriction_exists}");
            if (node_one.gameObject.tag == "ParkRide" || node_two.gameObject.tag == "ParkRide" || neighbouring_park_and_ride_restriction_exists)
            {
                foreach (var child in chooserObj.GetComponentsInChildren<Transform>(true))
                {
                    if (child.tag != "ParkRide") continue;
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
    public void AddEdgeRestriction(NetworkData.RestrictionType restriction)
    {
        var node_ids = GetNodeIDsFromName();
        RestAPI.Instance.SetEdgeRestriction(success => {
            //Debug.Log("Succsessfullly added restriction");
        }, failure => { Debug.Log("could not add restriction to this edge : " + failure); }, node_ids[0], node_ids[1], restriction, false);
    }

    public void RemoveEdgeRestriction(NetworkData.RestrictionType restriction)
    {
        var node_ids = GetNodeIDsFromName();
        RestAPI.Instance.SetEdgeRestriction(success => {
            //Debug.Log("Succsessfullly removed restriction");
        }, failure => { Debug.Log("could not remove restriction from this edge : " + failure); }, node_ids[0], node_ids[1], restriction, true);
    }
}
