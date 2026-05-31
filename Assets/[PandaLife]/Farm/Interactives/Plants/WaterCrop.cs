using UnityEngine;

public class WaterCrop : Interactuable
{
    [SerializeField] private Crop crop;

    void Awake()
    {
        crop = GetComponent<Crop>();
    }

    public override bool ShouldShakeHead(Player player)
    {
        // Intentar regar sin cubo
        if (!player.IsHoldingBucket()) return true;

        // Intentar regar con el cubo vacío
        PickupDrop bucket = player.GetBucket();
        if (bucket != null)
        {
            BucketWater cubo = bucket.GetComponent<BucketWater>();
            if (cubo == null || !cubo.hasWater) return true;
        }

        // Intentar regar una planta ya regada o lista para cosechar
        if (crop != null)
        {
            if (crop.IsWatered) return true;
            if (crop.IsHarvestable()) return true;
        }

        return false;
    }

    public override void Interactuar(Player player)
    {
        PickupDrop bucket = player.GetBucket();

        if (bucket == null)
        {
            Debug.Log("Necesitas el cubo");
            return;
        }

        BucketWater water = bucket.GetComponent<BucketWater>();

        if (water == null || !water.hasWater)
        {
            Debug.Log("El cubo está vacío");
            return;
        }

        if (crop != null && (crop.IsWatered || crop.IsHarvestable()))
        {
            Debug.Log("La planta ya está regada o lista para cosechar.");
            return;
        }

        crop.Water();
        water.Empty();
    }
}