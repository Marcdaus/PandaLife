using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString ConfigurationScene;
    [SerializeField] private GameString MainmenuScene;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip backSound;
    
    public void Play()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        SceneManager.LoadScene(HomeScene.Value); 
    }
    public void Configuration()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        SceneManager.LoadScene(ConfigurationScene.Value); 
    }
    public void Exit()
    {//para simular que sales del juego

        //UnityEditor.EditorApplication.isPlaying = false; // Para salir del modo Play en el Editor 
        SoundManager.instance.PlaySfx(buttonSound);
        Application.Quit(); // Para compilaciones finales

    }
    public void back()
    {
        SoundManager.instance.PlaySfx(backSound);
        SceneManager.LoadScene("Mainmenu"); 
    }
}

