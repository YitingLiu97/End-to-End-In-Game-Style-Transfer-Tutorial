using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamCapture : MonoBehaviour
{
    // Start is called before the first frame update
    // create a dropdown and select webcam input 

    [SerializeField] string _deviceName = "";

    public void Start()
    {

        WebCamTexture webcamTexture = new WebCamTexture(_deviceName);

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
     
    }
}
