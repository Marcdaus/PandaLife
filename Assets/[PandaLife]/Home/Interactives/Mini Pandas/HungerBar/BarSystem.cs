using UnityEngine;
using UnityEngine.UI;

public abstract class BarSystem : MonoBehaviour, IBarSystem
{
    [Header("UI References")]
    [SerializeField] protected Slider bar;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected Image indicatorImage;

    [Header("Settings")]
    [SerializeField] protected float maxValue = 100f;
    [SerializeField] protected float currentValue;
    [SerializeField] protected float changeRate = 5f;

    protected bool isActive = false;

    public virtual void Activate() => isActive = true;
    public virtual void Deactivate() => isActive = false;

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
        UpdateColors();
    }

    protected virtual void Update()
    {
        UpdateSystem();
    }

    protected abstract void UpdateValue();
    protected abstract void UpdateColors();

    // Propiedades
    public float CurrentValue { get => currentValue; set => currentValue = value; }
    public float MaxValue { get => maxValue; set => maxValue = value; }
    public float ChangeRate { get => changeRate; set => changeRate = value; }
}