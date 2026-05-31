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

        if (GameManager.instance != null && !GameManager.instance.tutorialRioCompletado)
        {
            GameManager.instance.tutorialRioCompletado = true;
            Debug.Log("Tutorial de coger el cubo completado para esta partida.");
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