using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject currentPlayer;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;

    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;

    public GameObject goal1;
    public GameObject goal2;
    public GameObject goal3;
    public GameObject goal4;
    public GameObject goal5;

    // Start is called before the first frame update
    void Start()
    {
        setPos(player2, "I1");
        setVisibility(player1, true);
    }

    void setCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
    }
    void setPos(GameObject obj, string location)
    {

        GameObject parent = GameObject.Find(location);

        obj.transform.parent = parent.transform;
        obj.transform.localPosition = new Vector3(0, 0, 0);
    }

    void setVisibility(GameObject obj, bool visible)
    {
        //obj.GetComponent<SpriteRenderer>().enabled = visible;
        obj.SetActive(visible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
