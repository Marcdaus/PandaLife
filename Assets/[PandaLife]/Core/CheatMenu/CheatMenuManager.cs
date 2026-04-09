using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheatMenuManager : MonoBehaviour
{
    private GameObject cheatPanel;
    [SerializeField] private MenuCauldron menucauldron;
    [SerializeField] private DayNightCycle daynightcycle;

    private Dictionary<HungerSystem, float> originalRates = new Dictionary<HungerSystem, float>();
  

    void Start()
    {
        // Buscamos el Canvas persistente usando su Singleton, y obtenemos el primer hijo (el Panel)
        if (CheatMenuPersistent.instance != null)
        {
            cheatPanel = CheatMenuPersistent.instance.transform.GetChild(0).gameObject;
        }

    }

    void Update()
    {
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
        if(GameManager.instance.multiplicadorVelocidad == 1f)
        {
            GameManager.instance.multiplicadorVelocidad = 20f;
            Debug.Log("CHEAT: multiplicador de velocidad activado");
        }
        else
        {
            GameManager.instance.multiplicadorVelocidad = 1f;
            Debug.Log("CHEAT: multiplicador de velocidad desactivado");
        }

    }
    
    public void Cheat_adelantardia()
    {
        GameManager.instance.tiempoTranscurrido = 280.0f;
    }
    public void Cheat_Volver_dia1()
    {
        GameManager.instance.tedypersistente = false;
        GameManager.instance.notepersistente = false;
        GameManager.instance.numeroDia = 1;
        GameManager.instance.quitarBambu();
        SceneManager.LoadScene("Home");
    }

    /*public void Cheat_cambiarDia(int dia)
    {
        Debug.Log("Has seleccionado el día: " + dia);

        switch (dia)
        {
            case 1:
                GameManager.instance.numeroDia = 1;
                break;
            case 2:
                GameManager.instance.numeroDia = 2;
                break;
            case 3:
                GameManager.instance.numeroDia = 3;
                break;
        }
        daynightcycle.Rewards("Bags");
        daynightcycle.Rewards("Collectables");

        MenuCauldron menuCauldron = FindAnyObjectByType<MenuCauldron>();
        if (menuCauldron != null)
            menuCauldron.RefreshCards();
    }*/

    // Cheat: Poner a los 3 pandas en estado de ira
   public void Cheat_PandasIraMaxima()
    {
        HungerSystem[] pandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        foreach (var panda in pandas)
        {
            panda.ForceRage();
        }

        Debug.Log("CHEAT: Todos los pandas en ira máxima");
    }

        //  Cheat: Pausar o reanudar la disminución de hambre
    public void Cheat_PausarOReanudarHambre()
    {
        HungerSystem[] pandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        bool pause = true;

        foreach (var panda in pandas)
        {
            if (panda.ChangeRate == 0f)
                pause = false;
        }

        foreach (var panda in pandas)
        {
            if (pause)
            {
                if (!originalRates.ContainsKey(panda))
                    originalRates[panda] = panda.ChangeRate;

                panda.ChangeRate = 0f;
            }
            else
            {
                if (originalRates.ContainsKey(panda))
                    panda.ChangeRate = originalRates[panda];
            }
        }

        Debug.Log(pause ? "CHEAT: Hambre pausada" : "CHEAT: Hambre reanudada");
    }

}