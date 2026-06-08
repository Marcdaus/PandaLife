using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip clip;

    public void PlaySound()
    {
        SoundManager.instance.PlaySfx(clip);
    }
}
