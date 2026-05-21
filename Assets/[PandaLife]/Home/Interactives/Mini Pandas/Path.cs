using UnityEngine;
using UnityEngine.AI;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] pathPoints;

    NavMeshAgent agent;

    [SerializeField] float waitTime = 2f;

    private float timer = 0f;

    private bool waiting = false;

    private Places currentPlace;

    // Animator
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Obtener Animator
        animator = GetComponent<Animator>();

        PlacesManager.Initialize(pathPoints);

        MoveToNextPlace();
    }

    void Update()
    {
        // Velocidad actual del NavMeshAgent
        float speed = agent.velocity.magnitude;

        // Enviar velocidad al Animator
        animator.SetFloat("distance", speed);

        // Si llegó al destino
        if (!waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Comprobar que realmente se ha parado
            if (agent.velocity.magnitude < 0.1f)
            {
                waiting = true;
                timer = waitTime;

                // Idle
                animator.SetFloat("distance", 0f);
            }
        }

        // Espera
        if (waiting)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                waiting = false;

                // Liberar lugar anterior
                if (currentPlace != null)
                    PlacesManager.FreePlace(currentPlace);

                MoveToNextPlace();
            }
        }
    }

    void MoveToNextPlace()
    {
        currentPlace = PlacesManager.GetNextFreePlace();

        agent.SetDestination(currentPlace.position.position);
    }
}