using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString ConfigurationScene;
    [SerializeField] private GameString MainmenuScene;
    public void Play()
    {
        SceneManager.LoadScene(HomeScene.Value); 
    }
    public void Configuration()
    {
        SceneManager.LoadScene(ConfigurationScene.Value); 
    }
    public void Exit()
    {//para simular que sales del juego

        //UnityEditor.EditorApplication.isPlaying = false; // Para salir del modo Play en el Editor 

        Application.Quit(); // Para compilaciones finales

    }
    public void back()
    {
        SceneManager.LoadScene(MainmenuScene.Value); 
    }
}

