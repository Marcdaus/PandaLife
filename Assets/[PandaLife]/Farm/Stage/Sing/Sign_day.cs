using UnityEngine;

public class Sign_day : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject sign1;
    [SerializeField] private GameObject sign2;
    void Start()
    {
        if (GameManager.instance.numday == 2)
        {
            sign1.SetActive(true);
        }
        else if(GameManager.instance.numday == 3)
        {
            sign2.SetActive(true);
        }
    }
}
