using UnityEngine;

public class BucketWater : MonoBehaviour
{
    [SerializeField] private Renderer bucketrenderer;

    [SerializeField] private Material emptybucket;
    [SerializeField] private Material fullbucket;

    public bool haswater = false;

    public void Fill()
    {
        if (haswater)
        {
            Debug.Log("El cubo ya está lleno");
            return;
        }

        haswater = true;
        bucketrenderer.material = fullbucket;
        Debug.Log("Cubo lleno");
        // Completar el tutorial de llenar la cubeta y no mostrar más el pin
        if (!GameManager.instance.tutorialRioCompletado)
        {
            GameManager.instance.tutorialRioCompletado = true;
            Debug.Log("Tutorial de coger el cubo completado para esta partida.");
        }
    }

    public void Empty()
    {
        haswater = false;
        bucketrenderer.material = emptybucket;
    }
}