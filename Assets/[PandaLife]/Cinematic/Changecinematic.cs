using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Collections.Generic;
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
    public LoadScene LoadScene;
    // Update is called once per frame

   

    
    public IEnumerator Start()
    {
        LoadScene = FindFirstObjectByType<LoadScene>();
        //Scena 1
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("dialogbox_1");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowLine(dialoguelines[0], Dialogue_scene1, dialogueText_1));

        //Scena 2

        yield return new WaitForSeconds(3f);
        anim.SetTrigger("de1a2");
       

        yield return new WaitForSeconds(2f);
        anim.SetTrigger("dialogbox_2");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ShowLine(dialoguelines[1], Dialogue_scene2, dialogueText_2));

        yield return new WaitForSeconds(3f);
        //Scena 3
        anim.SetTrigger("de2a3");

        yield return new WaitForSeconds(2f);
        anim.SetTrigger("dialogbox_3");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ShowLine(dialoguelines[2], Dialogue_scene3, dialogueText_3));

        yield return new WaitForSeconds(3f);
        
        if (LoadScene != null)
        {
            LoadScene.StartLoadScene();
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }
    public IEnumerator changescene()
    {
        // Iniciamos la animación de transición
        if (LoadScene != null)
        {
            LoadScene.StartLoadScene();
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("GameOver");
    }

    public void SkipCinematic()
    { 
        StartCoroutine(changescene());
    }
    private IEnumerator ShowLine(string dialogueline,AudioSource clip, TextMeshProUGUI text) 
    {
        //reproduce el sonido del dialogo
        clip.Play();
        //limpia el texto antes de mostrar la nueva linea
        text.text = string.Empty;
        //muestra cada letra de la linea de dialogo con un retraso entre cada una
        foreach (char letter in dialogueline.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(textvelocity);
        }
    }
      
}
