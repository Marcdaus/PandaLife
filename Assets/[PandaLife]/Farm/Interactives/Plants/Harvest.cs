using UnityEngine;

public class Harvest : Interactuable
{
    [SerializeField] private Crop crop;
    public FarmingArea area; //se guarda la parcela donde esta plantado para marcarla como vacia al cosechar

    void Awake()
    {
        crop = GetComponent<Crop>();
       
    }

    public override void Interactuar()
    {
        if (crop == null) return;

        
        if (crop.IsHarvestable())
        {
          
            crop.Harvest(); // llama a la funci�n de cosechar del Crop

            if (area != null)
                area.VaciarParcela(); // libera la parcela
            Debug.Log("Terreno libre para plantar");
        }
        else
        {
            Debug.Log("A�n no est� libre");
        }
    }

    // permite que Player obtenga la referencia al Crop
    public Crop GetCrop()
    {
        return crop;
    }
}