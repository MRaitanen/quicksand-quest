using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject textObject;


    private void Start()
    {
        textObject.SetActive(true);
    }
}
