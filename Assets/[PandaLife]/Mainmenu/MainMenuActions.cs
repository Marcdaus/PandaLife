using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Farm"); 
    }
    public void Configuration()
    {
        SceneManager.LoadScene("Configuration"); 
    }
    public void Exit()
    {//para simular que sales del juego

        UnityEditor.EditorApplication.isPlaying = false; // Para salir del modo Play en el Editor 

        //Application.Quit(); // Para compilaciones finales

    }
    public void back()
    {
        SceneManager.LoadScene("Mainmenu"); 
    }
}

