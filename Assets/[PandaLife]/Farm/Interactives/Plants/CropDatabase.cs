using System.Collections.Generic;
using UnityEngine;

public class CropDatabase : MonoBehaviour
{
    public static CropDatabase instance;

    [Header("Lista de todos los prefabs de cultivos")]
    public List<GameObject> allCropPrefabs = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

   
    public GameObject GetCropPrefabByID(int id)
    {
        foreach (GameObject prefab in allCropPrefabs)
        {
            if (prefab.GetComponent<Crop>().type == id)
            {
                return prefab;
            }
        }
        return null;
    }
}