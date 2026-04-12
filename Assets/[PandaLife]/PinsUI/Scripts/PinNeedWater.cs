using UnityEngine;

public class PinNeedWater : PinUIElement
{

    [SerializeField] private FarmingArea farmingArea;

    public override bool CheckCondition()
    {
        // Si no hay nada plantado
        if (farmingArea == null || !farmingArea.ThereIsSomething)
        {
            return false;
        }

        // Buscamos el cultivo actual
        Crop currentCrop = farmingArea.GetComponentInChildren<Crop>();

        if (currentCrop != null)
        {
            // El pin se muestra si no está regado y si no ha crecido del todo
            return !currentCrop.IsWatered && currentCrop.growthstage < currentCrop.maxStages;
        }

        return false;
    }
}