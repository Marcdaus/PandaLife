using UnityEngine;

public class CanvasRewardPerssitent : MonoBehaviour
{
    public static CanvasRewardPerssitent instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}
