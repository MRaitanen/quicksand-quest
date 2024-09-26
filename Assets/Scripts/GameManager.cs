using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }

    // Variables
    public int keysCollected = 0;

    private void Awake()
    {
        // Check if the instance already exists and destroy the new one
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void CollectKey()
    {
        keysCollected++;
    }
}
