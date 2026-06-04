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

        // Comprobamos el bloque entero de 4 parcelas
        FarmingArea miArea = GetComponentInParent<FarmingArea>();
        if (miArea != null && miArea.transform.parent != null)
        {
            // Buscamos todas las plantas en este grupo
            Crop[] todosLosCultivos = miArea.transform.parent.GetComponentsInChildren<Crop>();
            bool todasListas = true;

            foreach (Crop c in todosLosCultivos)
            {
                // Si al menos una planta no está regada, dejamos que se gaste el agua de la cubeta
                if (!c.IsWatered && !c.IsHarvestable())
                {
                    todasListas = false;
                    break;
                }
            }

            // Si todas estaban regadas/cosechables, dice que no
            if (todasListas) return true;
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

        bool riegoAlguna = false;

        // Buscamos a la farming area y a la parcela
        FarmingArea miArea = GetComponentInParent<FarmingArea>();

        if (miArea != null && miArea.transform.parent != null)
        {
            // Recolectamos todas las plantas dentro del grupo de 4
            Crop[] todosLosCultivos = miArea.transform.parent.GetComponentsInChildren<Crop>();

            // Regamos todas las que necesiten agua
            foreach (Crop c in todosLosCultivos)
            {
                if (!c.IsWatered && !c.IsHarvestable())
                {
                    c.Water();
                    riegoAlguna = true;
                }
            }
        }
        
        // Solo vaciamos el cubo si conseguimos regar al menos una planta
        if (riegoAlguna)
        {
            water.Empty();
        }
        else
        {
            Debug.Log("Todas las plantas de este bloque ya están regadas o listas para cosechar.");
        }
    }
}
