using UnityEngine;
using UnityEngine.UI;

public enum ActionIconType
{
    None,
    PickUpBucket,
    Harvest,
    Water,
    NeedBucket,
    EmptyBucket,
    Pet,
    Feed,
    Interact,
    Plant,
    PickUpSeedBag,
    Radio,
    Cauldron
}

public class InteractionTextUI : MonoBehaviour
{
    public static InteractionTextUI instance;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private Image iconoInteraccion;

    [Header("UI Soltar (Drop)")]
    [SerializeField] private GameObject panelSoltar; // El nuevo objeto/panel diferente

    [Header("Sprites de Acción")]
    [SerializeField] private Sprite iconPickUpBucket;
    [SerializeField] private Sprite iconHarvest;
    [SerializeField] private Sprite iconWater;
    [SerializeField] private Sprite iconNeedBucket;
    [SerializeField] private Sprite iconEmptyBucket; 
    [SerializeField] private Sprite iconPet;
    [SerializeField] private Sprite iconFeed;
    [SerializeField] private Sprite iconInteract; // Icono genérico, patita
    [SerializeField] private Sprite iconPlant;
    [SerializeField] private Sprite iconPickUpSeedBag;
    [SerializeField] private Sprite iconRadio;
    [SerializeField] private Sprite iconCauldron;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        OcultarIcono();
        OcultarIconoSoltar();
    }

    public void MostrarIcono(ActionIconType tipoAccion)
    {
        if (iconoInteraccion != null)
        {
            // Asignamos el sprite correspondiente según la acción que pase el Player
            switch (tipoAccion)
            {
                case ActionIconType.PickUpBucket: iconoInteraccion.sprite = iconPickUpBucket; break;
                case ActionIconType.Harvest: iconoInteraccion.sprite = iconHarvest; break;
                case ActionIconType.Water: iconoInteraccion.sprite = iconWater; break;
                case ActionIconType.NeedBucket: iconoInteraccion.sprite = iconNeedBucket; break;
                case ActionIconType.EmptyBucket: iconoInteraccion.sprite = iconEmptyBucket; break;
                case ActionIconType.Pet: iconoInteraccion.sprite = iconPet; break;
                case ActionIconType.Feed: iconoInteraccion.sprite = iconFeed; break;
                case ActionIconType.Plant: iconoInteraccion.sprite = iconPlant; break;
                case ActionIconType.PickUpSeedBag: iconoInteraccion.sprite = iconPickUpSeedBag; break;
                case ActionIconType.Radio: iconoInteraccion.sprite = iconRadio; break;
                case ActionIconType.Cauldron: iconoInteraccion.sprite = iconCauldron; break;
                case ActionIconType.Interact: iconoInteraccion.sprite = iconInteract; break;
                default: iconoInteraccion.sprite = null; break;
            }
        }

        if (panelPrincipal != null && tipoAccion != ActionIconType.None)
        {
            panelPrincipal.SetActive(true);
        }
    }

    public void OcultarIcono()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }
    }

    public void MostrarIconoSoltar()
    {
        if (panelSoltar != null && !panelSoltar.activeSelf)
        {
            panelSoltar.SetActive(true);
        }
    }

    public void OcultarIconoSoltar()
    {
        if (panelSoltar != null && panelSoltar.activeSelf)
        {
            panelSoltar.SetActive(false);
        }
    }
}