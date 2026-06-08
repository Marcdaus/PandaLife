using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    AudioSource m_AudioSource;
    void Awake()
    {

        if (instance == null)
        {  
            instance = this;
            m_AudioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }
}