using System.Collections.Generic;
using UnityEngine;

public class FarmDataManager : MonoBehaviour
{
    public static FarmDataManager instance;

    // Diccionario: ID de la parcela -> Datos del cultivo
    public Dictionary<string, CropSaveData> farmData = new Dictionary<string, CropSaveData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   
    public void SaveArea(string areaID, CropSaveData data)
    {
        farmData[areaID] = data;
    }
}