using UnityEngine;
using UnityEngine.Events;

public class FarmingArea : MonoBehaviour
{
    [SerializeField] public string areaID;
    [SerializeField]private GameObject objecttospawn; // aqui el cultivo que se va a plantar
    public Transform spawnpoint; // Lugar donde aparecera (tierra de cultivo)
    private bool thereissomething;
    //una property que te dice si hay o no crops en el farmingArea
    public bool ThereIsSomething { get { return thereissomething; } set { thereissomething = value; } }

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
            currentCrop.areaID = this.areaID;

            // Importante: Si el cultivo tiene un script Harvest, actualizarle la referencia
            if (cropObj.TryGetComponent(out Harvest h))
            {
                h.area = this;
            }
        }

    }
}
