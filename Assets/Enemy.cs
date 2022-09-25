using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool hostile; 
    private Rigidbody2D rb;
    private GameObject player;
    public Vector2 spawnPosition;

    private bool canFollow = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        hostile = SpaceModifier.isPositiveSpace;
        spawnPosition = transform.position;
        //follow the player if hostile
    }

    void FixedUpdate()
    {
        //follow the player if hostile
        if(hostile && canFollow)
        {
            float xSpeed = (player.transform.position.x - transform.position.x) * 0.6f;
            float ySpeed = (player.transform.position.y - transform.position.y) * 0.6f;

                if(Mathf.Abs(xSpeed) > 14)
            {
                xSpeed = Mathf.Sign(xSpeed) * 14;
            }
            else if(Mathf.Abs(xSpeed) < 1)
            {
                xSpeed = Mathf.Sign(xSpeed) * 1;
            }
            if(Mathf.Abs(ySpeed) > 14)
            {
                ySpeed = Mathf.Sign(ySpeed) * 14;
            }
            else if(Mathf.Abs(ySpeed) < 1)
            {
                ySpeed = Mathf.Sign(ySpeed) * 1;
            }

            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hostile)
        {
            collision.gameObject.transform.position = collision.gameObject.GetComponent<PlayerMove>().spawnPosition;
            reset();
        }
    }

    public void reset()
    {
        transform.position = spawnPosition;
        canFollow = false;
        StartCoroutine(setFollow());
    }

    IEnumerator setFollow()
    {
        yield return new WaitForSeconds(0.25f);
        canFollow = true;
    }

    

    public void swap()
    {
        hostile = !hostile;
        if(hostile) //act as an enemy
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 6;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else //become a platform
        {
            gameObject.tag = "Ground";
            gameObject.layer = 6;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }


}
