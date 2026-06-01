using UnityEngine;

public class PinOpenExclamation : PinUIElement
{
    public override bool CheckCondition()
    {
        return  TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.OpenRedDragon ||
                TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.OpenUchuva;
    }
}
