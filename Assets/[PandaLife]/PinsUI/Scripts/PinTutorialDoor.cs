using UnityEngine;

public class PinTutorialDoor : PinUIElement
{
    public override bool CheckCondition()
    {
        return !GameManager.instance.tutorialPuertaCompletado;
    }
}
