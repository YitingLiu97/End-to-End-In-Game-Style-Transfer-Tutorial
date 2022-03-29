using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MediaAssetManager : MonoBehaviour
{
    [SerializeField] List<string> videoExtensions, imageExtensions;
    [SerializeField] string imageFolder = "/Images";
    [SerializeField] string styleImageFolder = "/StylizedImages";
    [SerializeField] RectTransform stylizedImageRect;
    [SerializeField] RawImage styleizedImage;
    public GameObject quad;
    public List<string> loadedMedia;
    int index;
    [SerializeField] Camera styleDesign;
    [SerializeField] Camera stylePencil;
    [SerializeField] Canvas styleImageCanvas;
    [SerializeField] bool styleDesignCamOn = true;
    [SerializeField] bool stylePencilCamOn = false;

    void Start()
    {

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



    void ResizeImageToScreen(int i)
    {
        Vector2 size = ResizeImportedImage(i);
        stylizedImageRect.sizeDelta = size;
        Debug.Log($" changed size is {size}");

    }


    Vector2 ResizeImportedImage(int i)
    {
        Texture2D tex = ImageToTexture(i);
        float ratio = tex.width / tex.height;
        float changedHeight, changedWidth;
        // tex is wider
        if (ratio >= 1)
        {
            changedHeight = Screen.height;
            changedWidth = Screen.height * tex.width / tex.height;
        }
        // tex is taller 
        else
        {
            changedWidth = Screen.width;
            changedHeight = Screen.width * tex.height / tex.width;

        }

        return new Vector2(changedWidth, changedHeight);

    }

    // using arrow keys to change the image 
    void UpdateTextureToMaterial(int i)
    {
        styleizedImage.texture = ImageToTexture(i);

        int length = loadedMedia[i].LastIndexOf(".") - loadedMedia[i].LastIndexOf(@"\");
        string imageName = loadedMedia[i].Substring(loadedMedia[i].LastIndexOf(@"\"), length);
        styleizedImage.texture.name = imageName;

    }

    void UpdateTextureSizeToScreenSize(int width, int height, bool fullScreen = false)
    {
        Screen.SetResolution(width, height, fullScreen);
    }

    IEnumerator SaveScreenCapture(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        DirectoryInfo stylizedImageFolderPath = new DirectoryInfo(Application.streamingAssetsPath + styleImageFolder);


        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = ImageConversion.EncodeArrayToJPG(tex.GetRawTextureData(), tex.graphicsFormat, (uint)width, (uint)height);
        Object.Destroy(tex);

        if (stylePencilCamOn)
        {
            File.WriteAllBytes(stylizedImageFolderPath + "/" + styleizedImage.texture.name + "Pencil_stylized" + ".jpg", bytes);

        }
        if (styleDesignCamOn)
        {


            File.WriteAllBytes(stylizedImageFolderPath + "/" + styleizedImage.texture.name + "Design_stylized" + ".jpg", bytes);

        }


    }


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

        styleImageCanvas.renderMode = RenderMode.ScreenSpaceCamera;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            styleDesignCamOn = true;
            stylePencilCamOn = false;
            styleImageCanvas.worldCamera = styleDesign;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            stylePencilCamOn = true;
            styleDesignCamOn = false;
            styleImageCanvas.worldCamera = stylePencil;


        }

        styleDesign.enabled = styleDesignCamOn;
        stylePencil.enabled = stylePencilCamOn;

        UpdateTextureToMaterial(index);
        // resize the imported images 
        ResizeImageToScreen(index);
        int width = (int)ResizeImportedImage(index).x;
        int height = (int)ResizeImportedImage(index).y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SaveScreenCapture(width, height));
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        styleDesign.enabled = styleDesignCamOn;
        stylePencil.enabled = stylePencilCamOn;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        styleDesign.enabled = styleDesignCamOn;
        stylePencil.enabled = stylePencilCamOn;
    }
}
