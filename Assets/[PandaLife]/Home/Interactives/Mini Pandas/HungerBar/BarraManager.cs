using UnityEngine;

public class BarraManager : MonoBehaviour
{
    private static BarraManager _instancia;
    public static BarraManager Instancia => _instancia;

    [Header("Hunger Data")]
    public float HungerCurrentValue = 100f;
    public float HungerMaxValue = 100f;
    public float HungerChangeRate = 5f;

    [Header("Rage Data")]
    public bool RageActivated = false;
    public float RageCurrentValue = 0f;
    public float RageMaxValue = 100f;
    public float RageChangeRate = 5f;

    private void Awake()
    {
        if (_instancia == null)
        {
            _instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}