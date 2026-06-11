using System.Collections;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioSource happypanda;

    [SerializeField] private AudioSource hungrypanda;

    [SerializeField] private AudioSource angrypanda;

    [SerializeField] private AudioSource pettingclips;

    [SerializeField] private AudioSource eatingclips;


    private Coroutine loopCoroutine;


    // -------------------- PUBLIC ESTADOS --------------------

    public void PlayHappy()
    {
        StopAll();
        happypanda.Play();

    }

    public void PlayHungry()
    {
        StopAll();
        hungrypanda.Play();
    }

    public void PlayAngry()
    {
        StopAll();
        angrypanda.Play();
    }

    public void StopAll()
    {

        happypanda.Stop();
        angrypanda.Stop();
        hungrypanda.Stop();

    }

    // -------------------- ACCIONES --------------------

    public void PlayPetting()
    {
        pettingclips.PlayOneShot(pettingclips.clip);
    }
    public void Eating()
    {
        eatingclips.PlayOneShot(eatingclips.clip);
    }

}