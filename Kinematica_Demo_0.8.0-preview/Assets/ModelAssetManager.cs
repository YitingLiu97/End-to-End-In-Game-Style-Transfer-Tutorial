using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Barracuda;
using UnityEngine;

public class ModelAssetManager : MonoBehaviour
{
    [System.Serializable]
    public class ModelData {
        public string modelOneName;
        public string modelTwoName;
 
    }

    public ModelData modelData = new ModelData();

    public StyleTransfer styleTransferOne;
    public StyleTransfer styleTransferTwo;

    public string MODEL_PATH;
    // Start is called before the first frame update
    void Awake()
    {
        MODEL_PATH = Path.Combine(Application.streamingAssetsPath, "Models");
        MODEL_PATH=MODEL_PATH.Replace(@"\", "/");
        Debug.Log($"Model path is {MODEL_PATH}");
        ReadFromJson();
        SaveToJson();
    }

    // Update is called once per frame
  

    void SaveToJson() {
        if (!File.Exists(MODEL_PATH))
        {

            File.Create(MODEL_PATH);

        }
        else { 
        string json = JsonUtility.ToJson(modelData);
        File.WriteAllText(MODEL_PATH, json);
        }
    }

    void ReadFromJson() {
        if (!File.Exists(MODEL_PATH))
        {

            File.Create(MODEL_PATH);

        }
        else
        {
            string json = File.ReadAllText(MODEL_PATH);
            Debug.Log("read from json"+json);

            ModelData md = JsonUtility.FromJson<ModelData>(json);
            md.modelOneName = styleTransferOne.modelFileNameInStreamingAsset;
            md.modelTwoName = styleTransferTwo.modelFileNameInStreamingAsset;


        }
       
    }
}
