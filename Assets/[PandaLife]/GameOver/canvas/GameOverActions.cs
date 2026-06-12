using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverActions : MonoBehaviour
{
    [SerializeField] private GameString HomeScene;
    [SerializeField] private GameString MainmenuScene;
    [SerializeField] private AudioClip buttonSound;

    public void MainMenu()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        GameManager.instance.Resetplay();
        SceneManager.LoadScene(MainmenuScene.Value);

    }
    public void Replay()
    {
        SoundManager.instance.PlaySfx(buttonSound);
        GameManager.instance.Resetplay();
        SceneManager.LoadScene(HomeScene.Value);
    }
}
