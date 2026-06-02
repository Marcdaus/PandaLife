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

    [Header("Smoke")]
    [SerializeField] private ParticleSystem smokeparticles;
    [SerializeField] private float normalsmokesize;  // Tamaño del humo en reposo
    [SerializeField] private float cookingsmokesize;// Tamaño del humo al cocinar

    [Header("Bubbles")]
    [SerializeField] private ParticleSystem bubbleparticles;
    [SerializeField] private float normalbubbleemission;  // Emisión de las burbujas en reposo
    [SerializeField] private float cookingbubbleemission; // Emisión de las burbujas al cocinar

    [Header("Water Shader")]
    [SerializeField] private Renderer waterRenderer;
    private string colorPropertyName = "_MainColor";
    [SerializeField] private Color defaultWater;

    [Header("Sonidos")]
    [SerializeField] private AudioSource openmenu;
    public AudioSource hoversound;
    public AudioSource pressedsound;

    private void Start()
    {
        openmenu = GetComponent<AudioSource>();
        panelcauldron.SetActive(true);
        panelcooking.SetActive(false);

        worldpanelbar.SetActive(false);
        panelcauldron.SetActive(false);

        ResetSmokeSize();
        ResetBubbleEmission();

        StartCoroutine(RestoreOnStart());
    }

    IEnumerator RestoreOnStart()
    {
        yield return null;

        var mgr = CauldronPersistenceManager.instance;
        if (mgr == null) yield break;

        if (mgr.isCooking)
        {
            float elapsed = mgr.GetElapsedCookingTime();
            float total = mgr.cookingRecipe.tiempopreparacion;
            RecipesData receta = mgr.cookingRecipe;
            mgr.ClearCookingState();

            if (elapsed >= total)
            {
                FinishCookingInstant(receta);
                // No llamar RestoreFromPersistence, FinishCookingInstant ya spawnea el plato
            }
            else
            {
                StartCoroutine(Cooking(receta, elapsed));
            }
        }
        else
        {
            foreach (RecipeCard tarjeta in tarjetas)
                tarjeta.CheckUnblock();
        }

        if (mgr.hasPendingDish)
            cauldron.RestoreFromPersistence();
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
        //cursorManager.cursorblock = true;
        //cursorManager.MostrarCursor();
        openmenu.Play();

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
        //cursorManager.cursorblock = false;
        //cursorManager.OcultarCursor();
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
        GameManager.instance.sumarBambu(-recipe.bamburojo, 3);
        GameManager.instance.sumarBambu(-recipe.arandano, 2);
        GameManager.instance.sumarBambu(-recipe.bayauchuva, 4);

        SetWaterColor(recipe.colorAgua);
        StartCoroutine(Cooking(recipe, 0f));
    }

    IEnumerator Cooking(RecipesData receta, float tiempoinicial)
    {
        cooking = true;
        panelcooking.SetActive(true);
        worldpanelbar.SetActive(true);

        float tiempoTranscurrido = tiempoinicial;
        float tiempoTotal = receta.tiempopreparacion;

        // Guardar en el manager desde el principio con el tiempo ya transcurrido
        CauldronPersistenceManager.instance?.SaveCookingState(receta, tiempoTranscurrido);

        progressbar.value = tiempoTranscurrido / tiempoTotal;
        worldprogressbar.value = tiempoTranscurrido / tiempoTotal;
        cookingtext.text = "Cocinando " + receta.nombrereceta + "...";

        // Bloquear todas las tarjetas
        foreach (RecipeCard card in tarjetas) card.Block();

        var mainModule = smokeparticles.main;
        var bubbleEmission = bubbleparticles.emission;

        while (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progreso = tiempoTranscurrido / tiempoTotal;
            progressbar.value = progreso;
            worldprogressbar.value = progreso;

            Color colorActual = Color.Lerp(defaultWater, receta.colorAgua, progreso);
            SetWaterColor(colorActual);

            if (smokeparticles != null)
            {
                mainModule.startSize = Mathf.Lerp(normalsmokesize, cookingsmokesize, progreso);
            }

            if (bubbleparticles != null)
            {
                bubbleEmission.rateOverTime = Mathf.Lerp(normalbubbleemission, cookingbubbleemission, progreso);
            }

            yield return null;
        }

        progressbar.value = 1f;
        cookingtext.text = "¡" + receta.nombrereceta + " listo!";
        cooking = false;
        CauldronPersistenceManager.instance?.ClearCookingState();

        ResetSmokeSize();
        ResetBubbleEmission();

        // Spawn del plato
        if (receta.prefabResultado != null) {
            cauldron.SpawnDish(receta.prefabResultado, receta, panelcauldron.activeSelf);
        }

        foreach (RecipeCard tarjeta in tarjetas) tarjeta.CheckUnblock();
        worldpanelbar.SetActive(false);

        //desvanecer suavemete el color del agua de vuelta al default
        float tiempoDesvanecer = 0f;
        float duracionDesvanecer = 2f; // Coincide con el tiempo del WaitForSeconds anterior
        Color colorFinalCoccion = receta.colorAgua;

        while (tiempoDesvanecer < duracionDesvanecer)
        {
            tiempoDesvanecer += Time.deltaTime;
            float progresoDesvanecer = tiempoDesvanecer / duracionDesvanecer;

            Color colorHaciaDefecto = Color.Lerp(colorFinalCoccion, defaultWater, progresoDesvanecer);
            SetWaterColor(colorHaciaDefecto);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        SetWaterColor(defaultWater);
        panelcooking.SetActive(false);
    }

    private void FinishCookingInstant(RecipesData receta)
    {
        // Terminó mientras estábamos fuera: spawnear plato directamente en el suelo del caldero
        if (receta.prefabResultado != null)
            cauldron.SpawnDish(receta.prefabResultado, receta, menuabierto: false);

        foreach (RecipeCard t in tarjetas) t.CheckUnblock();

        ResetSmokeSize();
        ResetBubbleEmission();
        SetWaterColor(defaultWater);
    }

    private void ResetSmokeSize()
    {
        if (smokeparticles != null)
        {
            var mainModule = smokeparticles.main;
            mainModule.startSize = normalsmokesize;
        }
    }

    private void ResetBubbleEmission()
    {
        if (bubbleparticles != null)
        {
            var bubbleEmission = bubbleparticles.emission;

            bubbleEmission.rateOverTime = normalbubbleemission;
        }
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

    private void SetWaterColor(Color color)
    {
        if (waterRenderer != null)
            waterRenderer.material.SetColor(colorPropertyName, color);
    }
}
