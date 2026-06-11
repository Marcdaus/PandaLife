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
        if (happypanda != null)
        {
            StopAll();
            happypanda.Play();
        }
        return;

    }

    public void PlayHungry()
    {
        if (hungrypanda != null)
        {
            StopAll();
            hungrypanda.Play();
        }
        return;
    }

    public void PlayAngry()
    {
        if (angrypanda != null)
        {
            StopAll();
            angrypanda.Play();
        }
        return;
    }

    public void StopAll()
    {
        if(happypanda != null)
        {
            happypanda.Stop();
            angrypanda.Stop();
            hungrypanda.Stop();
        }
     

    }

    // -------------------- ACCIONES --------------------

    public void PlayPetting()
    {
        pettingclips.Play();
    }
    public void Eating()
    {
        Debug.Log("Eating");
        eatingclips.Play();
    }

}