using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MediaAssetManager : MonoBehaviour
{
    public List<string> videoExtensions, imageExtensions;
    public string imageFolder = "/Images";
    public string styleImageFolder = "/StylizedImages";
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;
    public GameObject quad;
    public List<string> loadedMedia;
    float fileCounter;
    public int index;
    void Start()
    {
        m_Renderer = quad.GetComponent<Renderer>();
        fileCounter = 0;

        LoadAllMedia();
        Debug.Log("loaded media is " + loadedMedia.Count);
    }


    void LoadAllMedia()
    {

        DirectoryInfo imageFolderPath = new DirectoryInfo(Application.streamingAssetsPath + imageFolder);
        FileInfo[] fileinfo = imageFolderPath.GetFiles("*.*");

        Debug.Log("file info is " + fileinfo[0]);
        Debug.Log("file info is " + fileinfo[1]);

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
            Debug.Log("loadedmedia count is " + loadedMedia.Count);
            Debug.Log("loadedMedia " + i + loadedMedia[i] + "file name is " + _fileName);

            if (loadedMedia[i].Contains(_fileName))
            {
                return true;
            }
        }

        return false;
    }

    Texture2D LoadAllImages(int index)
    {

        Texture2D tex = new Texture2D(2, 2);
        byte[] imageBytes = System.IO.File.ReadAllBytes(loadedMedia[index]);

        //Creates texture and loads byte array data to create image
        tex.LoadImage(imageBytes);

        return tex;


    }
    // using arrow keys to change the image 
    void UpdateTextureToMaterial(int i)
    {
        //Make sure to enable the Keywords
        m_Renderer.material.EnableKeyword("_NORMALMAP");
        m_Renderer.material.EnableKeyword("_METALLICGLOSSMAP");

        /*for (int i = 0; i < loadedMedia.Count; i++)
        {*/
        m_MainTexture = LoadAllImages(i);
        int length = loadedMedia[i].LastIndexOf(".") - loadedMedia[i].LastIndexOf(@"\");
        string imageName = loadedMedia[i].Substring(loadedMedia[i].LastIndexOf(@"\"), length);
        m_MainTexture.name = imageName;
        Debug.Log("image name is " + imageName);
        /* }*/
        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);

    }

    // on space bar click // or wait after the load is complete 
    IEnumerator SaveScreenCapture()
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


    }





    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            index++;

            if (index > loadedMedia.Count-1) {
                index = 0;

            }
         
            

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;

            if (index < 0 )
            {
                index = loadedMedia.Count-1;

            }
            
        }

        UpdateTextureToMaterial(index);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SaveScreenCapture());
        }
    }
}
