using UnityEngine;

public class SoundWalkingMinipanda : MonoBehaviour
{
    //RECUERDA esto tu lo llamas desde el animation de walking con un evento dentro de la animation
    //que esta escondidio
   [SerializeField] private AudioSource audioSource;

    public void ReproducirPaso()
    {
        if (audioSource != null)
        {
            audioSource.Play(); 
        }
    }
}
