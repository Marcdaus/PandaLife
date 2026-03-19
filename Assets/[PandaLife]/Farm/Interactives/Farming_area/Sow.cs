using UnityEngine;

public class Sow : Interactuable
{
    private FarmingArea area;
    public GameObject cropbamboo;
    public GameObject cropreddragon;
    public GameObject cropblue_Berry;
    public GameObject cropuchuva;
    // Declaramos el enum
    private enum Cultivo { bamboo, reddragon, blueberry, uchuva }

    // Esta es la variable que aparecerá en el Inspector
    [SerializeField] private Cultivo cultivoSeleccionado;

    void Awake()
    {
        area = GetComponent<FarmingArea>();
    }

    public override void Interactuar()
    {
        if (area == null) return;

        if (!area.ThereIsSomething)
        {
            if (cultivoSeleccionado == Cultivo.bamboo)
            {
                GameObject crop = Instantiate(cropbamboo, area.spawnpoint.position, Quaternion.identity);
                crop.transform.SetParent(area.transform);
                area.ThereIsSomething = true;
                crop.GetComponent<Harvest>().area = area;
            }
            else if (cultivoSeleccionado == Cultivo.reddragon)
            {
                GameObject crop = Instantiate(cropreddragon, area.spawnpoint.position, Quaternion.identity);
                crop.transform.SetParent(area.transform);
                area.ThereIsSomething = true;
                crop.GetComponent<Harvest>().area = area;
            }
            else if (cultivoSeleccionado == Cultivo.blueberry)
            {
                GameObject crop = Instantiate(cropblue_Berry, area.spawnpoint.position, Quaternion.identity);
                crop.transform.SetParent(area.transform);
                area.ThereIsSomething = true;
                crop.GetComponent<Harvest>().area = area;
            }
            else if (cultivoSeleccionado == Cultivo.uchuva)
            {
                GameObject crop = Instantiate(cropuchuva, area.spawnpoint.position, Quaternion.identity);
                crop.transform.SetParent(area.transform);
                area.ThereIsSomething = true;
                crop.GetComponent<Harvest>().area = area;
            }

            area.sowing();
            area.ThereIsSomething = true; 
            Debug.Log("Sembrado correctamente");
        }
    }
}