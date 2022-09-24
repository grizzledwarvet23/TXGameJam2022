using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float period;
    
    public bool isHorizontal;

    private float timeStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        timeStarted = Time.timeSinceLevelLoad;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isHorizontal)
        {

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, speed * Mathf.Sin( (Time.timeSinceLevelLoad - timeStarted) / period));
        }
    }
}
