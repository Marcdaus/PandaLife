using UnityEngine;
using UnityEngine.AI;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] pathPoints;
    NavMeshAgent agent;
    [SerializeField] float waitTime = 2f;

    private float timer = 0f;
    private bool waiting = false;

    private bool isPaused = false;

    private Places currentPlace;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PlacesManager.Initialize(pathPoints);
        MoveToNextPlace();
    }

    void Update()
    {
        if (isPaused)
        {
            animator.SetFloat("distance", 0f);
            return; 
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("distance", speed);

     
        if (!waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (agent.velocity.magnitude < 0.1f)
            {
                waiting = true;
                timer = waitTime;
                animator.SetFloat("distance", 0f);
            }
        }

        if (waiting)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                waiting = false;

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

    public void StopPandas()
    {
        isPaused = true;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; 
        }
    }

    public void ResumePandas()
    {
        isPaused = false;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false; 
        }
    }
}