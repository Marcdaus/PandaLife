using UnityEngine;

public class BarraManager : MonoBehaviour
{
    //Variables que guardan datos
    private static BarraManager _instancia;
    public static BarraManager Instancia => _instancia;

   
    public float HungerCurrentValue = 100f;
    public float HungerMaxValue = 100f;
    public float HungerChangeRate = 5f;

    
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