using UnityEngine;
using UnityEngine.Events;

public class FarmingArea : MonoBehaviour
{

    [SerializeField]private GameObject objecttospawn; // aqui el cultivo que se va a plantar
    public Transform spawnpoint; // Lugar donde aparecera (tierra de cultivo)
    private bool thereissomething;

    //una property que te dice si hay o no crops en el farmingArea
    public bool ThereIsSomething
    {
        get { return thereissomething; }
        set { thereissomething = value; }
    }

    //funcion para plantar  --------------------------------------------
    public void sowing()
    {
        if (!thereissomething) //si no hay nada
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
        if (objecttospawn != null)
        {
            GameObject crop = Instantiate(objecttospawn, spawnpoint.position, spawnpoint.rotation);
            crop.transform.SetParent(transform);
        }
    
    }
}
