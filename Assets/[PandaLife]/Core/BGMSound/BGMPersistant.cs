using UnityEngine;

public class BGMPersistant : MonoBehaviour
{
    public static BGMPersistant instance;

    private void Awake()
    {

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
}
