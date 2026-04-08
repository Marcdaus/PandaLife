using UnityEngine;
using UnityEngine.UI;

public class RageSystem : BarSystem
{
    private Color calmColor = new Color(1f, 0f, 0f); // Rojo claro
    private Color rageColor = new Color(0.6f, 0f, 0f); // Rojo oscuro
    private Color lockedFaceColor = Color.red;

    void Awake()
    {
        Deactivate();
    }

    void Start()
    {
        // Si al cargar escena el manager dice que la ira está activa, recuperamos datos
     /*   if (BarraManager.Instancia != null && BarraManager.Instancia.RageActivated)
        {
            currentValue = BarraManager.Instancia.RageCurrentValue;
            maxValue = BarraManager.Instancia.RageMaxValue;
           
        }*/
        if (BarraManager.Instancia != null)
        {
            maxValue = BarraManager.Instancia.rageMaxValue;
            changeRate = BarraManager.Instancia.rageChangeRate;
        }
            Activate();
            UpdateUI();
    }

    //Activa el estado de Ira
    public void ActivateRage(Slider sharedBar, Image sharedFill, Image sharedIndicator, Color currentFaceColor)
    {
        //Realiza las asignaciones
        bar = sharedBar;
        fillImage = sharedFill;
        indicatorImage = sharedIndicator;

        currentValue = 0f;

        lockedFaceColor = currentFaceColor;
        Activate();
    }

    protected override void UpdateValue()
    {
        Debug.Log("Ira actual: " + currentValue + " | Active: " + isActive);
        currentValue += changeRate * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

      }

    //Cambia el color al pasar el 50% de la barra
    protected override void UpdateColors()
    {
        float percentage = (currentValue / maxValue) * 100f;

        if (fillImage != null)
            fillImage.color = (percentage < 50f) ? calmColor : rageColor;

        if (indicatorImage != null)
        {
            Color c = lockedFaceColor;
            c.a = 1f;
            indicatorImage.color = c;
        }
    }
}