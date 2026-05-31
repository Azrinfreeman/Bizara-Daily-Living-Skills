using UnityEngine;

public class PersistentBGM : MonoBehaviour
{
    private static PersistentBGM instance;

    void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one to prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
