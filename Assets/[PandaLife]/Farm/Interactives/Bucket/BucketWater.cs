using UnityEngine;

// Fíjate que vuelve a ser MonoBehaviour
public class BucketWater : MonoBehaviour
{
    [SerializeField] private GameObject emptybucketobject;
    [SerializeField] private GameObject fullbucketobject;

    public bool hasWater = false;

    private void Start()
    {
        UpdateVisual();
    }

    public void Fill()
    {
        if (hasWater) return;

        hasWater = true;
        UpdateVisual();
        Debug.Log("Cubo lleno");

        // Si el tutorial está activo y esto es un cubo, completamos el paso
        if (TutorialManager.instance != null && GetComponent<BucketWater>() != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.CogerCubo);
        }
    }

    public void Empty()
    {
        hasWater = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (emptybucketobject != null) emptybucketobject.SetActive(!hasWater);
        if (fullbucketobject != null) fullbucketobject.SetActive(hasWater);
    }
}