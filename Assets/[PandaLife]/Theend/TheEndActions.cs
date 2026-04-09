using UnityEngine;
using UnityEngine.SceneManagement;
public class TheEndActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString MainmenuScene;


    public void MainMenu()
    {
       
      
        SceneManager.LoadScene(MainmenuScene.Value);
    }
    public void Replay()
    {
        if (BarraManager.Instancia != null)
            BarraManager.Instancia.PrepareRetry();

        SceneManager.LoadScene(HomeScene.Value);
    }
}
