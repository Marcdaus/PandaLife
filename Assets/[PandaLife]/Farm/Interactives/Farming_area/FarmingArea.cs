using UnityEngine;
using UnityEngine.Events;

public class FarmingArea : MonoBehaviour
{

        [SerializeField]private GameObject objectToSpawn; // Arrastra aquí tu prefab
        [SerializeField]private Transform spawnPoint; // Lugar donde aparecerá
       [SerializeField]private bool thereIsSomething;

        public bool ThereIsSomething
        {
            get { return thereIsSomething; }
            set { thereIsSomething = value; }
        }

        public void sowing()
        {
            if (!thereIsSomething)
            {
                Debug.Log("plantaste un bambu, yei");
                SpawnObject();
                ThereIsSomething = true;
            }
            else
        {
            Debug.Log("ya tienes algo plantado friendo");
        }
            
        }
         void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            GameObject crop = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
            crop.transform.SetParent(transform);
        }
    
    }
}



/*
    [SerializeField]private GameObject objectToSpawn; // Arrastra aquí tu prefab
    [SerializeField]private Transform spawnPoint; // Lugar donde aparecerá
    public KeyCode interactionKey = KeyCode.E;

   private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(interactionKey))
        {
            SpawnObject();
            Debug.Log("se supone que el objeto esta");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<movement>(out movement player))
        {
            playerInside = true;
            Debug.Log("el player esta dentro");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<movement>(out movement player))
        {
            playerInside = false;
            Debug.Log("el player esta fuera del trozo de parcela");
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }*/

