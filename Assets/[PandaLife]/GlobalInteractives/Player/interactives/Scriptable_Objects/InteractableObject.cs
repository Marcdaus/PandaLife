using UnityEngine;

[CreateAssetMenu(fileName = "InteractableObject", menuName = "Interactable Objects/InteractableObject")]
public class InteractableObject : ScriptableObject
{
    public string actionText = "Interactuar"; // Texto para la UI
    public string animationTrigger = "Interactuar"; // El trigger del Animator
    public AudioClip interactionSound; // Sonido único para este objeto
    public AudioClip errorSound; // Sonido si el jugador se equivoca (Agitar la cabeza)
    public AudioClip dropSound; // Sonido si el jugador suelta el objeto
}
