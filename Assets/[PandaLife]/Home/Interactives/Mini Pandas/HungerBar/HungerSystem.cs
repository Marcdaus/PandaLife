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

    //Propiedades
    public RageSystem Ragesystem
    {
        get => rageSystem;
        set => rageSystem = value;
    }

    public bool RageActivated
    {
        get => rageActivated;
        set => rageActivated = value;
    }

    void Start()
    {
        GenerateDerivedColors();
        // Inicializa valores desde el manager
        if (BarraManager.Instancia != null)
        {
            currentValue = BarraManager.Instancia.HungerCurrentValue;
            maxValue = BarraManager.Instancia.HungerMaxValue;
            //changeRate = BarraManager.Instancia.HungerChangeRate;
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
       Debug.Log("ira al iniciar: " + rageActivated);

    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {
        // Leer valores directamente del manager en tiempo real
        if (BarraManager.Instancia != null)
        {
            maxValue = BarraManager.Instancia.HungerMaxValue;
            //changeRate = BarraManager.Instancia.HungerChangeRate;
            rageActivated = BarraManager.Instancia.RageActivated;
        }

        // Disminuir hambre solo si no está pausada
        if (!isPaused)
        {
            currentValue -= changeRate * Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

            if (BarraManager.Instancia != null)
                BarraManager.Instancia.HungerCurrentValue = currentValue;
        }

        // Activar ira si llega a 0 o si RageActivated ya está en true
        if (rageSystem != null && (!rageActivated && currentValue <= 0 || (BarraManager.Instancia != null && BarraManager.Instancia.RageActivated)))
        {
            rageActivated = true;
            if (BarraManager.Instancia != null)
                BarraManager.Instancia.RageActivated = true;

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
}