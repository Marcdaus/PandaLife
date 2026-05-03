using UnityEngine;
using UnityEngine.UI;

public class RageSystem : BarSystem
{
    //Variables específicas del sistema de ira
    [SerializeField] private string pandaID;

    private Color calmColor = new Color(1f, 0f, 0f); // Rojo claro
    private Color rageColor = new Color(0.6f, 0f, 0f); // Rojo oscuro

    [SerializeField] private Sprite calmFace;
    [SerializeField] private Sprite rageFace;



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

    public void ActivateRage(Slider sharedBar, Image sharedFill)
    {
        //Realiza las asignaciones
        bar = sharedBar;
        fillImage = sharedFill;
        GameManager.instance.miniPandasHambrientos--;

        if (BarraManager.Instancia != null && pandaID != "")
    {
        var manager = BarraManager.Instancia;

        if (manager.rageValues.ContainsKey(pandaID))
            currentValue = manager.rageValues[pandaID];

        maxValue = manager.rageMaxValue;
        changeRate = manager.rageChangeRate;
    }
    Debug.Log("ACTIVANDO RAGE DE " + pandaID);

        Activate();
    }


    //Actualiza el valor de ira
    protected override void UpdateValue()
    {
        currentValue += changeRate * Time.deltaTime * GameManager.instance.barmultiplicator;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

    }

    //Cambia el color al pasar el 50% de la barra
    protected override void UpdateColors()
    {
        float percentage = (currentValue / maxValue) * 100f;

        if (fillImage != null)
            fillImage.color = (percentage < 50f) ? calmColor : rageColor;
            
        if (indicatorImage != null)
                indicatorImage.sprite = (percentage < 50f) ? calmFace : rageFace;
    }
    
    protected override void Update()
    {
        if (isActive)
            {
            //Debug.Log("RAGE UPDATE FUNCIONANDO");
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

     public void ReducirIraPorcentaje(float porcentaje)
    {
        float reduction = (porcentaje / 100f) * maxValue;
        currentValue -= reduction;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        UpdateUI();
        
    }
    public void ResetSystem()
    {
        currentValue = 0f;
        UpdateColors();
        if (bar != null) bar.value = 0f;
    }


}