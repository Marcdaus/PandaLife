using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class Changecinematic : MonoBehaviour
{
    [SerializeField] private Image scene1;
    [SerializeField] private Image scene2;
    [SerializeField] private Image scene3;
    

    // Update is called once per frame
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        scene1.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        scene2.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameOver");
    }
    public void changescene()
    {
        if(scene1.gameObject.activeSelf == true)
        {
            scene1.gameObject.SetActive(false);
        }else if (scene2.gameObject.activeSelf == true)
        {
            scene2.gameObject.SetActive(false);
        }else
        {
            SceneManager.LoadScene("GameOver");
        }
 
        
    }
}
