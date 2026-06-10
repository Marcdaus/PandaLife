using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool isopen = false;
    [SerializeField] private Player panda;
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString MainmenuScene;
    [SerializeField] private AudioMixerSnapshot normalAudio;
    [SerializeField] private AudioMixerSnapshot pausedAudio;
    [SerializeField] private GameObject pauseMask;
    private bool paused = false;


    public void TimeStop()
    {
        if (GameManager.instance.stopTime == false)
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
    public void StopBars()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = !BarraManager.Instancia.hungerPaused;
            Debug.Log(BarraManager.Instancia.hungerPaused ? "CHEAT: Hambre pausada" : "CHEAT: Hambre reanudada");
        }
    }

    public void StopPanda()
    {
        if (!isopen)
        {
            panda.DisableMovement();
        }
        else
        {
            panda.EnableMovement();
        }
    }

    public void StopMiniPandas()
    {

    }

   
    public bool Paused
    {
        get => paused;
        set
        {
            if (paused == value) return;
            paused = value;
            if (paused)
            {
                pausedAudio.TransitionTo(1f);
                pauseMask.SetActive(true);
            }
            else
            {
                normalAudio.TransitionTo(1f);
                pauseMask.SetActive(false);
            }
        }
    }
    //====================================================
    //                BBOTONES DEL MENÚ
    //====================================================
    public void OpenMenu()
    {
        isopen = true;

        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        Paused = Paused;
    }
    public void Continue()
    {
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        isopen= false;
        Paused = !Paused;
    } 
    // Botón para el menú principal
    public void GoToMainMenu()
    {
        isopen = false;
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        Paused = !Paused;
        GameManager.instance.Resetplay();
        SceneManager.LoadScene(MainmenuScene.Value);
    }
    public void Replay() 
    { 
        isopen = false;
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        Paused = !Paused;
        GameManager.instance.Resetplay();
        SceneManager.LoadScene(HomeScene.Value);
    }

}
