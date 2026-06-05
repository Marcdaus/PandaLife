using UnityEngine;

public class ColisionTutorialRight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // La flecha que no le gusta a Minerva
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.Flecha1);
        }
    }
}
