using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeTraversal : MonoBehaviour
{
    public GameObject[] neighbourNodes;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(GameObject.Find("gameBoard/CitySquare").transform.GetSiblingIndex());
        //Debug.Log(GameObject.Find("gameBoard").transform.GetChild(2).GetType());
        //Debug.Log(transform.GetSiblingIndex());
        Steps steps = GetComponent<Steps>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
