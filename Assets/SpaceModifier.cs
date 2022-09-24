using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceModifier : MonoBehaviour
{
    private bool canSwitch;
    private bool isPositiveSpace;
    private float swapDuration = 0.3f;
    public GameObject blackWhiteMaps;

    public SpriteRenderer[] positiveSprites;
    public GameObject toggleMapColliders;

    public GameObject enemies;
    public Collider2D[] otherColliders;

    public Camera camera;
    public GameObject player;

    private float fadeK = 15f;
    // Start is called before the first frame update
    void Start()
    {
        canSwitch = true;
        isPositiveSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canSwitch)
        {
            StartCoroutine(switchSpace());
        }
    }

    IEnumerator switchSpace()
    {
        canSwitch = false;
        isPositiveSpace = !isPositiveSpace;
            if(isPositiveSpace)
            {

                foreach(Tilemap map in blackWhiteMaps.GetComponentsInChildren<Tilemap>())
                {
                    StartCoroutine(fadeColor(map, Color.white, Color.black));
                }
                foreach(SpriteRenderer renderer in positiveSprites)
                {
                    StartCoroutine(fadeColor(renderer, Color.white, Color.black));
                } 
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.white, Color.black));
                StartCoroutine(fadeColor(camera, Color.black, Color.white));
            }
            else
            {
                foreach(Tilemap map in blackWhiteMaps.GetComponentsInChildren<Tilemap>())
                {
                    StartCoroutine(fadeColor(map, Color.black, Color.white));
                } 
                foreach(SpriteRenderer renderer in positiveSprites)
                {
                    StartCoroutine(fadeColor(renderer, Color.black, Color.white));
                } 
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.black, Color.white));
                StartCoroutine(fadeColor(camera, Color.white, Color.black));
            }
            foreach(Tilemap map in toggleMapColliders.GetComponentsInChildren<Tilemap>())
            {
                map.GetComponent<TilemapCollider2D>().enabled = !map.GetComponent<TilemapCollider2D>().enabled;
            }
            foreach(Collider2D otherCollider in otherColliders)
            {
                otherCollider.enabled = !otherCollider.enabled;
            }
            foreach(Enemy enemy in enemies.GetComponentsInChildren<Enemy>())
            {
                enemy.swap();
            } 
        yield return new WaitForSeconds(swapDuration);
        canSwitch = true;
    }

    IEnumerator fadeColor(SpriteRenderer rend, Color startColor, Color endColor)
    {
        rend.color = startColor;
        for(int i = 0; i <= fadeK; i++)
        {
            rend.color = Color.Lerp(startColor, endColor, i/fadeK);
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    IEnumerator fadeColor(Tilemap rend, Color startColor, Color endColor)
    {
        rend.color = startColor;
        for(int i = 0; i <= fadeK; i++)
        {
            rend.color = Color.Lerp(startColor, endColor, i/fadeK);
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    IEnumerator fadeColor(Camera rend, Color startColor, Color endColor)
    {
        rend.backgroundColor = startColor;
        for(int i = 0; i <= fadeK; i++)
        {
            rend.backgroundColor = Color.Lerp(startColor, endColor, i/fadeK);
            yield return new WaitForSeconds(0.02f);
        }
        
    }
    
}
