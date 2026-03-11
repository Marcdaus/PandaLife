using UnityEngine;
using UnityEngine.Events;

public class FarmingArea : MonoBehaviour
{

    [SerializeField]private GameObject objectToSpawn; // aqui el cultivo que se va a plantar
    [SerializeField]private Transform spawnPoint; // Lugar donde aparecera (tierra de cultivo)
    private bool thereIsSomething;

    //una property que te dice si hay o no crops en el farmingArea
    public bool ThereIsSomething
    {
        get { return thereIsSomething; }
        set { thereIsSomething = value; }
    }

    //funcion para plantar  --------------------------------------------
    public void sowing()
    {
        if (!thereIsSomething) //si no hay nada
        {
            Debug.Log("plantaste un bambu, yei"); //plantas bambu
            SpawnObject();
            ThereIsSomething = true;
        }
        else
        {
            Debug.Log("ya tienes algo plantado friendo");
        }       
    }

    //funcion para que aparezca el bambu  --------------------------------------------

    void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            GameObject crop = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
            crop.transform.SetParent(transform);
        }
    
    }
}
