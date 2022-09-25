using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceModifier : MonoBehaviour
{
    private bool canSwitch;

    [System.NonSerialized]
    public static bool isPositiveSpace;
    private float swapDuration = 0.2f;
    public GameObject blackWhiteMaps;
    public Tilemap yellowBlueMap; 

    public SpriteRenderer[] positiveSprites;
    public GameObject toggleMapColliders;

    public AudioSource positiveMusic;
    public AudioSource negativeMusic;

    public GameObject enemies;
    public Collider2D[] otherColliders;

    public Camera camera;
    public GameObject player;

    private float fadeK = 10f;
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
                StartCoroutine(fadeColor(yellowBlueMap, Color.blue, Color.yellow)); 
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.white, Color.black));
                StartCoroutine(fadeColor(camera, Color.black, Color.white));

                positiveMusic.volume = 1;
                negativeMusic.volume = 0;
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
                StartCoroutine(fadeColor(yellowBlueMap, Color.yellow, Color.blue));
                StartCoroutine(fadeColor(player.GetComponent<SpriteRenderer>(), Color.black, Color.white));
                StartCoroutine(fadeColor(camera, Color.white, Color.black));

                positiveMusic.volume = 0;
                negativeMusic.volume = 1;
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
            yellowBlueMap.GetComponent<YellowBlue>().swap(); 
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
