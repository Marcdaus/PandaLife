using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] private AudioSource generalmusic;
    [SerializeField] private AudioSource gameovermusic;

    private void Start()
    {
        Scene escenaactual = SceneManager.GetActiveScene();
        string nombreescena= escenaactual.name;
        Change(nombreescena);
    }
    public void Change( string nombre)
    {
        if (nombre=="Cinematic" || nombre=="GameOver")
        {   
            generalmusic.enabled = false;
            gameovermusic.enabled = true;
           
        }
        else
        {
            generalmusic.enabled = true;
            gameovermusic.enabled = false;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        Change(escena.name);
    }

}
