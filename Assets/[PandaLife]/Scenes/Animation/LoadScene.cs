using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        EndLoadScene();
    }

    public void StartLoadScene()
    {
        Debug.Log("StartLoadScene called");
        anim.SetTrigger("Start");
    }

    public void EndLoadScene()
    {
        if (anim != null)
        {
            anim.SetTrigger("End");
        }
    }
}