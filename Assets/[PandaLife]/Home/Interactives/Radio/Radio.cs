using UnityEngine;
using System.Collections;

public class Radio : Interactuable
{
    [SerializeField] private ParticleSystem notasParticles;
    [SerializeField] private float duracion;
    [SerializeField] private AudioSource macarena;

    // Condiciónnegar con la cabeza
    public override bool ShouldShakeHead(Player player)
    {
        // No puedes ponerte a bailar si tienes algo en las manos
        return !player.IsHandEmpty();
    }

    public override void Interactuar(Player player)
    {
        if (!player.IsHandEmpty()) return;
        macarena.Play();
        PlayNotas();
    }

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