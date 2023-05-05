using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{
    public Sprite sprite;
    SpriteRenderer spriteRenderer;
    private bool clicked = true;
    static bool unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //Debug.Log("clicked");
        if (clicked == true)
        {
            if (CompareTag("ParkRide"))
            {
                unlocked = !unlocked;
                spriteRenderer.sprite = sprite;
            }
            if (unlocked && clicked)
            {
                spriteRenderer.sprite = sprite;
            }
            clicked = !clicked;
        }
        else if (clicked == false)
        {
            if (CompareTag("ParkRide"))
            {
                unlocked = !unlocked;
                spriteRenderer.sprite = null;
            }
            if (unlocked && !clicked)
            {
                spriteRenderer.sprite = null;
            }
            clicked = !clicked;
        }
        Debug.Log(clicked);
        Debug.Log(unlocked);
    }
}
