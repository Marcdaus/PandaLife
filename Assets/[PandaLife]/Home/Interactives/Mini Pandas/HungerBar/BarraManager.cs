using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class BarraManager : MonoBehaviour
{
    //Variables de Singleton
    private static BarraManager _instancia;
    public static BarraManager Instancia => _instancia;

    //Variables de hambre
    public float hungerMaxValue = 100f;
    public float hungerChangeRate = 0.5f;

    //Variables de ira
    public float rageMaxValue = 100f;
    public float rageChangeRate = 0.5f;

    // Diccionarios para almacenar valores de hambre e ira por panda, y estados de ira
    public Dictionary<string, float> hungerValues = new();
    public Dictionary<string, float> rageValues = new();
    public Dictionary<string, bool> rageStates = new();

    // Flags de control
    public bool sceneLoaded = false; // Evita recargas múltiples
    public bool comingFromGameOver = false; // Flag para reinicio
    public bool hungerPaused = false; // Flag global para pausar hambre

    private void Awake()
    {
        if (_instancia == null)
        {
            _instancia = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // No poner nada más aqui, las barras chequearán hungerPaused individualmente
    }

    private void Update()
    {
        
        CheckRage();

    }

    public void PrepareRetry()
    {

        BarraManager.Instancia.hungerValues.Clear();
        BarraManager.Instancia.rageValues.Clear();
        BarraManager.Instancia.rageStates.Clear();
    }

    public void CheckRage()
    {
        if (sceneLoaded) return;

        HungerSystem[] pandas =
            FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        int ragingCount = 0;

        foreach (var panda in pandas)
        {
            if (panda.IsRageActivated)
            {
                ragingCount++;
                
            }
        }

        // Todos en estado de ira
        if (ragingCount == 3)
        {
            sceneLoaded = true;
            comingFromGameOver = false;
            Debug.Log("GAME OVER: Los 3 pandas están en ira");
            SceneManager.LoadScene("Theend");
            return;
        }

      //Uno en ira la máximo
        foreach (var value in rageValues.Values)
        {
            if (value >= rageMaxValue)
            {
                sceneLoaded = true;
                comingFromGameOver = false;
                Debug.Log("GAME OVER: Un panda llegó a ira máxima");
                SceneManager.LoadScene("Theend");
                return; 
            }
        }
    }
}




