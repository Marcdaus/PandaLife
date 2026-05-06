using UnityEngine;
using UnityEngine.Events;

public class FarmingArea : MonoBehaviour
{
    [SerializeField] private string areaID;
    public string AreaID => areaID;
    [SerializeField]private GameObject objecttospawn; // aqui el cultivo que se va a plantar
    public Transform spawnpoint; // Lugar donde aparecera (tierra de cultivo)

    //una property que te dice si hay o no crops en el farmingArea
    public bool ThereIsSomething { get; private set; }

    [SerializeField] private Renderer rend;
    [SerializeField] private Material dryMaterial;
    [SerializeField] private Material wateredMaterial;

    public void SetWatered(bool watered)
    {
        if (rend == null) return;

        rend.material = watered ? wateredMaterial : dryMaterial;
    }

    private Crop currentCrop;
    public void SetCropPrefab(GameObject prefab)
    {
        objecttospawn = prefab;
    }

    void Start()
    {
        if (FarmDataManager.instance == null || CropDatabase.instance == null) return;

        if (FarmDataManager.instance.farmData.TryGetValue(areaID, out CropSaveData savedData))
        {
            if (savedData.isPlanted)
            {
                GameObject prefabCorrecto = CropDatabase.instance.GetCropPrefabByID(savedData.cropType);
                if (prefabCorrecto != null)
                {
                    objecttospawn = prefabCorrecto;
                    SpawnObject();
                    if (currentCrop != null)
                    {
                        currentCrop.LoadSavedState(savedData.growthStage, savedData.isWatered);
                        ThereIsSomething = true;
                    }
                }
            }
        }
    }
    //funcion para plantar  --------------------------------------------
    public void sowing()
    {
        if (!ThereIsSomething)
        {
            SpawnObject();
            if (currentCrop != null)
            {
                ThereIsSomething = true;
                
                CropSaveData newData = new CropSaveData
                {
                    isPlanted = true,
                    growthStage = currentCrop.growthstage,
                    cropType = currentCrop.type, 
                    isWatered = currentCrop.IsWatered
                };
                FarmDataManager.instance.SaveArea(areaID, newData);
                
            }
        }
    }

    //funcion para que aparezca el bambu  --------------------------------------------

    void SpawnObject()
    {
        if (objecttospawn != null)
        {
            GameObject cropObj = Instantiate(objecttospawn, spawnpoint.position, spawnpoint.rotation);
            cropObj.transform.SetParent(transform);
            currentCrop = cropObj.GetComponent<Crop>();
            currentCrop.SetAreaID(this.areaID);
            currentCrop.SetFarmingArea(this);
            // Importante: Si el cultivo tiene un script Harvest, actualizarle la referencia
            if (cropObj.TryGetComponent(out Harvest h))
            {
                h.area = this;
            }
        }

    }
    public void VaciarParcela()
    {
        ThereIsSomething = false;

        
        if (FarmDataManager.instance != null)
        {
            CropSaveData emptyData = new CropSaveData { isPlanted = false };
            FarmDataManager.instance.SaveArea(areaID, emptyData);
        }

        Debug.Log($"Parcela {areaID} ahora est� vac�a.");
    }

}
