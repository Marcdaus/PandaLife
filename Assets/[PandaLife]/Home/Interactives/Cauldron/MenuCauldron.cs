using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuCauldron : MonoBehaviour
{
    [SerializeField] private GameObject panelcauldron;
    [SerializeField] private Cauldron cauldron;

    [Header("Cooking")]
    [SerializeField] private GameObject panelcooking;
    [SerializeField] private TextMeshProUGUI cookingtext;
    [SerializeField] private Slider progressbar;

    private bool cooking = false;

    [Header("Tarjetas")]
    [SerializeField] private RecipeCard[] tarjetas;

    [Header("Barra mundo")]
    [SerializeField] private Slider worldprogressbar;
    [SerializeField] private GameObject worldpanelbar;

    private void Start()
    {
        panelcauldron.SetActive(true);
        panelcooking.SetActive(false);

        worldpanelbar.SetActive(false);
        panelcauldron.SetActive(false);

    }

    private void Update()
    {
        if (panelcauldron.activeSelf && Input.GetButtonDown("SalirMenuCaldero"))
        {
            CloseCauldron();
        }
    }

    public void OpenCauldron()
    {
        panelcauldron.SetActive(true);
        worldpanelbar.SetActive(false);
        if (!cooking)
        {
            foreach (RecipeCard tarjeta in tarjetas)
                tarjeta.CheckUnblock();
        }
    }

    public void CloseCauldron()
    {
        panelcauldron.SetActive(false);
        if (cooking) worldpanelbar.SetActive(true);
        else panelcooking.SetActive(false);
    }

    public void StartCooking(RecipesData recipe)
    {
        if (cooking) return;

        if (!HasIngredients(recipe))
        {
            cookingtext.text = "No tienes ingredientes suficientes.";
            panelcooking.SetActive(true);
            return;
        }

        // Consumir ingredientes
        GameManager.instance.sumarBambu(-recipe.bambuverde, 1);
        GameManager.instance.sumarBambu(-recipe.bamburojo, 2);
        GameManager.instance.sumarBambu(-recipe.arandano, 3);
        GameManager.instance.sumarBambu(-recipe.bayauchuva, 4);

        StartCoroutine(Cooking(recipe));
    }

    IEnumerator Cooking(RecipesData receta)
    {
        cooking = true;
        panelcooking.SetActive(true);
        worldpanelbar.SetActive(true); 

        progressbar.value = 0f;
        worldprogressbar.value = 0f; 

        cookingtext.text = "Cocinando " + receta.nombrereceta + "...";

        // Bloquear todas las tarjetas
        foreach (RecipeCard card in tarjetas)
            card.Block();

        float tiempoTranscurrido = 0f;
        float tiempoTotal = receta.tiempopreparacion;

        while (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;
            progressbar.value = tiempoTranscurrido / tiempoTotal;
            worldprogressbar.value = tiempoTranscurrido / tiempoTotal;
            yield return null;
        }

        progressbar.value = 1f;
        cookingtext.text = "¡" + receta.nombrereceta + " listo!";
        cooking = false;

        // Spawn del plato
        if (receta.prefabResultado != null) {
            cauldron.SpawnDish(receta.prefabResultado, panelcauldron.activeSelf);
    }
        foreach (RecipeCard tarjeta in tarjetas)
            tarjeta.CheckUnblock();

        yield return new WaitForSeconds(2f);
        panelcooking.SetActive(false);
        worldpanelbar.SetActive(false);

    }

    public bool HasIngredients(RecipesData recipe)
    {
        GameManager gm = GameManager.instance;
        return gm.bambuverde >= recipe.bambuverde
            && gm.bamburojo >= recipe.bamburojo
            && gm.bayaarandanos >= recipe.arandano
            && gm.bayauchuva >= recipe.bayauchuva;
    }

    public void RefreshCards()
    {
        foreach (RecipeCard tarjeta in tarjetas)
            tarjeta.CheckUnblock();
    }
}
