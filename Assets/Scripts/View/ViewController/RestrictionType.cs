using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class RestrictionType : MonoBehaviour
{
    public NetworkData.RestrictionType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && transform.parent.tag == "edgeChooser")
        {
            gameObject.transform.parent.GetComponentInParent<clickable>().AddEdgeRestriction(type);
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<clickable>().RemoveEdgeRestriction(type);
        }
    }
}
