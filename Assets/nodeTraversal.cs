using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeTraversal : MonoBehaviour
{
    public GameObject player;

    public GameObject[] neighbourNodes;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    void Start()
    {
        //Debug.Log(GameObject.Find("gameBoard/CitySquare").transform.GetSiblingIndex());
        //Debug.Log(GameObject.Find("gameBoard").transform.GetChild(2).GetType());
        //Debug.Log(transform.GetSiblingIndex());
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (neighbourNodes.Contains(player.transform.parent.gameObject))
        {
            //Debug.Log("COLOR!!!");
            spriteRenderer.color = Color.cyan;
        }
    }

    private void OnMouseExit()
    {
            //Debug.Log("Reset!!!");
            spriteRenderer.color = Color.white;
    }

    void OnMouseDown()
    {
        if (neighbourNodes.Contains(player.transform.parent.gameObject))
        {
            //Debug.Log("clicked");
            player.transform.parent = transform;
            player.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

}
