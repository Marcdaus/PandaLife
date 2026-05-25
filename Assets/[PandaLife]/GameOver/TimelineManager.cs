using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{

    [Header("Referencias")]
    public PlayableDirector director;

    [Header("Configuración de Retroceso")]
    [Tooltip("Velocidad a la que retrocederá el Timeline (en segundos por frame)")]
    public float velocidadRetroceso = 0.05f; 

    void Update()
    {
        if (director == null) return;

           //PAUSAR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Si está reproduciendo, lo pausa. Si está pausado, continúa.
            if (director.state == PlayState.Playing)
            {
                director.Pause();
            }
            else
            {
                director.Play();
            }
        }

        // ATRASAR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Si el Timeline se estaba reproduciendo, lo pausamos para poder retroceder manualmente
            if (director.state == PlayState.Playing)
            {
                director.Pause();
            }

           
            director.time = Mathf.Max(0f, (float)director.time - velocidadRetroceso);

         
            director.Evaluate();
        }
    }
}

