using UnityEngine;
using UnityEngine.SceneManagement;

public class StepsPlayer : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource sourceGrass;
    [SerializeField] private AudioSource sourceWood;
    [SerializeField] private AudioSource sourceHome;

    [Header("Layers")]
    [SerializeField] private string layerTierra = "FarmingGrounds";
    [SerializeField] private string layerMadera = "BridgeFloor";
    [SerializeField] private string layerPiedra = "HouseGrounds";

    private AudioSource sourceActual;

    private void Start()
    {
        string nombreEscena = SceneManager.GetActiveScene().name;
        if (nombreEscena == "Farm")
        {
            sourceActual = sourceGrass;
        }
        else {
            sourceActual = sourceHome;
        }
        
    }

public void ReproducirPaso()
{
   
    if(sourceActual == null)
    {
        return;
    }

    sourceActual.Play();
}

    public void ActualizarSueloDesdeHijo(Collider o)
{
    int layer = o.gameObject.layer;

    if(layer == LayerMask.NameToLayer(layerTierra))
    {
        sourceActual = sourceGrass;
       // Debug.Log("GRASS elegido");
    }
    else if(layer == LayerMask.NameToLayer(layerMadera))
    {
        sourceActual = sourceWood;
      //  Debug.Log("WOOD elegido");
    }
    else if(layer == LayerMask.NameToLayer(layerPiedra))
    {
        sourceActual = sourceHome;
      // Debug.Log("HOME elegido");
    }
}
}