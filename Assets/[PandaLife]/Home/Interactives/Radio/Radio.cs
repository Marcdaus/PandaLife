using UnityEngine;
using System.Collections;

public class Radio : Interactuable
{
    [SerializeField] private ParticleSystem notasParticles;
    [SerializeField] private float duracion;

    public override void Interactuar() { }

    public void PlayNotas()
    {
        if (notasParticles != null)
            StartCoroutine(PlayAndStop());
    }

    IEnumerator PlayAndStop()
    {
        notasParticles.Play();
        yield return new WaitForSeconds(duracion);
        notasParticles.Stop();
    }
}
