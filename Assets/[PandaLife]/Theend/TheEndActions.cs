using UnityEngine;
using UnityEngine.SceneManagement;
public class TheEndActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString MainmenuScene;


    public void MainMenu()
    {
        // Limpiar datos de BarraManager
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerValues.Clear();
            BarraManager.Instancia.rageValues.Clear();
            BarraManager.Instancia.rageStates.Clear();
            BarraManager.Instancia.sceneLoaded = false;
            BarraManager.Instancia.comingFromGameOver = false;
        }
        SceneManager.LoadScene(MainmenuScene.Value);
    }
    public void Replay()
    {
        if (BarraManager.Instancia != null)
            BarraManager.Instancia.PrepareRetry();

        SceneManager.LoadScene(HomeScene.Value);
    }
}
