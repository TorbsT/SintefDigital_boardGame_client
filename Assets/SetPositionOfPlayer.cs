using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOfPlayer : MonoBehaviour
{
    //public GameObject player;

    public string location;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setPos()
    {
        //GameObject parent = GameObject.Find("I1");
        GameObject parent = GameObject.Find(location);
        //Debug.Log(parent.transform.localPosition);

        player.parent = parent.transform;
        player.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
