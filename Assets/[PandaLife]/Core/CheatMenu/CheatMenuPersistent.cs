using UnityEngine;

public class CheatMenuPersistent : MonoBehaviour
{
    public static CheatMenuPersistent instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
