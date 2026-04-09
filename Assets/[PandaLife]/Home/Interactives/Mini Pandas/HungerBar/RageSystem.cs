using UnityEngine;
using UnityEngine.UI;

public class RageSystem : BarSystem
{
    //Variables específicas del sistema de ira
    [SerializeField] private string pandaID;

    private Color calmColor = new Color(1f, 0f, 0f); // Rojo claro
    private Color rageColor = new Color(0.6f, 0f, 0f); // Rojo oscuro
    private Color lockedFaceColor = Color.red;

    void Awake()
    {
        //Deactivate();
    }

    void Start()
    {
        // Si al cargar escena el manager dice que la ira está activa, recuperamos datos
     if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.rageValues.ContainsKey(pandaID))
            {
                currentValue = manager.rageValues[pandaID];
                 maxValue = BarraManager.Instancia.rageMaxValue;
                changeRate = BarraManager.Instancia.rageChangeRate;
            }
        }
       
         UpdateUI();
    }

    //Activa el estado de Ira
    public void ActivateRage(Slider sharedBar, Image sharedFill, Image sharedIndicator, Color currentFaceColor)
    {
        //Realiza las asignaciones
        bar = sharedBar;
        fillImage = sharedFill;
        indicatorImage = sharedIndicator;

        if (BarraManager.Instancia != null && pandaID != "")
    {
        var manager = BarraManager.Instancia;

        if (manager.rageValues.ContainsKey(pandaID))
            currentValue = manager.rageValues[pandaID];

        maxValue = manager.rageMaxValue;
        changeRate = manager.rageChangeRate;
    }
    Debug.Log("ACTIVANDO RAGE DE " + pandaID);

        lockedFaceColor = currentFaceColor;
        Activate();
    }
    //Actualiza el valor de ira
    protected override void UpdateValue()
    {
        //Debug.Log("Ira actual: " + currentValue + " | Active: " + isActive);
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

    protected override void Update()
{
    if (isActive)
        {
        Debug.Log("RAGE UPDATE FUNCIONANDO");
        UpdateSystem();
        }
}
    void LateUpdate()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            BarraManager.Instancia.rageValues[pandaID] = currentValue;
        }
    }

}