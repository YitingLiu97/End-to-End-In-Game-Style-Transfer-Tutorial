using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MediaAssetManager : MonoBehaviour
{
    [SerializeField] List<string> videoExtensions, imageExtensions;
    [SerializeField] string imageFolder = "/Images";
    [SerializeField] string styleImageFolder = "/StylizedImages";
    [SerializeField] Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;
    public GameObject quad;
    public List<string> loadedMedia;
    int index;
    float fileCounter;
    [SerializeField] Camera styleDesign;
    [SerializeField] Camera stylePencil;
    [SerializeField] bool styleDesignCamOn = true;
    [SerializeField] bool stylePencilCamOn = false;

    void Start()
    {
        m_Renderer = quad.GetComponent<Renderer>();
        fileCounter = 0;

        styleDesign.enabled = styleDesignCamOn;
        stylePencil.enabled = stylePencilCamOn;

        LoadAllMedia();
        Debug.Log("loaded media is " + loadedMedia.Count);
    }


    void LoadAllMedia()
    {

        DirectoryInfo imageFolderPath = new DirectoryInfo(Application.streamingAssetsPath + imageFolder);
        FileInfo[] fileinfo = imageFolderPath.GetFiles("*.*");

        foreach (FileInfo info in fileinfo)
        {

            if (imageExtensions.Contains(info.Extension))//removing dot just to be safe
            {
                int endIndex = info.Name.IndexOf('.');
                string fileName = info.Name;
                fileName = fileName.Substring(0, endIndex);

                if (!MediaExists(fileName))
                {
                    loadedMedia.Add(info.FullName);
                }
                else
                {
                    Debug.Log("file exists");

                }

            }
        }

    }

    bool MediaExists(string _fileName)
    {
        for (int i = 0; i < loadedMedia.Count; i++)
        {
            if (loadedMedia[i].Contains(_fileName))
            {
                return true;
            }
        }

        return false;
    }


    Texture2D ImageToTexture(int index)
    {
        Texture2D tex = new Texture2D(2, 2);
        byte[] imageBytes = System.IO.File.ReadAllBytes(loadedMedia[index]);
        tex.LoadImage(imageBytes);
        return tex;
    }
    // using arrow keys to change the image 
    void UpdateTextureToMaterial(int i)
    {
        //Make sure to enable the Keywords
        m_Renderer.material.EnableKeyword("_NORMALMAP");
        m_Renderer.material.EnableKeyword("_METALLICGLOSSMAP");
        m_MainTexture = ImageToTexture(i);

        int length = loadedMedia[i].LastIndexOf(".") - loadedMedia[i].LastIndexOf(@"\");
        string imageName = loadedMedia[i].Substring(loadedMedia[i].LastIndexOf(@"\"), length);
        m_MainTexture.name = imageName;

        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);

        UpdateTextureSizeToScreenSize(ImageToTexture(i).width, ImageToTexture(i).height, false);

        Debug.Log($"texture for image {m_MainTexture.name} is {ImageToTexture(i).width} and {ImageToTexture(i).height} ");

    }

    void UpdateTextureSizeToScreenSize(int width, int height, bool fullScreen = false)
    {
        Screen.SetResolution(width, height, fullScreen);
    }

    IEnumerator SaveScreenCapture()
    {
        yield return new WaitForEndOfFrame();
        DirectoryInfo stylizedImageFolderPath = new DirectoryInfo(Application.streamingAssetsPath + styleImageFolder);

        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = ImageConversion.EncodeArrayToJPG(tex.GetRawTextureData(), tex.graphicsFormat, (uint)width, (uint)height);
        Object.Destroy(tex);

        if (stylePencilCamOn)
        {
            File.WriteAllBytes(stylizedImageFolderPath + "/" + m_MainTexture.name + "Pencil_stylized" + ".jpg", bytes);

        }
        if (styleDesignCamOn)
        {


            File.WriteAllBytes(stylizedImageFolderPath + "/" + m_MainTexture.name + "Design_stylized" + ".jpg", bytes);

        }


    }


    /* IEnumerator SaveImagesRelativeSize()
     {
         yield return new WaitForEndOfFrame();
         DirectoryInfo stylizedImageFolderPath = new DirectoryInfo(Application.streamingAssetsPath + styleImageFolder);
         FileInfo[] fileinfo = stylizedImageFolderPath.GetFiles("*.*");


         int width = Screen.width;
         int height = Screen.height;
         Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
         tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
         tex.Apply();

         byte[] bytes = ImageConversion.EncodeArrayToJPG(tex.GetRawTextureData(), tex.graphicsFormat, (uint)width, (uint)height);
         Object.Destroy(tex);

         File.WriteAllBytes(stylizedImageFolderPath + "/" + m_MainTexture.name + "stylized" + ".jpg", bytes);


     }*/



    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            index++;

            if (index > loadedMedia.Count - 1)
            {
                index = 0;

            }



        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;

            if (index < 0)
            {
                index = loadedMedia.Count - 1;

            }

        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            styleDesignCamOn = true;
            stylePencilCamOn = false;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            stylePencilCamOn = true;
            styleDesignCamOn = false;

        }

        styleDesign.enabled = styleDesignCamOn;
        stylePencil.enabled = stylePencilCamOn;

        UpdateTextureToMaterial(index);






        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SaveScreenCapture());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
