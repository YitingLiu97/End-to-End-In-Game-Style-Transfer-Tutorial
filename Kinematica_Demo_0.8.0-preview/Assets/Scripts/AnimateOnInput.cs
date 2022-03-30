using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnInput : MonoBehaviour
{
    // Start is called before the first frame update

    public bool playAnimation;
    private Animation animationParallex;
    void Start()
    {
        animationParallex = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playAnimation)
        {


            animationParallex.Play();

        }
        else {

            animationParallex.Stop();
        
        }
        
    }
}
