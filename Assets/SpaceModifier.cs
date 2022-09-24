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
                    StartCoroutine(fadeColor(map, Color.white, Color.black));
                } 
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.white, Color.black));
                camera.backgroundColor = Color.white;
            }
            else
            {
                foreach(Tilemap map in blackWhiteMaps.GetComponentsInChildren<Tilemap>())
                {
                    StartCoroutine(fadeColor(map, Color.black, Color.white));
                } 
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.black, Color.white));
                camera.backgroundColor = Color.black;
            }
            foreach(Tilemap map in toggleColliders.GetComponentsInChildren<Tilemap>())
            {
                map.GetComponent<TilemapCollider2D>().enabled = !map.GetComponent<TilemapCollider2D>().enabled;
            }
    }

    IEnumerator fadeColor(SpriteRenderer rend, Color startColor, Color endColor)
    {
        rend.color = startColor;
        for(int i = 0; i <= 60; i++)
        {
            rend.color = Color.Lerp(startColor, endColor, i/60f);
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    IEnumerator fadeColor(Tilemap rend, Color startColor, Color endColor)
    {
        rend.color = startColor;
        for(int i = 0; i <= 60; i++)
        {
            rend.color = Color.Lerp(startColor, endColor, i/60f);
            yield return new WaitForSeconds(0.02f);
        }
        
    }
    
}
