using UnityEngine;

public class HungerSystem : BarSystem
{
    [Header("Colors")]
    private Color satisfiedBarColor = Color.green;
    private Color normalBarColor = Color.yellow;
    private Color hungryBarColor = Color.red;

    [SerializeField] Color initialCircleColor = Color.white;
    private Color normalCircleColor;
    private Color hungryCircleColor;

    [SerializeField] RageSystem rageSystem;
    private bool rageActivated = false;

    void Start()
    {
        GenerateDerivedColors();

        // 1. Cargar datos del Manager
        if (BarraManager.Instancia != null)
        {
            currentValue = BarraManager.Instancia.HungerCurrentValue;
            maxValue = BarraManager.Instancia.HungerMaxValue;
            changeRate = BarraManager.Instancia.HungerChangeRate;
            rageActivated = BarraManager.Instancia.RageActivated;
        }

        // 2. Si ya estaba en modo Ira, pasar el mando y desactivar este
        if (rageActivated && rageSystem != null)
        {
            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, hungryCircleColor);
        }
        else
        {
            Activate();
        }

        UpdateUI();
    }

    protected override void UpdateValue()
    {
        if (rageActivated) return;

        currentValue -= changeRate * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        // Guardar en el Manager
        if (BarraManager.Instancia != null)
            BarraManager.Instancia.HungerCurrentValue = currentValue;

        if (currentValue <= 0 && rageSystem != null)
        {
            rageActivated = true;
            if (BarraManager.Instancia != null) BarraManager.Instancia.RageActivated = true;

            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);
        }
    }

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

    void GenerateDerivedColors()
    {
        normalCircleColor = Color.Lerp(initialCircleColor, Color.yellow, 0.5f);
        hungryCircleColor = Color.Lerp(initialCircleColor, Color.red, 0.7f);
    }
}