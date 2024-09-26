using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private float showTime = 3f;

    private void Start()
    {
        textObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
            Invoke("HideText", showTime);
        }
    }
}
