using UnityEngine;

public class PinOpenExclamation : PinUIElement
{
    public override bool CheckCondition()
    {
        return  TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.OpenRedDragon ||
                TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.OpenUchuva;
    }
    override public void Hide()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);

    }
}
