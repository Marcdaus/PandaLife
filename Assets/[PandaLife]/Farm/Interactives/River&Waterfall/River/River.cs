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
            // 2. Le ordenamos al jugador que empiece la secuencia de animación
            player.CollectWater();
        }
        else
        {
            Debug.Log("Necesitas tener el cubo en la mano para recoger agua.");
        }
    }
}