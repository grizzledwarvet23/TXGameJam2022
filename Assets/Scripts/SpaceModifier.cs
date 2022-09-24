using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceModifier : MonoBehaviour
{
    private bool isPositiveSpace;
    public GameObject blackWhiteMaps;
    public GameObject toggleColliders;

    public Camera camera;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        isPositiveSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            switchSpace();
        }
    }

    void switchSpace()
    {
        isPositiveSpace = !isPositiveSpace;
            if(isPositiveSpace)
            {
                foreach(Tilemap map in blackWhiteMaps.GetComponentsInChildren<Tilemap>())
                {
                    map.color = Color.black;
                } 
                player.GetComponent<SpriteRenderer>().color = Color.black;
                camera.backgroundColor = Color.white;
            }
            else
            {
                foreach(Tilemap map in blackWhiteMaps.GetComponentsInChildren<Tilemap>())
                {
                    map.color = Color.white;
                }
                player.GetComponent<SpriteRenderer>().color = Color.white;
                camera.backgroundColor = Color.black;
            }
            foreach(Tilemap map in toggleColliders.GetComponentsInChildren<Tilemap>())
            {
                map.GetComponent<TilemapCollider2D>().enabled = !map.GetComponent<TilemapCollider2D>().enabled;
            }
    }
}
