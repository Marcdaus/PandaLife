using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubblesSound : MonoBehaviour
{
    [SerializeField] private AudioSource bubble;
    [SerializeField] private AudioSource bubbleintensity;
    public void ChangeSound()
    {
        if (bubble.enabled == true || bubbleintensity.enabled == false)
        {
            bubble.enabled = false;
            bubbleintensity.enabled = true;
        }
        else {
            bubble.enabled = true;
            bubbleintensity.enabled = false;
        }
    }

}