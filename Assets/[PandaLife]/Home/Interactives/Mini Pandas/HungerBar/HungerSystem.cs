using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private PandaBarUIConection barraUI;
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

        if (BarraManager.Instancia != null)
        {
            maxValue = BarraManager.Instancia.hungerMaxValue;
            changeRate = BarraManager.Instancia.hungerChangeRate;
        }
      
        //Si la ira ya está activada, muestra la barra de ira
        if (rageActivated && rageSystem != null)
        { Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, hungryCircleColor);
        }
        else
        { Activate();}

        UpdateUI();

    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {

        // Disminuir hambre solo si no está pausada
        if (!isPaused)
        {
            currentValue -= changeRate * Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        }

        // Activar ira si llega a 0 o si RageActivated ya está en true
        if (rageSystem != null && !rageActivated && currentValue <= 0 )     
        {
            rageActivated = true;
            Deactivate(); // apaga la barra de hambre 
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);
            
            if (barraUI != null)
                barraUI.SetRage(rageSystem); 

        }

            Debug.Log("Hambre en Update: " + currentValue);
    }

    //Funcion que actualiza los colores de las caras y barras
    protected override void UpdateColors()
    {
        if (!rageActivated) // <-- solo cambia colores si no hay ira
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
    }
    //Hace que las caras varien de color a otro tono
    void GenerateDerivedColors()
    {
        normalCircleColor = Color.Lerp(initialCircleColor, Color.yellow, 0.5f);
        hungryCircleColor = Color.Lerp(initialCircleColor, Color.red, 0.7f);
    }

    public void Restaurar(float cantidad)
    {
        float amount = (cantidad / 100f) * maxValue;

        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        Debug.Log("Nueva hambre: " + currentValue);

        UpdateUI();

    }

    public void PauseHunger(float seconds)
    {
        StartCoroutine(PauseRoutine(seconds));
    }

    private IEnumerator PauseRoutine(float seconds)
    {
        isPaused = true;
        yield return new WaitForSeconds(seconds);
        isPaused = false;
    }

    public void ForceRage()
{
    if (!rageActivated)
    {
        rageActivated = true;

        Deactivate();

        rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);

        if (barraUI != null)
            barraUI.SetRage(rageSystem);
    }
}
}