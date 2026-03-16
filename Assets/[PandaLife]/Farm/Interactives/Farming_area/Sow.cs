using UnityEngine;

public class Sow : Interactuable
{
    private FarmingArea area;
    public GameObject cropprefab;

    void Awake()
    {
        area = GetComponent<FarmingArea>();
    }

    public override void Interactuar()
    {
        if (area == null) return;

        if (!area.ThereIsSomething)
        {

            GameObject crop = Instantiate(cropprefab, area.spawnpoint.position, Quaternion.identity);

            //poner el Crop como hijo de la parcela
            crop.transform.SetParent(area.transform);

            area.ThereIsSomething = true; // marca terreno ocupado

            // asignar el FarmingArea a la planta recién creada
            crop.GetComponent<Harvest>().area = area;

            area.sowing();
            area.ThereIsSomething = true; 
            Debug.Log("Sembrado correctamente");
        }
    }
}