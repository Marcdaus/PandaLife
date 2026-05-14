using UnityEngine;

public class River : Interactuable
{
    Player player;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }
    public override void Interactuar()
    {
        if (player != null && player.IsHoldingBucket())
        {
            Debug.Log("El jugador está recogiendo agua del río...");
        }
        else
        {
            Debug.Log("Necesitas tener el cubo en la mano para recoger agua.");
        }
    }
}