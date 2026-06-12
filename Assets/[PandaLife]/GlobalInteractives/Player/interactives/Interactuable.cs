using UnityEngine;
using UnityEngine.Audio;

public abstract class Interactuable : MonoBehaviour , IInteractuable
{
    // Referencia del ScriptableObject
    [SerializeField] protected InteractableObject interactData;
    public AudioMixerGroup sfxMixerGroup;

    // Obligamos a cada hijo a definir qué pasa
    public abstract void Interactuar(Player player);

    // Por defecto, asumimos que siempre se puede interactuar
    public virtual bool CanInteract(Player player)
    {
        return true;
    }

    // Por defecto, no negamos con la cabeza.
    public virtual bool ShouldShakeHead(Player player)
    {
        return false;
    }

    // Devuelve los datos para que el Player lea el texto, animación y sonidos
    public InteractableObject GetInteractData()
    {
        return interactData;
    }

    protected void Mensaje(string texto)
    {
        Debug.Log(texto);
    }

    // Devuelve el texto para la UI. Por defecto usa el del ScriptableObject.
    public virtual string GetActionText(Player player)
    {
        return interactData != null ? interactData.actionText : "Interactuar";
    }

    // Devuelve el Trigger de animación. Por defecto usa el del ScriptableObject.
    public virtual string GetAnimationTrigger(Player player)
    {
        return interactData != null ? interactData.animationTrigger : "Interactuar";
    }

    protected void ReproducirSonidoEnPunto(AudioClip clip, Vector3 posicion)
    {
        if (clip == null) return;

        // Si se nos olvidó asignar el grupo en el Inspector, avisamos por consola
        if (sfxMixerGroup == null)
        {
            Debug.LogWarning($"Falta asignar el SFX Mixer Group en {gameObject.name}");
        }

        GameObject audioTemp = new GameObject("TempAudio_" + clip.name);
        audioTemp.transform.position = posicion;

        AudioSource source = audioTemp.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1f;

        // Asignamos el grupo
        source.outputAudioMixerGroup = sfxMixerGroup;

        source.Play();
        Destroy(audioTemp, clip.length);
    }
}
