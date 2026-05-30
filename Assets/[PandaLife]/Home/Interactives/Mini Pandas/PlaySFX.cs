using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public AudioClip[] happyclips;
    public AudioClip[] hungryclips;
    public AudioClip[] angryclips;

    AudioSource audiosource;

    int happycount = 0;
    int hungrycount = 0;
    int angrycount = 0;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlayHappy()
    {
        if (happyclips.Length == 0) return;

        audiosource.PlayOneShot(happyclips[happycount % happyclips.Length]);
        happycount++;
    }

    public void PlayHungry()
    {
        if (hungryclips.Length == 0) return;

        audiosource.PlayOneShot(hungryclips[hungrycount % hungryclips.Length]);
        hungrycount++;
    }

    public void PlayAngry()
    {
        if (angryclips.Length == 0) return;

        audiosource.PlayOneShot(angryclips[angrycount % angryclips.Length]);
        angrycount++;
    }

    public void StopAll()
    {
        audiosource.Stop();
    }
}