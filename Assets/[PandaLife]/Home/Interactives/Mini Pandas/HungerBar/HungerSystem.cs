using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : BarSystem
{
    [SerializeField] private string pandaID;
    [SerializeField] private PandaBarUIConection barraUI;

    [SerializeField] private RageSystem rageSystem;
    private bool rageActivated = false;

    private Color satisfiedBarColor = Color.green;
    private Color normalBarColor = Color.yellow;
    private Color hungryBarColor = Color.red;

    [SerializeField] Color initialCircleColor;
    private Color normalCircleColor;
    private Color hungryCircleColor;

    private bool isPaused = false;

    public bool IsRageActivated => rageActivated;
    void Start()
    {
        GenerateDerivedColors();

        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.hungerValues.ContainsKey(pandaID))
            {
                currentValue = manager.hungerValues[pandaID];
                rageActivated = manager.rageStates[pandaID];
            }
            else
            {
                currentValue = maxValue;
                rageActivated = false;
                manager.hungerValues[pandaID] = currentValue;
                manager.rageStates[pandaID] = rageActivated;
            }
        }

        if (rageActivated && rageSystem != null)
        {
            Deactivate(); // hambre OFF

            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);

            if (barraUI != null)
                barraUI.SetRage(rageSystem);
        }
        else
        {
            Activate(); // hambre ON si no hay ira

            if (barraUI != null)
                barraUI.SetHunger(this);
        }

        UpdateUI();
    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem(); //Actualiza solo si esta activa en barra de hambre
    }

    //Recalcula el valor de hambre, considerando pausas y el cheat global
    protected override void UpdateValue()
    {
        // Pausar hambre si está activo el cheat global
        bool globalPause = BarraManager.Instancia != null && BarraManager.Instancia.hungerPaused;

        if (!isPaused && !globalPause)
        {
            currentValue -= changeRate * Time.deltaTime * GameManager.instance.barmultiplicator;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }

        //Si el sistema de ira está asignado, no se ha activado aún y el hambre llegó a 0, activamos la ira
        if (rageSystem != null && !rageActivated && currentValue <= 0)
        {
            rageActivated = true;
            Deactivate();
            rageSystem.ActivateRage(bar, fillImage, indicatorImage, indicatorImage.color);
          //  GameManager.instance.miniPandasHambrientos--;
            if (barraUI != null)
                barraUI.SetRage(rageSystem);
        }

        //Debug.Log("Hambre en Update: " + currentValue);
    }

    void LateUpdate()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            if (BarraManager.Instancia.isResetting) return;
            BarraManager.Instancia.hungerValues[pandaID] = currentValue;
            BarraManager.Instancia.rageStates[pandaID] = rageActivated;
        }
    }

    //Cambia el color segun el estado
    protected override void UpdateColors()
    {
        if (!rageActivated)
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

    //Genera los diferentes colores de cada carita
    void GenerateDerivedColors()
    {
        normalCircleColor = Color.Lerp(initialCircleColor, Color.yellow, 0.2f);
        hungryCircleColor = Color.Lerp(initialCircleColor, Color.red, 0.2f);
    }

    //Restaura un porcentaje de la barra, considerando el valor máximo
    public void Restaurar(float cantidad)
    {
        float amount = (cantidad / 100f) * maxValue;
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        UpdateUI();
    }

    //Pausa el hambre
    public void PauseHunger(float seconds)
    {
        StartCoroutine(PauseRoutine(seconds));
    }

    private System.Collections.IEnumerator PauseRoutine(float seconds)
    {
        isPaused = true;
        yield return new WaitForSeconds(seconds);
        isPaused = false;
    }
    public void ResetSystem()
    {
        if (rageActivated)
        {
            currentValue = 0f;

            if (rageSystem != null)
            {
                rageSystem.ResetSystem();
            }
        }
        else
        {
            currentValue = maxValue;

            if (rageSystem != null)
            {
                rageSystem.ResetSystem();
            }
        }
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        UpdateColors();
        if (bar != null)
        {
            bar.value = currentValue; 
        }
    }
    public void ResetParaNuevoDia()
    {
        if (rageActivated)
        {
            currentValue = 0f;
            if (rageSystem != null)
            {
                rageSystem.ResetSystem();
            }
        }
        else
        {
            currentValue = maxValue;
        }

        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerValues[pandaID] = currentValue;
            BarraManager.Instancia.rageStates[pandaID] = rageActivated;
        }

        UpdateColors();
        if (bar != null) bar.value = currentValue;
    }

    public void ForceFullReset()
    {
        rageActivated = false; 
        currentValue = maxValue; 
        Activate(); 

        if (rageSystem != null)
            rageSystem.ResetSystem();

        UpdateUI();
    }
}