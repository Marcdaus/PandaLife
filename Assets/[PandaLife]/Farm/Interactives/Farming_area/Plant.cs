using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactuable
{
    private FarmingArea area;
    [SerializeField] private List<GameObject> cropslist = new List<GameObject>();
    [SerializeField] private List<GameObject> childrenlist = new List<GameObject>();

    [SerializeField] private GameObject handpoint;
  

    void Awake()
    {
        area = GetComponent<FarmingArea>();
    }

    public override void Interactuar()
    {
        if (area == null) return;

        if (!area.ThereIsSomething)
        {

            GameObject cropselected = ChangePlant();
            if (cropselected == null)
            {
                Debug.Log("No tienes un saco para sembrar");
                return;
            }
            GameObject crop = Instantiate(cropselected, area.spawnpoint.position, Quaternion.identity);
            crop.transform.SetParent(area.transform);
            area.ThereIsSomething = true;
            crop.GetComponent<Harvest>().area = area;

            area.sowing();
            area.ThereIsSomething = true; 
            Debug.Log("Sembrado correctamente");
        }
    }
    private GameObject ChangePlant()
    {
        GameObject crop = null;
        for (int i = 0; i < childrenlist.Count;i++ )
        {
            if (handpoint.transform.Find(childrenlist[i].name))
            {
                crop = cropslist[i];
                break;
            }
        }
        return crop;
    }
}