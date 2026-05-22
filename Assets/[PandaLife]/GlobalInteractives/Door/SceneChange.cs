using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : Interactuable
{
    public LoadScene LoadScene;
    // Campos
    [SerializeField] private GameString scenename; // Variable que contendrá el nombre de la escena a cargar
    private Player player;
    // Función donde se encuentran los objetos
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        LoadScene = FindFirstObjectByType<LoadScene>();
        //pickupobject = FindFirstObjectByType<PickupDrop>();
    }
    // Función interactuar que comprueba si tiene el cubo o un plato en la mano.
    public override void Interactuar()
    {
        bool limpiarAntes = true; // Por defecto limpiamos antes de guardar

        if (player.IsHoldingBucket() || player.IsHoldingDish())
        {
            PickupDrop pickupobject = player.pickedobject;
            if (pickupobject != null && player.IsHoldingDish())
            {
                // Limpiar y guardar todo desde cero
                CauldronPersistenceManager.instance.ClearAllDishStates();

                Dish dish = pickupobject.GetComponentInChildren<Dish>();
                if (dish != null && CauldronPersistenceManager.instance != null)
                {
                    CauldronPersistenceManager.instance.SaveDishState(
                        dish.GetReceta(),
                        pickupobject.transform.position,
                        inHand: false
                    );
                }
                pickupobject.Drop();
                limpiarAntes = false; // Ya hemos limpiado los platos arriba, no lo hacemos de nuevo
            }
        }

        // Completar el tutorial de puerta y no mostrar más el pin
        if (!GameManager.instance.tutorialPuertaCompletado)
        {
            GameManager.instance.tutorialPuertaCompletado = true;
            Debug.Log("Tutorial de la puerta completado para esta partida.");
        }

        // SIEMPRE llamamos a la corrutina para asegurar que la animación se reproduce
        StartCoroutine(EsperarParaCargar(limpiarAntes));
    }


    IEnumerator EsperarParaCargar(bool limpiarAntes)
    {
        // Iniciamos la animación de transición
        if (LoadScene != null)
        {
            LoadScene.StartLoadScene();
        }

        // Guardamos la persistencia
        GuardarPlatoSueltoSiExiste(limpiarAntes);

        // Esperamos a que la animación termine
        yield return new WaitForSeconds(1f);

        // Cargamos la escena
        SceneManager.LoadScene(scenename.Value);

        
    }

    private void GuardarPlatoSueltoSiExiste(bool limpiarAntes)
    {
        if (CauldronPersistenceManager.instance == null) return;

        Dish[] platos = FindObjectsByType<Dish>(FindObjectsSortMode.None);
        Cauldron cauldron = FindFirstObjectByType<Cauldron>();


        List<Dish> platosSueltos = new List<Dish>();
        foreach (Dish dish in platos)
        {
            GameObject raiz = dish.transform.root.gameObject;
            if (player.pickedobject != null &&
                raiz == player.pickedobject.transform.root.gameObject) continue;

            platosSueltos.Add(dish);
        }

        if (platosSueltos.Count == 0) return;

        if (limpiarAntes)
            CauldronPersistenceManager.instance.ClearAllDishStates();


        foreach (Dish dish in platosSueltos)
        {
            GameObject raiz = dish.transform.root.gameObject;


            bool esPlatoDelCaldero = false;
            if (cauldron != null && cauldron.PlatoPendienteGameObject != null)
            {

                if (raiz == cauldron.PlatoPendienteGameObject.transform.root.gameObject)
                {
                    esPlatoDelCaldero = true;
                }
            }

            Debug.Log($"[SceneChange] Guardando plato: {raiz.name}. ¿Es del caldero?: {esPlatoDelCaldero}");

            CauldronPersistenceManager.instance.SaveDishState(
                dish.GetReceta(),
                raiz.transform.position,
                inHand: esPlatoDelCaldero
            );
        }
    }
    //ahora funciona con la E
    /*
    private void OnTriggerEnter(Collider other)
    {
        // Si es el player llama a interactuar
        if (other.CompareTag("Player"))
        {
            Interactuar();
        }
    }
    */
}