using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : BarSystem
{
    // Variables de la barra según estados
    private Color satisfiedBarColor = Color.green;
    private Color normalBarColor = Color.yellow;
    private Color hungryBarColor = Color.red;

    // Variables de las caras según estados
    [SerializeField] Color initialCircleColor = Color.white;
    private Color normalCircleColor;
    private Color hungryCircleColor;

    // Variables estado de ira
    [SerializeField] RageSystem rageSystem;
    private bool rageActivated = false;

    // Variable para pausar la barra
    private bool isPaused = false;

    // Variable que activa la ira de un panda
    private static bool cheatActivated = false;
    HungerSystem[] allPandas;

    void Start()
    {
        //Obtiene a los pandas
        allPandas = Object.FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);
        GenerateDerivedColors();
        // Inicializa valores desde el manager
        if (BarraManager.Instancia != null)
        {
            currentValue = BarraManager.Instancia.HungerCurrentValue;
            maxValue = BarraManager.Instancia.HungerMaxValue;
            changeRate = BarraManager.Instancia.HungerChangeRate;
            rageActivated = BarraManager.Instancia.RageActivated;
        }
        //Si la ira ya está activada, muestra la barra de ira
        if (rageActivated && rageSystem != null)
        {
            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, hungryCircleColor);
        }
        else
        {
            Activate();
        }
        // Actualiza la UI
        UpdateUI();
    }

    protected override void Update()
    {
     
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {
        if (rageActivated || isPaused) return;

        currentValue -= changeRate * Time.deltaTime; // Disminuye el hambre con el tiempo
        currentValue = Mathf.Clamp(currentValue, 0, maxValue); //Pone el máximo y el mínimo

        //Guarda el valor  en el manager
        if (BarraManager.Instancia != null)
            BarraManager.Instancia.HungerCurrentValue = currentValue;

        //Activa la barra de ira
        if (currentValue <= 0 && rageSystem != null)
        {
            rageActivated = true;
            if (BarraManager.Instancia != null) BarraManager.Instancia.RageActivated = true;

            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);
        }
    }

    //Funcion que actualiza los colores de las caras y barras
    protected override void UpdateColors()
    {
        float percentage = (currentValue / maxValue) * 100f;

        if (fillImage != null)
            fillImage.color = (percentage >= 80f) ? satisfiedBarColor : (percentage >= 41f) ? normalBarColor : hungryBarColor;

        if (indicatorImage != null)
        {
            Color faceColor = (percentage >= 80f) ? initialCircleColor : (percentage >= 41f) ? normalCircleColor : hungryCircleColor;
            faceColor.a = 1f;
            indicatorImage.color = faceColor;
        }
    }
    //Hace que las caras varien de color a otro tono
    void GenerateDerivedColors()
    {
        normalCircleColor = Color.Lerp(initialCircleColor, Color.yellow, 0.5f);
        hungryCircleColor = Color.Lerp(initialCircleColor, Color.red, 0.7f);
    }

    // Función para activar la barra de ira de un panda al máximo
    private void ActivateCheat()
    {
        if (!cheatActivated && rageSystem != null)
        {
            cheatActivated = true;
            rageActivated = true;

            if (BarraManager.Instancia != null)
                BarraManager.Instancia.RageActivated = true;

            // 🔹 Pausar hambre de este panda
            PauseHunger();

            // 🔹 Pausar hambre de todos los demás pandas
            
            foreach (HungerSystem panda in allPandas)
            {
                if (panda != this) // No pausar el que activó el cheat
                {
                    panda.PauseHunger();
                }
            }

            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);
        }
    }
    // Funciones para pausar/reanudar
    public void PauseHunger() { isPaused = true; }
    public void ResumeHunger() { isPaused = false; }
}