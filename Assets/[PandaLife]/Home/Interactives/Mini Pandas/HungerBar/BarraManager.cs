using UnityEngine;

public class BarraManager : MonoBehaviour
{
    private static BarraManager _instancia;
    public static BarraManager Instancia => _instancia;

    // config
    public float hungerMaxValue = 100f;
    public float hungerChangeRate = 1f;

    public float rageMaxValue = 100f;
    public float rageChangeRate = 1f;

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