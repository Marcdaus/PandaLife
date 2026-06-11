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

    [SerializeField] private Animator animPrincipal;
    [SerializeField] private Animator animSoltar;
    private bool iconoPrincipalActivo = false;
    private bool iconoSoltarActivo = false;

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
            if (!iconoPrincipalActivo)
            {
                panelPrincipal.SetActive(true);
                if (animPrincipal != null) animPrincipal.SetTrigger("Mostrar");
                iconoPrincipalActivo = true;
            }
        }
    }

    public void OcultarIcono()
    {
        if (iconoPrincipalActivo)
        {
            if (animPrincipal != null) animPrincipal.SetTrigger("Ocultar");
            iconoPrincipalActivo = false;
        }
    }

    public void MostrarIconoSoltar()
    {
        if (!iconoSoltarActivo)
        {
            panelSoltar.SetActive(true);
            if (animSoltar != null) animSoltar.SetTrigger("Mostrar");
            iconoSoltarActivo = true;
        }
    }

    public void OcultarIconoSoltar()
    {
        if (iconoSoltarActivo)
        {
            if (animSoltar != null) animSoltar.SetTrigger("Ocultar");
            iconoSoltarActivo = false;
        }
    }
}