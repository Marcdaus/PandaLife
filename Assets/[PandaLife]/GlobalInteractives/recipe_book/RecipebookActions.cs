using UnityEngine;

public class RecipebookActionv : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public void Open_notebook()
    {
        anim.SetTrigger("Open");
    }
    public void Close_notebook()
    {
        anim.SetTrigger("Close");
    }
}
