using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTransition : MonoBehaviour
{
    public Vector2 newPosition;
    public Vector2 newSpawnPosition;
    public GameObject oldCamera;
    public GameObject newCamera;

    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDisable;
    
    void Start()
    {
        if(newSpawnPosition.x == 0 && newSpawnPosition.y == 0)
        {
            newSpawnPosition = newPosition;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = newPosition;
            collision.gameObject.GetComponent<PlayerMove>().spawnPosition = newSpawnPosition;
            oldCamera.SetActive(false);
            newCamera.SetActive(true);

            foreach(GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
            foreach(GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }
}
