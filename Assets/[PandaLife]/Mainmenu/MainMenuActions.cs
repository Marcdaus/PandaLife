using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString ConfigurationScene;
    [SerializeField] private GameString MainmenuScene;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip backSound;
    public LoadScene LoadScene;


    private void Start()
    {

        LoadScene = FindFirstObjectByType<LoadScene>();
    }
    public void Play()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        StartCoroutine(changescene(HomeScene.Value));
        
    }
    public void Configuration()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        SceneManager.LoadScene(ConfigurationScene.Value); 
    }

    public IEnumerator changescene(string sceneName)
    {
        // Iniciamos la animación de transición
        if (LoadScene != null)
        {
            LoadScene.StartLoadScene();
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
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

