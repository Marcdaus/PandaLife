using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class Changecinematic : MonoBehaviour
{
    [SerializeField] private Image scene1;
    [SerializeField] private Image scene2;
    [SerializeField] private Image scene3;
    [SerializeField] private Animator anim;


    // Update is called once per frame
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        anim.SetTrigger("de1a2");
        yield return new WaitForSeconds(5f);
        anim.SetTrigger("de2a3");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameOver");
    }
    public void changescene()
    {
        if(scene1.gameObject.activeSelf == true)
        {
            anim.SetTrigger("de1a2");
        }
        else if (scene2.gameObject.activeSelf == true)
        {
            anim.SetTrigger("de2a3");
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
 
        
    }
}
