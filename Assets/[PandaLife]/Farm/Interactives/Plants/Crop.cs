using UnityEngine;
using System.Collections;
using System;
public class Crop : MonoBehaviour
{
    [Range(1, 3)] public int growthstage = 1;
    [SerializeField] private int valor = 1;

    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;
    
    [SerializeField] private float growtime = 10f;
    [SerializeField] private FarmingArea farmingArea;
    public int type;

    public int maxStages = 3;

    private bool watered = false;
    private long waterTimeTicks = 0;

    [SerializeField] private string areaID;
    public string AreaID => areaID;

    public void SetAreaID(string id)
    {
        areaID = id;
    }
   public bool IsWatered => watered;
    public int Valor => valor;

    public bool IsHarvestable()
    {
        return growthstage >= maxStages;
    }
    public void SetFarmingArea(FarmingArea area)
    {
        farmingArea = area;
    }

    // ========================
    // REGAR
    // ========================
    public void Water()
    {
        if (watered)
        {
            Debug.Log("Ya está regada");
            return;
        }

        if (growthstage >= maxStages)
        {
            Debug.Log("Ya está completamente crecida");
            return;
        }

        watered = true;
        waterTimeTicks = DateTime.Now.Ticks;

        if (farmingArea != null)
        {
            farmingArea.SetWatered(true);
        }
        UpdateSingletonData();
        StartCoroutine(Grow(growtime));
    }

    IEnumerator Grow(float timeToWait)
    {
        Debug.Log("Planta regada, creciendo...");
        yield return new WaitForSeconds(timeToWait);
        AdvanceGrowthStage();
    }

    // NUEVO: Separamos la lógica de avanzar de fase para reutilizarla
    private void AdvanceGrowthStage()
    {
        watered = false;
        waterTimeTicks = 0; // Reseteamos el tiempo

        if (farmingArea != null)
        {
            farmingArea.SetWatered(false);
        }

        if (growthstage == 1)
        {
            stage1.SetActive(false);
            stage2.SetActive(true);
            growthstage = 2;
        }
        else if (growthstage == 2 && maxStages == 3)
        {
            stage2.SetActive(false);
            if (stage3 != null) stage3.SetActive(true);
            growthstage = 3;
        }

        UpdateSingletonData();
        Debug.Log("Nueva fase: " + growthstage);
    }

    // ========================
    // COSECHAR
    // ========================
    public void Harvest()
    {
        Debug.Log("Bambú cosechado");
        GameManager.instance.sumarBambu(Valor,type);
        CropSaveData emptyData = new CropSaveData { isPlanted = false };
        FarmDataManager.instance.SaveArea(areaID, emptyData);
        Destroy(gameObject);
    }
    //  función para cargar el estado al entrar a la escena
    public void LoadSavedState(int savedStage, bool savedWatered, long savedTimeTicks)
    {
        growthstage = savedStage;
        watered = savedWatered;

        stage1.SetActive(growthstage == 1);
        stage2.SetActive(growthstage == 2);
        if (stage3 != null)stage3.SetActive(growthstage == 3);

        if (farmingArea != null)
        {
            farmingArea.SetWatered(watered);
        }
        // Si estaba regado y no ha terminado de crecer, retomamos la corrutina
        if (watered && growthstage < maxStages)
        {
            long ticksPassed = DateTime.Now.Ticks - waterTimeTicks;
            float secondsPassed = (float)TimeSpan.FromTicks(ticksPassed).TotalSeconds;

            float remainingTime = growtime - secondsPassed;

            if (remainingTime <= 0)
            {
                // Si ya pasó el tiempo necesario mientras estabas en otra escena
                AdvanceGrowthStage();
            }
            else
            {
                // Si aún le falta tiempo, iniciamos la corrutina con el tiempo restante
                StartCoroutine(Grow(remainingTime));
            }
        }
    }
    private void UpdateSingletonData()
    {
        CropSaveData data = new CropSaveData
        {
            isPlanted = true,
            growthStage = growthstage,
            cropType = type,
            isWatered = watered,
            timeWateredTicks = waterTimeTicks
        };
        FarmDataManager.instance.SaveArea(areaID, data);
    }
}
