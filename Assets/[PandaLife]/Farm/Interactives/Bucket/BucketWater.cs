using UnityEngine;

public class BucketWater : MonoBehaviour
{
    [SerializeField] private Renderer bucketrenderer;

    [SerializeField] private Material emptybucket;
    [SerializeField] private Material fullbucket;

    public bool hasWater = false;

    public void Fill()
    {

        if (hasWater)
        {
            Debug.Log("El cubo ya está lleno");
            return; // No hacer nada más
        }

        hasWater = true;
        bucketrenderer.material = fullbucket;
        Debug.Log("Cubo lleno");
    }

    public void Empty()
    {
        hasWater = false;
        bucketrenderer.material= emptybucket;
    }
}