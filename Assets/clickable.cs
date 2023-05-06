using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class clickable : MonoBehaviour
{
    public Sprite sprite;
    SpriteRenderer spriteRenderer;
    private bool clicked = true;
    //static int unlocked = 0;
    public ParkAndRideStart parkAndRideStart;

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
        if (clicked == true)
        {
            if ((parkAndRideStart.readUnlocked > 0) && clicked)
            {
                float w = 7; //8
                float h = 6 * (0.28f/transform.localScale.y);
                spriteRenderer.sprite = sprite;
                spriteRenderer.size = new Vector2(w, h);
            }
            clicked = !clicked;
        }
        else if (clicked == false)
        {
            if ((parkAndRideStart.readUnlocked > 0) && !clicked)
            {
                spriteRenderer.sprite = null;
            }
            clicked = !clicked;
        }
        Debug.Log(parkAndRideStart.readUnlocked);
        Debug.Log(clicked);
    }
}
