using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool isopen = false;
    [SerializeField] private Player panda;
    [SerializeField] private GameString homescene;
    [SerializeField] private GameString mainmenuscene;
    [SerializeField] private AudioMixerSnapshot normalaudio;
    [SerializeField] private AudioMixerSnapshot pausedaudio;
    [SerializeField] private GameObject pausemask;
    private bool paused = false;

    //Seleccionar a los pandas
    [SerializeField] private Path minipandared;
    [SerializeField] private Path minipandablack;
    [SerializeField] private Path minipandalight;

   
  


    public void TimeStop()
    {
        Debug.Log("CHEAT: Detener/Reanudar tiempo");
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
        Debug.Log("CHEAT: Detener/Reanudar barras");
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = !BarraManager.Instancia.hungerPaused;
            Debug.Log(BarraManager.Instancia.hungerPaused ? "CHEAT: Hambre pausada" : "CHEAT: Hambre reanudada");
        }
    }

    public void StopPanda()
    {
        Debug.Log("CHEAT: Detener/Reanudar panda");
        if (isopen)
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
        Debug.Log("CHEAT: Detener/Reanudar mini pandas");
        if (minipandared == null && minipandablack == null && minipandalight == null)
        {
            Debug.Log("No hay mini pandas asignados, se omite.");
            return;
        }
        if (isopen)
        {
            minipandared.StopPandas();
            minipandablack.StopPandas();
            minipandalight.StopPandas();
        }
        else
        {
            minipandared.ResumePandas();
            minipandablack.ResumePandas();
            minipandalight.ResumePandas();
        }
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
                pausedaudio.TransitionTo(1f);
                pausemask.SetActive(true);
            }
            else
            {
                normalaudio.TransitionTo(1f);
                pausemask.SetActive(false);
            }
        }
    }
    //====================================================
    //                BBOTONES DEL MENÚ
    //====================================================
    public void OpenMenu()
    {
        isopen = true;
        pausemask.SetActive(true);
        StopMiniPandas();
        StopBars();
        TimeStop();
        StopPanda();
        Paused = true;
    }
    public void Continue()
    {
        isopen = false;
        pausemask.SetActive(false);
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
   
        Paused = false;
    } 
    // Botón para el menú principal
    public void GoToMainMenu()
    {
        isopen = false;
        pausemask.SetActive(false);
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        Paused = false;
       
        SceneManager.LoadScene(mainmenuscene.Value);
        BarraManager.Instancia.ResetSceneState();
        GameManager.instance.Resetplay();
    }
    public void Replay() 
    { 
        isopen = false;
        StopPanda();
        StopMiniPandas();
        StopBars();
        TimeStop();
        Paused = false;
        pausemask.SetActive(false);
        if (BarraManager.Instancia != null)
        {
            SceneManager.LoadScene(homescene.Value);
            BarraManager.Instancia.ResetSceneState();
            GameManager.instance.Resetplay();
        }
    }

}
