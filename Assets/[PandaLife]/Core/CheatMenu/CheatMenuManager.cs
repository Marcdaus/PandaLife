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
    [SerializeField] PinUIElement pinUIElement;

    HungerSystem[] pandas;
    [SerializeField] private PandaRequest pandaRequest;

    public HungerSystem minipanda;
    RageSystem rageSystem;
    
    void Start()
    {
        // Ira al minimo
        pinUIElement.gameObject.SetActive(false);
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
        if (!GameManager.instance.numeroDia.Equals(1))
        {
            GameManager.instance.tedypersistente = false;
            GameManager.instance.notepersistente = false;
            GameManager.instance.numeroDia = 1;
            GameManager.instance.quitarBambu();
            SceneManager.LoadScene("Main");
        }
        StartCoroutine(MostrarTemporal(2f));
    }
    public IEnumerator MostrarTemporal(float duracion)
    {
        pinUIElement.Show();

        yield return new WaitForSeconds(duracion);

        pinUIElement.Hide();
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
        pandaRequest.GenerateRandomRequests();
    }

    public void DesbloquearPedidos() 
    {
        pandaRequest.UnlockCropsForDay(2);
    }


    public void Cheat_MiniPandaEnIra()
    {
        
        if (BarraManager.Instancia != null)
        {
            minipanda.CurrentValue = 10f;

            Debug.Log("CHEAT: bajar hambre");
        }
    }
    public void Cheat_ResetHambreEira()
    {
        // Reset global de hambre si existe sistema global
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = false;
        }

        // Buscar todos los pandas en escena

        foreach (var panda in pandas)
        {
            //  hambre al máximo
            panda.Restaurar(100f);

            //Ira al minimo
            rageSystem = panda.GetComponent<RageSystem>();
            rageSystem.SetRage(0);
        }

        Debug.Log("CHEAT: hambre a 100 y ira reseteada");
    }

}