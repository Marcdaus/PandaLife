using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class Changecinematic : MonoBehaviour
{
    [SerializeField] private Image scene1;
    [SerializeField] private Image scene2;
    [SerializeField] private Image scene3;
    [Header("animaciones")]

    [SerializeField] private Animator anim;

    [Header("Tiempo entre imagenes en segundos")]
    [SerializeField] private float time;

    [Header("Lineas de dialogo")]
    [SerializeField,TextArea(4,6)] private string[] dialoguelines;
    [SerializeField] private TextMeshProUGUI dialogueText_1;
    [SerializeField] private TextMeshProUGUI dialogueText_2;
    [SerializeField] private TextMeshProUGUI dialogueText_3;
    [SerializeField] private float textvelocity;
    [Header("Sonidos")]
    [SerializeField] private AudioSource Dialogue_scene1;
    [SerializeField] private AudioSource Dialogue_scene2;
    [SerializeField] private AudioSource Dialogue_scene3;
    // Update is called once per frame
    public IEnumerator Start()
    {
        //Scena 1
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("dialogbox_1");
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowLine(dialoguelines[0], Dialogue_scene1));

        //Scena 2
        yield return new WaitForSeconds(time);
        anim.SetTrigger("de1a2");
        yield return new WaitForSeconds(time);

        //Scena 3
        anim.SetTrigger("de2a3");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("GameOver");
    }
    public void changescene()
    {
        //si esta oculta la escena 1, entonces se activa la escena 2 
        if (scene1.gameObject.activeSelf == true)
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

    private IEnumerator ShowLine(string dialogueline,AudioSource clip) 
    {
        //reproduce el sonido del dialogo
        clip.Play();
        //limpia el texto antes de mostrar la nueva linea
        dialogueText_1.text = string.Empty;
        //muestra cada letra de la linea de dialogo con un retraso entre cada una
        foreach (char letter in dialogueline.ToCharArray())
        {
            dialogueText_1.text += letter;
            yield return new WaitForSeconds(textvelocity);
        }
    }
}
