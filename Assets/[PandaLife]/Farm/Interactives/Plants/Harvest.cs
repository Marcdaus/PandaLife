using UnityEngine;

public class Harvest : Interactuable
{

    [SerializeField] private Crop crop;
    public FarmingArea area; // Se guarda la parcela para marcarla vacía al cosechar

      [SerializeField] private AudioSource audiosource;

    void Awake()
    {
        crop = GetComponent<Crop>();
    }

    // Condición para negar con la cabeza
    public override bool ShouldShakeHead(Player player)
    {
        // Si el jugador intenta cosechar pero NO tiene las manos vacías
        if (!player.IsHandEmpty())
        {
            return true;
        }

        return false;
    }

    public override void Interactuar(Player player)
    {
        if (crop == null) return;

        if (!player.IsHandEmpty())
        {
            Debug.Log("No puedes cosechar con las manos ocupadas.");
            return;
        }

        if (crop.IsHarvestable())
        {
            //audiosource.Play();
            AudioSource.PlayClipAtPoint(
                audiosource.clip,
                transform.position
            );
            crop.Harvest(); // Llama a la función de cosechar del Crop

            if (area != null)
                area.VaciarParcela(); // Libera la parcela

            Debug.Log("Terreno libre para plantar");
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.Cosechar);
            }
        }
        else
        {
            Debug.Log("Aún no está listo");
        }
    }

    public Crop GetCrop()
    {
        return crop;
    }
}