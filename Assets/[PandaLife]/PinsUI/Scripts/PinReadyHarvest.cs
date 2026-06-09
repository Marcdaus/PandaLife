using UnityEngine;
using UnityEngine.UI;

public class PinReadyHarvest : PinUIElement
{
    [SerializeField] private FarmingArea farmingArea;
    [SerializeField] private Image pinImage;
    [SerializeField] private Sprite[] cropSprites;
    
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
            if (currentCrop.IsHarvestable())
            {
                ActualizarSpritePin(currentCrop.type);
                return true;
            }
        }

        return false;
    }

    private void ActualizarSpritePin(int cropType)
    {
        // Validamos que el componente Image no sea nulo y que el tipo esté dentro del rango del array
        if (pinImage != null && cropType >= 0 && cropType < cropSprites.Length)
        {
            if (cropSprites[cropType] != null)
            {
                pinImage.sprite = cropSprites[cropType];
            }
        }
        else
        {
            Debug.Log($"tipocultivo: {cropType}. algo fue mal");
        }
    }
}