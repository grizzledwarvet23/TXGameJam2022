using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBlue : MonoBehaviour
{
    private bool yellow;
    // Start is called before the first frame update
    void Start()
    {
        yellow = SpaceModifier.isPositiveSpace;
    }

    public void swap()
    {
        yellow = !yellow;
        if(yellow)
        {
            tag = "WallJumpable";
        }
        else
        {
            tag = "Slippery";
        }
        

    }
    
}
