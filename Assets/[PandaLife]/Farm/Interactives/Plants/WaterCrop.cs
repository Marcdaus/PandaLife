using UnityEngine;

public class WaterCrop : Interactuable
{
    [SerializeField] private Crop crop;
    private Player player;

    void Awake()
    {
        crop = GetComponent<Crop>();
        player = FindFirstObjectByType<Player>();
    }

    public override void Interactuar()
    {
        PickupDrop bucket = player.GetBucket();

        if (bucket == null)
        {
            Debug.Log("Necesitas el cubo");
            return;
        }

        BucketWater water = bucket.GetComponent<BucketWater>();

        if (water == null || !water.haswater)
        {
            Debug.Log("El cubo está vacío");
            return;
        }

        crop.Water();
        water.Empty();
        
    }
}