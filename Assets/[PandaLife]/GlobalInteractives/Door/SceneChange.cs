using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Interactuable
{
    public LoadScene LoadScene;
    [SerializeField] private GameString scenename;
    private AudioSource opendoor;

    private void Start()
    {
        opendoor = GetComponent<AudioSource>();
        LoadScene = FindFirstObjectByType<LoadScene>();
    }

    public override void Interactuar(Player player)
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

        // Completar paso del tutorial
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.SalirDeCasa);
        }

        StartCoroutine(EsperarParaCargar(limpiarAntes, player));
    }


    IEnumerator EsperarParaCargar(bool limpiarAntes, Player player)
    {
        // Iniciamos la animación de transición
        if (LoadScene != null)
        {
            LoadScene.StartLoadScene();
        }

        // Guardamos la persistencia, pasándole el player
        GuardarPlatoSueltoSiExiste(limpiarAntes, player);

        // Esperamos a que la animación termine
        if (opendoor != null) opendoor.Play();
        yield return new WaitForSeconds(1f);

        // Cargamos la escena
        SceneManager.LoadScene(scenename.Value);
    }

    // La función de guardado usa el Player recibido para hacer las comprobaciones
    private void GuardarPlatoSueltoSiExiste(bool limpiarAntes, Player player)
    {
        if (CauldronPersistenceManager.instance == null) return;

        Dish[] platos = FindObjectsByType<Dish>(FindObjectsSortMode.None);
        Cauldron cauldron = FindFirstObjectByType<Cauldron>();

        List<Dish> platosSueltos = new List<Dish>();
        foreach (Dish dish in platos)
        {
            GameObject raiz = dish.transform.root.gameObject;

            // Usamos el player para ver qué tiene en la mano
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
}