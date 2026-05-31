using UnityEngine;

public class River : Interactuable
{
    public override bool ShouldShakeHead(Player player)
    {
        // Si el jugador intenta interactuar con el río, tiene el cubo en la mano, 
        // pero el cubo YA TIENE AGUA, entonces niega con la cabeza.
        if (player.IsHoldingBucket())
        {
            BucketWater cubo = player.pickedobject.GetComponent<BucketWater>();
            if (cubo != null && cubo.hasWater)
            {
                return true;
            }
        }
        return false;
    }

    public override void Interactuar(Player player)
    {
        
        if (player.IsHoldingBucket())
        {
            Debug.Log("El jugador está recogiendo agua del río...");
        }
        else
        {
            Debug.Log("Necesitas tener el cubo en la mano para recoger agua.");
        }
    }
}