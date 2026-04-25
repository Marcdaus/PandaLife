using UnityEngine;

public class BucketWater : MonoBehaviour
{
    [SerializeField] private GameObject emptybucketobject;
    [SerializeField] private GameObject fullbucketobject;

    public bool haswater = false;

    private void Start()
    {
        UpdateVisual();
    }

    public void Fill()
    {
        if (haswater)
        {
            Debug.Log("El cubo ya está lleno");
            return;
        }

        haswater = true;
        UpdateVisual();

        Debug.Log("Cubo lleno");

        if (!GameManager.instance.tutorialRioCompletado)
        {
            GameManager.instance.tutorialRioCompletado = true;
            Debug.Log("Tutorial de coger el cubo completado para esta partida.");
        }
    }

    public void Empty()
    {
        haswater = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        emptybucketobject.SetActive(!haswater);
        fullbucketobject.SetActive(haswater);
    }
}