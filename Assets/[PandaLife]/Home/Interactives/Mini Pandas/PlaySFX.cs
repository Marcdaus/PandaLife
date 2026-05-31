using System.Collections;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [Header("Clips por estado")]
    [SerializeField] private AudioClip[] happyClips;
    [SerializeField] private AudioClip[] hungryClips;
    [SerializeField] private AudioClip[] angryClips;

    [SerializeField] private AudioClip[] pettingClips;
    [SerializeField] private AudioClip[] eatingClips;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volúmenes")]
    [Range(0f, 1f)]
    [SerializeField] private float pettingVolume = 0.3f;

    [Range(0f, 1f)]
    [SerializeField] private float eatingVolume = 0.5f;

    private enum Emotion
    {
        None,
        Happy,
        Hungry,
        Angry
    }

    private Emotion currentEmotion = Emotion.None;

    private AudioClip[] currentClips;
    private int index;

    private Coroutine loopCoroutine;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // -------------------- PUBLIC API --------------------

    public void PlayHappy()
    {
        StartEmotion(Emotion.Happy, happyClips);
    }

    public void PlayHungry()
    {
        StartEmotion(Emotion.Hungry, hungryClips);
    }

    public void PlayAngry()
    {
        StartEmotion(Emotion.Angry, angryClips);
    }

    public void StopAll()
    {
        currentEmotion = Emotion.None;
        currentClips = null;
        index = 0;

        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }

        audioSource.Stop();
        audioSource.clip = null;
    }

    // -------------------- CORE --------------------

    private void StartEmotion(Emotion emotion, AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        if (currentEmotion == emotion && loopCoroutine != null)
            return;

        StopAll();

        currentEmotion = emotion;
        currentClips = clips;
        index = 0;

        loopCoroutine = StartCoroutine(LoopClips());
    }

    private IEnumerator LoopClips()
    {
        while (currentClips != null && currentClips.Length > 0)
        {
            AudioClip clip = currentClips[index % currentClips.Length];
            index++;

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }

    public void PlayPetting()
    {
        PlaySingleAction(pettingClips, pettingVolume);
    }

    public void Eating()
    {
        PlaySingleAction(eatingClips, eatingVolume);
    }

    private void PlaySingleAction(AudioClip[] clips, float volume)
    {
        if (clips == null || clips.Length == 0)
            return;

        AudioClip clip = clips[(int)Random.Range(0, clips.Length)];

        sfxSource.PlayOneShot(clip, volume);
    }
}