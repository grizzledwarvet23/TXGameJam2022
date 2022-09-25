using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameObject enemies;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = collision.gameObject.GetComponent<PlayerMove>().spawnPosition;

            foreach(Enemy enemy in enemies.GetComponentsInChildren<Enemy>())
            {
                enemy.reset();
            }
        }
    }
}
