using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString ConfigurationScene;
    [SerializeField] private GameString MainmenuScene;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource backSound;
    public void Play()
    {
        buttonSound.Play();
        SceneManager.LoadScene(HomeScene.Value); 
    }
    public void Configuration()
    {
        buttonSound.Play();
        SceneManager.LoadScene(ConfigurationScene.Value); 
    }
    public void Exit()
    {//para simular que sales del juego

        //UnityEditor.EditorApplication.isPlaying = false; // Para salir del modo Play en el Editor 
        buttonSound.Play();
        Application.Quit(); // Para compilaciones finales

    }
    public void back()
    {
        backSound.Play();
        SceneManager.LoadScene("Mainmenu"); 
    }
}

