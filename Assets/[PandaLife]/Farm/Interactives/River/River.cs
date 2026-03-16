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
        PickupDrop bucket = player.GetBucket();

        if (bucket == null) return;

        BucketWater water = bucket.GetComponent<BucketWater>();

        if (water != null)
        {
            water.Fill();
        }
    }
}