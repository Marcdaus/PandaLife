using UnityEngine;

public class PickupDrop : Interactuable
{
    [SerializeField] private Transform handpoint;
    private Rigidbody rb;
    private bool picked = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp()
    {
        if (picked) return;

        rb.useGravity = false;
        rb.isKinematic = true;

        transform.position = handpoint.position;
        transform.SetParent(handpoint);

        picked = true;
        Mensaje($"{rb.name} recogido");

        // Completar el tutorial de coger la cubeta y no mostrar más el pin
        if (!GameManager.instance.tutorialCuboCompletado)
        {
            GameManager.instance.tutorialCuboCompletado = true;
            Debug.Log("Tutorial de coger el cubo completado para esta partida.");
        }
    }

    public void Drop()
    {
        if (!picked) return;

        rb.useGravity = true;
        rb.isKinematic = false;

        transform.SetParent(null);

        picked = false;
        Mensaje($"{rb.name} soltado");
    }

    public override void Interactuar()
    {
        PickUp();
    }
}