using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenuManager : MonoBehaviour 
{
    private GameObject cheatPanel;
    [SerializeField] private MenuCauldron menucauldron;
    [SerializeField] private DayNightCycle daynightcycle;


    HungerSystem[] pandas;

    public HungerSystem minipanda;
    RageSystem rageSystem;
    
    void Start()
    {
        // Ira al minimo
 
        pandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);
        // Buscamos el Canvas persistente usando su Singleton, y obtenemos el primer hijo (el Panel)
        if (CheatMenuPersistent.instance != null)
        {
            cheatPanel = CheatMenuPersistent.instance.transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        // Bloquea el cheat menu si estamos en la escena "Theend"
        if (SceneManager.GetActiveScene().name == "Theend")
        {
            cheatPanel.SetActive(false); // cierra el menú si estaba abierto
            return;
        }
            

        if (Input.GetButtonDown("CheatMenu")) // He puesto en el input manager la tecla K
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (cheatPanel != null)
        {
            bool isMenuOpen = !cheatPanel.activeSelf;
            cheatPanel.SetActive(isMenuOpen);
        }
    }

    public void Cheat_DarBambu()
    {
        GameManager.instance.sumarBambu(10, 1);
        GameManager.instance.sumarBambu(10, 2);
        GameManager.instance.sumarBambu(10, 3);
        GameManager.instance.sumarBambu(10, 4);
        Debug.Log("CHEAT: +10 Bambú");
    }

    public void Cheat_QuitarBambu()
    {
        GameManager.instance.quitarBambu();
        Debug.Log("CHEAT: 0 Bambú");
    }

    public void Cheat_acelerardia()
    {
        if (GameManager.instance.multiplicadorvelocidaddia == 1f)
        {
            GameManager.instance.multiplicadorvelocidaddia = 20f;
            Debug.Log("CHEAT: multiplicador de velocidad activado");
        }
        else
        {
            GameManager.instance.multiplicadorvelocidaddia = 1f;
            Debug.Log("CHEAT: multiplicador de velocidad desactivado");
        }
    }

    public void Cheat_adelantardia()
    {
        GameManager.instance.tiempoTranscurrido = 280.0f;
    }

    public void Cheat_Volver_dia1()
    {
        if (!GameManager.instance.numday.Equals(1))
        {
            GameManager.instance.tedypersistente = false;
            GameManager.instance.notepersistente = false;
            GameManager.instance.numday = 1;
            GameManager.instance.quitarBambu();
            SceneManager.LoadScene("Main");
        }
    }

    // Cheat: Llevar directamente al Game Over
    public void Cheat_GameOver()
    {
        SceneManager.LoadScene("Theend");
       
    }

    // Cheat: Pausar o reanudar la disminución de hambre
    public void Cheat_PausarOReanudarHambre()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = !BarraManager.Instancia.hungerPaused;
            Debug.Log(BarraManager.Instancia.hungerPaused ? "CHEAT: Hambre pausada" : "CHEAT: Hambre reanudada");
        }
    }

    public void RerollPedidos()
    {
        PandaRequest pReq = GameManager.instance.GetComponent<PandaRequest>();

        if (pReq != null)
        {
            pReq.GenerateRandomRequests();

            RequestManager ui = Object.FindFirstObjectByType<RequestManager>();
            if (ui != null)
            {
                ui.ActualizarTextosManual();
            }

            Minipandas[] todosLosPandas = Object.FindObjectsByType<Minipandas>(FindObjectsSortMode.None);
            foreach (var panda in todosLosPandas) panda.ActualizarPedidoDebug();

            Debug.Log("Reroll de pedidos completado.");
        }
    }

    public void DesbloquearPedidos()
    {
        PandaRequest pReq = GameManager.instance.GetComponent<PandaRequest>();
        if (pReq != null)
        {
            pReq.UnlockCropsForDay(2);
            Debug.Log("Cultivos del Día 2 desbloqueados.");
        }
    }

    public void Cheat_MiniPandaEnIra()
    {

        HungerSystem[] todosLosPandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        if (todosLosPandas.Length > 0)
        {

            HungerSystem pandaAFectar = todosLosPandas[0];

            pandaAFectar.CurrentValue = 5f;
            Debug.Log("CHEAT: Forzando ira en " + pandaAFectar.name);
        }
        else
        {
            Debug.LogWarning("CHEAT: No se encontró ningún HungerSystem en esta escena.");
        }
    }

    public void Cheat_ResetHambreEira()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = false;
            BarraManager.Instancia.isResetting = false;
        }

        // Buscamos los pandas actuales en la escena
        HungerSystem[] pandasEnEscena = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        foreach (var panda in pandasEnEscena)
        {
   
            panda.Restaurar(100f);


            panda.ResetSystem();


            panda.UpdateUI();
        }

        Debug.Log("CHEAT: Todas las barras al 100% y estados de ira limpiados.");
    }

}