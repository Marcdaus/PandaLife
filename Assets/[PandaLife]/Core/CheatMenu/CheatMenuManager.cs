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
   // private CursorManager cursorManager;


    HungerSystem[] pandas;

    public HungerSystem minipanda;
    RageSystem rageSystem;

    /*private void Awake()
    {
        cursorManager = Object.FindFirstObjectByType<CursorManager>();
    }*/

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
            /*if (isMenuOpen) { 
                cursorManager.cursorblock = true;
                cursorManager.MostrarCursor();
            }
            else { cursorManager.cursorblock = false;
                cursorManager.OcultarCursor();
            }*/
            cheatPanel.SetActive(isMenuOpen);
        }
    }

    public void Cheat_Dar10Bambu()
    {
        GameManager.instance.sumarBambu(10, 1);
        GameManager.instance.sumarBambu(10, 2);
        GameManager.instance.sumarBambu(10, 3);
        GameManager.instance.sumarBambu(10, 4);
        Debug.Log("CHEAT: +10 Bambú");
    }
    public void Cheat_Dar1Bambu()
    {
        GameManager.instance.sumarBambu(1, 1);
        GameManager.instance.sumarBambu(1, 2);
        GameManager.instance.sumarBambu(1, 3);
        GameManager.instance.sumarBambu(1, 4);
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
    public void Cheat_retrasardia()
    {
        GameManager.instance.tiempoTranscurrido = 0f;
    }
    public void Cheat_parartimepo()
    {
       if(GameManager.instance.stopTime == false)
        {
            GameManager.instance.stopTime = true;
            Debug.Log("CHEAT: Tiempo detenido");
        }
       else
        {
            GameManager.instance.stopTime = false;
            Debug.Log("CHEAT: Tiempo reanudado");
        }
    }

    public void Cheat_Volver_dia1()
    {
        if (!GameManager.instance.numday.Equals(1))
        {
            if (GameManager.instance.numday == 1) return;
            GameManager.instance.Resetplay();
            SceneManager.LoadScene("House");
        }
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
            pReq.UnlockDishesForDay(2);
            pReq.UnlockDishesForDay(3);
            Debug.Log("Platos del Día 2 y 3 desbloqueados.");
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

    public void Cheat_acelerarbarras()
    {
        HungerSystem[] todosLosPandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        float nuevoRate;

        // Toggle simple
        if (todosLosPandas.Length > 0 && todosLosPandas[0].ChangeRate < 1f)
        {
            nuevoRate = 20f;
            Debug.Log("CHEAT: Barras aceleradas");
        }
        else
        {
            nuevoRate = 0.5f;
            Debug.Log("CHEAT: Barras normales");
        }

        foreach (var panda in todosLosPandas)
        {
            panda.ChangeRate = nuevoRate;
        }
    }
    public void Cheat_acelerarPandaIra()
    {
        HungerSystem[] todosLosPandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        if (todosLosPandas.Length == 0)
        {
            Debug.LogWarning("No hay pandas");
            return;
        }

        HungerSystem panda = todosLosPandas[0];
        RageSystem rage = panda.GetComponent<RageSystem>();

        // Siempre aceleramos el hambre
        panda.ChangeRate = 20f;

        if (rage != null)
        {
            if (panda.IsRageActivated)
            {
                rage.ChangeRate = 40f; // Ira más rápida
                Debug.Log("CHEAT: Panda en IRA → todo acelerado");
            }
            else
            {
                rage.ChangeRate = 1f; // Ira normal
                Debug.Log("CHEAT: Panda acelerado (sin ira)");
            }
        }
    }
    public void Cheat_cambiarporcentaje()
    {
        if(GameManager.instance.numday == 1)return;
        if (GameManager.instance.valuepercentage == 25)
        {
            GameManager.instance.valuepercentage = 15;
            GameManager.instance.barmultiplicator = 1.15f;
        }
        else
        {
            GameManager.instance.valuepercentage = 25;
            GameManager.instance.barmultiplicator = 1.25f;

        }
    }
}