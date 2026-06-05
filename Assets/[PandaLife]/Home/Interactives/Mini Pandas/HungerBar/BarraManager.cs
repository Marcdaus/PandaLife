using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BarraManager : MonoBehaviour
{
    private static BarraManager _instancia;
    public static BarraManager Instancia => _instancia;

    // public float hungerMaxValue = 100f;
    // public float hungerChangeRate = 0.5f;

    public float rageMaxValue = 100f;
    public float rageChangeRate = 0.5f;

    public Dictionary<string, float> hungerValues = new();
    public Dictionary<string, float> rageValues = new();
    public Dictionary<string, bool> rageStates = new();

    public bool sceneLoaded = false;
    public bool comingFromGameOver = false;
    public bool hungerPaused = false;
    public bool isResetting = false;

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

    private void Update()
    {
        CheckRage();
    }

    public void ResetSceneState()
    {
        sceneLoaded = false;
        isResetting = true;

        Invoke(nameof(EndResetFlag), 0.2f);
    }

    private void EndResetFlag()
    {
        isResetting = false;
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
                ragingCount++;
        }

        if (ragingCount == 3)
        {
            sceneLoaded = true;
            SceneManager.LoadScene("Cinematic");
            return;
        }

        foreach (var value in rageValues.Values)
        {
            if (value >= rageMaxValue)
            {
                sceneLoaded = true;
                SceneManager.LoadScene("Cinematic");
                return;
            }
        }
    }
}