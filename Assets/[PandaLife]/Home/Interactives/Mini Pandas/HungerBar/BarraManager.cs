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
    public float hungerChangeRate = 1f;

    //Variables de ira
    public float rageMaxValue = 100f;
    public float rageChangeRate = 1f;

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
        // Actualización de ira
       /* foreach (var key in rageValues.Keys.ToList())
        {
            
            /ageValues[key] += rageChangeRate * Time.deltaTime;
            rageValues[key] = Mathf.Clamp(rageValues[key], 0f, rageMaxValue);
            rageStates[key] = rageValues[key] >= rageMaxValue;
        }*/

        CheckRage();

    }
    

    public void PrepareRetry()
    {
        comingFromGameOver = true;

        // Compureba si los tres pandas estan en ira
        bool allThreeRaging = rageStates.Count == 3 && rageStates.Values.All(s => s);

        // Reduce la ira de todos los pandas al 25% del máximo
        foreach (var key in rageValues.Keys.ToList())
        {
            if (rageValues[key] >= rageMaxValue)
            {
                rageValues[key] = rageMaxValue * 0.25f;
                rageStates[key] = false;
                foreach (var ey in rageValues.Keys)
                {
                    Debug.Log("Panda ID: " + key);
                }
            }
        }

        // 3 Si los 3 estaban en ira, restaurar hambre al 25%
        if (allThreeRaging)
        {
            foreach (var key in hungerValues.Keys.ToList())
            {
                hungerValues[key] = hungerMaxValue * 0.25f;
            }
        }

        sceneLoaded = false;
    }

    public void CheckRage()
    {
        if (sceneLoaded || rageValues.Count == 0) return;

        //Game Over de los 3 pandas: revisamos si los tres específicos están en ira
        string[] pandaIDs = new string[] { "DarkPanda", "RedPanda", "LightPanda" };
        bool allThreeRaging = pandaIDs.All(id => rageStates.ContainsKey(id) && rageStates[id]);

        if (allThreeRaging)
        {
            sceneLoaded = true;
            comingFromGameOver = false;
            Debug.Log("GAME OVER: Los 3 pandas están en ira");
            SceneManager.LoadScene("Theend");
        }

        // Game Over individual: cualquier panda llega a ira máxima
        foreach (var value in rageValues.Values)
        {
            if (value >= rageMaxValue)
            {
                sceneLoaded = true;
                comingFromGameOver = false;
                Debug.Log("GAME OVER: Un panda llegó a ira máxima");
                SceneManager.LoadScene("Theend");
                foreach (var key in rageValues.Keys)
                {
                    Debug.Log("Clave en rageValues: " + key);
                }
                return;
            }
        }

        
    }

}