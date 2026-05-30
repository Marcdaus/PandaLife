using UnityEngine;

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
        sourceActual = sourceGrass;
    }

public void ReproducirPaso()
{
    Debug.Log("REPRODUCIRPASO LLAMADO");

    if(sourceActual == null)
    {
        Debug.Log("NULL");
        return;
    }

    Debug.Log("VOY A TOCAR WOOD? " + (sourceActual == sourceWood));
    Debug.Log("VOY A TOCAR GRASS? " + (sourceActual == sourceGrass));

    sourceActual.Play();
}

    public void ActualizarSueloDesdeHijo(Collider o)
{
    int layer = o.gameObject.layer;

    if(layer == LayerMask.NameToLayer(layerTierra))
    {
        sourceActual = sourceGrass;
        Debug.Log("GRASS elegido");
    }
    else if(layer == LayerMask.NameToLayer(layerMadera))
    {
        sourceActual = sourceWood;
        Debug.Log("WOOD elegido");
    }
    else if(layer == LayerMask.NameToLayer(layerPiedra))
    {
        sourceActual = sourceHome;
        Debug.Log("HOME elegido");
    }
}
}