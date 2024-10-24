using UnityEngine;

public class CollectibleKey : MonoBehaviour
{
    private GameManager _gm;

    private void Awake()
    {
        // Get the GameManager instance
        _gm = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // If the player collects the key
        if (other.CompareTag("Player"))
        {
            _gm.CollectKey();
            Destroy(gameObject);
        }
    }
}
