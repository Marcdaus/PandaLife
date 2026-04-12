using UnityEngine;
using UnityEngine.SceneManagement;
public class TheEndActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString MainmenuScene;


    public void MainMenu()
    {
            GameManager.instance.Resetplay();
            SceneManager.LoadScene(MainmenuScene.Value);
        
    }
    public void Replay()
    {

        GameManager.instance.Resetplay();
        SceneManager.LoadScene(HomeScene.Value);
    }
}
