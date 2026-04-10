using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BarSystem : MonoBehaviour, IBarSystem
{
    //Variables comunes a ambos sistemas
    [SerializeField] protected Slider bar;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected Image indicatorImage;

    //Variables de la barra
    [SerializeField] protected float maxValue = 100f;
    [SerializeField] protected float currentValue;
    [SerializeField] protected float changeRate = 1f;
    [SerializeField] protected TMP_Text valueText;
    protected bool isActive = false;

    // Propiedades
    public float CurrentValue { get => currentValue; set => currentValue = value; }
    public float MaxValue { get => maxValue; set => maxValue = value; }
    public float ChangeRate { get => changeRate; set => changeRate = value; }
    public bool IsActive => isActive;


    public virtual void Activate() => isActive = true;
    public virtual void Deactivate() => isActive = false;

    // Método para actualizar la UI y el valor de la barra
    public void UpdateSystem()
    {
        if (!isActive) return;
        UpdateValue();
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        if (bar != null)
        {
            bar.maxValue = maxValue;
            bar.value = currentValue;
        }

        if (valueText != null)
        {
            valueText.text = Mathf.RoundToInt(currentValue).ToString() ;
        }

        UpdateColors();
    }

    protected virtual void Update()
    {
        UpdateSystem();
    }

    protected abstract void UpdateValue();
    protected abstract void UpdateColors();

    
}