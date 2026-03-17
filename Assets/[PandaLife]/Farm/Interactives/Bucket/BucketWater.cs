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
    }

    public void Empty()
    {
        haswater = false;
        bucketrenderer.material = emptybucket;
    }
}