using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Number of keys required to unlock the door")]
    [SerializeField] private int requiredKeys = 1;

    private GameManager _gm;

    private void Awake()
    {
        // Get the GameManager instance
        _gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the door
        if (other.CompareTag("Player"))
        {
            _gm.EnterDoor(requiredKeys);
        }
    }
}