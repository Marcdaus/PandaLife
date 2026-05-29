using UnityEngine;

public class SoundWalkingMinipanda : MonoBehaviour
{
   public AudioSource audioSource;

    // Este método lo sigues llamando desde el Animation Event
    public void ReproducirPaso()
    {
        if (audioSource != null)
        {
            // Al darle Play, el AudioSource activará el contenedor 
            // y este elegirá un sonido al azar automáticamente
            audioSource.Play(); 
        }
    }
}
