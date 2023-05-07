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
        if(Input.GetMouseButtonDown(0) && tag == "edgeChooser")
        {
            gameObject.transform.parent.GetComponentInParent<clickable>().AddEdgeRestriction(type);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<clickable>().RemoveEdgeRestriction(type);
        }
    }
}
