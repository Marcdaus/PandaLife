using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource opentransition;
    [SerializeField] private AudioSource closetransition;
    private void Start()
    {
        anim = GetComponent<Animator>();
        EndLoadScene();
    }

    public void StartLoadScene()
    {
        
        opentransition.Play();
        Debug.Log("StartLoadScene called");
        anim.SetTrigger("Start");
    }

    public void EndLoadScene()
    {
        if (anim != null)
        {
            closetransition.Play();
            anim.SetTrigger("End");
        }
    }
}