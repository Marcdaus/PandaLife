using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void TimeStop()
    {
        if (GameManager.instance.stopTime == false)
        {
            GameManager.instance.stopTime = true;
            Debug.Log("CHEAT: Tiempo detenido");
        }
        else
        {
            GameManager.instance.stopTime = false;
            Debug.Log("CHEAT: Tiempo reanudado");
        }
    }
    public void StopBars()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = !BarraManager.Instancia.hungerPaused;
            Debug.Log(BarraManager.Instancia.hungerPaused ? "CHEAT: Hambre pausada" : "CHEAT: Hambre reanudada");
        }
    }


}
