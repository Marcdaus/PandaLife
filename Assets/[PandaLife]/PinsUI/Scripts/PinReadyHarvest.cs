using UnityEngine;

public class PinReadyHarvest : PinUIElement
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
            // El pin se muestra si está listo para ser cosechado
            return currentCrop.IsHarvestable();
        }

        return false;
    }
}