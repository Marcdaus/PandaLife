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
        Mensaje("Cubo recogido");
    }

    public void Drop()
    {
        if (!picked) return;

        rb.useGravity = true;
        rb.isKinematic = false;

        transform.SetParent(null);

        picked = false;
        Mensaje("Cubo soltado");
    }

    public override void Interactuar()
    {
        PickUp();
    }
}