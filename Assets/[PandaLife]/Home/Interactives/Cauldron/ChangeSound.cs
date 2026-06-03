using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSound : MonoBehaviour
{
    [SerializeField] private AudioSource Sound1;
    [SerializeField] private AudioSource sound2;
    public void Change()
    {
        if (Sound1.enabled == true || sound2.enabled == false)
        {
            Sound1.enabled = false;
            sound2.enabled = true;
        }
        else {
            Sound1.enabled = true;
            sound2.enabled = false;
        }
    }

}