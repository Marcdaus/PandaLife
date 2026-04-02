using UnityEngine;
using UnityEngine.AI;

public class Path : MonoBehaviour
{
    // Variables
    [SerializeField] Transform[] pathPoints;

    NavMeshAgent agent;

    [SerializeField] float waitTime;

    private float timer = 0f;

    private bool waiting = false;

    private Places currentPlace;

    private static bool managerInitialized = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Inicializa PlacesManager solo una vez
        if (!managerInitialized)
        {
            PlacesManager.Initialize(pathPoints);
            managerInitialized = true;
        }

        MoveToNextPlace(); // primer movimiento
    }

    void Update()
    {
        // Si llegó al destino y aún no está esperando
        if (!waiting && !agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            waiting = true;
            timer = waitTime; // iniciar temporizador
        }

        // Si está esperando, descontar tiempo
        if (waiting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                waiting = false;

                // Liberar el lugar anterior
                if (currentPlace != null)
                    PlacesManager.FreePlace(currentPlace);

                // Va al siguiente lugar
                MoveToNextPlace();
            }
        }
    }

    void MoveToNextPlace()
    {
        //Coge y va al siguiente lugar
        currentPlace = PlacesManager.GetNextFreePlace();
        agent.SetDestination(currentPlace.position.position);

        Debug.Log($"{agent.name} va a {currentPlace.position.name}");
    }
}