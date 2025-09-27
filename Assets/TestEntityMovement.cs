using Unity.VisualScripting;
using UnityEngine;

public class TestEntityMovement : MonoBehaviour
{
    [SerializeField] private GameObject self;
    [SerializeField] private float movespeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool shouldMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            Vector3 velocity = movespeed * direction * Time.deltaTime;
            Vector3 position = self.gameObject.transform.position;

            Vector3 newPosition = position + velocity;
            self.gameObject.transform.position = newPosition; 
        }
    }
}
