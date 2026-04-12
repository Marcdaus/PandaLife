using UnityEngine;
public class PinCauldronReady : PinUIElement
{
    [SerializeField] private Cauldron cauldron;

    public override bool CheckCondition()
    {
        return cauldron.tieneplatopendiente;
    }
}