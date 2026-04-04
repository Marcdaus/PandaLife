using UnityEngine;

public class Cauldron : Interactuable
{
    [SerializeField] private MenuCauldron cauldronmenuUI;
    public override void Interactuar()
    {
        cauldronmenuUI.OpenCauldron();
    }

}
